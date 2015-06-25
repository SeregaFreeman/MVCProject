using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCProject.Models
{
    public class Post
    {
        [Required]
        public int PostId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        public ApplicationUser Author { get; set; }

        public List<Tag> Tags { get; set; }

        [Display(Name = "Picture file")]
        public string File { get; set; }

        [Display(Name = "Status")]
        public int Status { get; set; }

    }

    public enum PostStatus
    {
        Sent = 1,
        Approved = 2
    }

    public class CreatePostViewModel
    {
        [Required]
        public int PostId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public List<Tag> AvailableTags { get; set; }

        public List<Tag> Tags { get; set; }

        public int? SelectedTagId { get; set; }

        public int? SelectedDelTagId { get; set; }

        [Display(Name = "Picture file")]
        public string File { get; set; }
    }

    public class EditPostViewModel
    {
        [Required]
        public int PostId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public List<Tag> AvailableTags { get; set; }

        public List<Tag> Tags { get; set; }

        public int? SelectedTagId { get; set; }

        public int? SelectedDelTagId { get; set; }

    }

    public class DetailedPostViewModel
    {
        [Required]
        public int PostId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        public string AuthorName { get; set; }
    }

    public class ApprovePostViewModel
    {
        [Required]
        public int PostId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Picture file")]
        public string File { get; set; }

        [Display(Name = "Status")]
        public int Status { get; set; }
    }
}
