namespace TeduShop.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TeduShop.Model.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TeduShop.Data.TeduShopDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TeduShop.Data.TeduShopDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            var hasher = new PasswordHasher();
            var user = new ApplicationUser();
            user.FullName = "admin";
            user.Email = "ngtquoc@gmail.com";
            user.PhoneNumber = "0799339291";
            user.UserName = "admin";
            user.PasswordHash = hasher.HashPassword("123456");

            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}
