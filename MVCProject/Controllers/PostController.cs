using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCProject.Models;

namespace MVCProject.Controllers
{
    public class PostController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Post
        [Authorize]
        public ActionResult Index()
        {

            if (HttpContext.User.IsInRole("user"))
            {
                List<Post> userPostsList = new List<Post>();
                var currentUser = db.Users.First(u => u.UserName == User.Identity.Name);
                userPostsList = db.Posts.Where(r => r.Author.Id == currentUser.Id).Include(p => p.Author).Include(p => p.Tags).ToList();
                return View(userPostsList);
            }
            else if (HttpContext.User.IsInRole("admin"))
            {
                List<Post> postsList = new List<Post>();
                postsList = db.Posts.Include(p => p.Author).ToList();
                return View(postsList);
            }
            return View();
            
        }

        // GET: Post/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Post post = db.Posts.Include(p => p.Author).First(p => p.PostId == id);

            //  DetailedPostViewModel post = new DetailedPostViewModel { Title = post1.Title, AuthorName = author.UserName, Description = post1.Description, PostId = (int) id };

            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Post/Create
        [Authorize]
        public ActionResult Create()
        {
            var currentUser = db.Users.First(u => u.UserName == User.Identity.Name);
            if (currentUser != null)
            {
                ViewBag.AvailableTags = new SelectList(db.Tags, "Id", "Name");
                return View();
            }
            return RedirectToAction("LogOff", "Account");
        }

        // POST: Post/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(CreatePostViewModel CreatePost, HttpPostedFileBase picture)
        {
            var currentUser = db.Users.First(u => u.UserName == User.Identity.Name);
            if (ModelState.IsValid)
            {
                Post post = new Post();
                post.Title = CreatePost.Title;
                post.Description = CreatePost.Description;
                post.Status = (int)PostStatus.Sent;
                post.Author = currentUser;
                CreatePost.AvailableTags = db.Tags.ToList<Tag>();
                post.Tags = CreatePost.AvailableTags;
                DateTime current = DateTime.Now;

                if (picture != null)
                {
                    // Получаем расширение
                    string ext = picture.FileName.Substring(picture.FileName.LastIndexOf('.'));
                    // сохраняем файл по определенному пути на сервере
                    string path = current.ToString("dd/MM/yyyy H:mm:ss").Replace(":", "_").Replace("/", ".") + ext;
                    picture.SaveAs(Server.MapPath("~/Uploads/" + path));
                    post.File = path;
                }

                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(CreatePost);
        }

        // GET: Post/Edit/5

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Include(p => p.Tags).First(p => p.PostId == id);
            EditPostViewModel editPost = new EditPostViewModel { PostId = post.PostId, Title = post.Title, Description = post.Description };
            if (post == null)
            {
                return HttpNotFound();
            }
            editPost.Tags = post.Tags;
            editPost.AvailableTags = db.Tags.Where(x => x.Posts.All(p => p.PostId != post.PostId)).ToList<Tag>();
            return View(editPost);
        }

        // POST: Post/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "PostId,Title,Description,SelectedTagId,SelectedDelTagId")] EditPostViewModel EditPost)
        {
            EditPost.Tags = db.Tags.Where(x => x.Posts.Any(p => p.PostId == EditPost.PostId)).ToList<Tag>();
            EditPost.AvailableTags = db.Tags.Where(x => x.Posts.All(p => p.PostId != EditPost.PostId)).ToList<Tag>();
            if (ModelState.IsValid)
            {

                var post = db.Posts.Include(p => p.Author).Include(p => p.Tags).First(p => p.PostId == EditPost.PostId);
                post.Title = EditPost.Title;
                post.Description = EditPost.Description;
                if (post.Tags == null)
                    post.Tags = new List<Tag>();

                if (EditPost.SelectedTagId != -1)
                    post.Tags.Add(db.Tags.First(t => t.TagId == EditPost.SelectedTagId));
                if (EditPost.SelectedDelTagId != -1)
                    post.Tags.Remove(db.Tags.First(t => t.TagId == EditPost.SelectedDelTagId));

                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(EditPost);
        }

        // действия для модератора
        [HttpGet]
        [Authorize(Roles = "moderator")]
        public ActionResult Approve()
        {
            var posts = db.Posts.Include(r => r.Author.UserName).Where(r => r.Status != (int)PostStatus.Approved);

            return View(posts);
        }

        [HttpPost]
        [Authorize(Roles = "moderator")]
        public ActionResult Approve(int? postId)
        {
            if (postId == null)
            {
                return RedirectToAction("Approve");
            }

            Post post = db.Posts.Find(postId);
            if (post == null)
            {
                return RedirectToAction("Approve");
            }

            post.Status = (int)PostStatus.Approved;

            db.Entry(post).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Approve");
        }
        // GET: Post/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
