namespace MVCProject.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using MVCProject.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MVCProject.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "MVCProject.Models.ApplicationDbContext";
        }

        protected override void Seed(MVCProject.Models.ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // создаем две роли
            var role1 = new IdentityRole { Name = "admin" };
            var role2 = new IdentityRole { Name = "user" };
            var role3 = new IdentityRole { Name = "moderator" };
            
            // добавляем роли в бд
            roleManager.Create(role1);
            roleManager.Create(role2);
            roleManager.Create(role3);

            // создаем пользователей
            var admin = new ApplicationUser { Email = "admin@mail.com", UserName = "admin@mail.com" };
            string password = "2wsx#EDC";
            var result = userManager.Create(admin, password);

            // если создание пользователя прошло успешно
            if (result.Succeeded)
            {
                // добавляем для пользователя роль
                userManager.AddToRole(admin.Id, role1.Name);
            }

            var moderator = new ApplicationUser { Email = "moderator@mail.ru", UserName = "moderator@mail.ru" };
            password = "1qaz@WSX";
            result = userManager.Create(moderator, password);

            if (result.Succeeded)
            {
                // добавляем для пользователя роль
                userManager.AddToRole(moderator.Id, role3.Name);
            }

            base.Seed(context);
        }
    }
}
