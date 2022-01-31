using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeduShop.Model.Models;

namespace TeduShop.Data
{
    //this class include asp.net.identity.entityframwork
    public class TeduShopDbContext : IdentityDbContext<ApplicationUser>
    {
        public TeduShopDbContext():base("TeduShopConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;    
        }
        public DbSet<Footer> Footers { set; get; }
        public DbSet<Menu> Menus { set; get; }
        public DbSet<MenuGroup> MenuGroups { set; get; }
        public DbSet<Order> Orders { set; get; }
        public DbSet<OrderDetail> OrderDetails { set; get; }
        public DbSet<Page> Pages { set; get; }
        public DbSet<Post> Posts { set; get; }
        public DbSet<PostCategory> PostCategories { set; get; }
        public DbSet<PostTag> PostTags { set; get; }
        public DbSet<Product> Products { set; get; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<ProductCategory> ProductCategories { set; get; }
        public DbSet<ProductTag> ProductTags { set; get; }
        public DbSet<Slide> Slides { set; get; }
        public DbSet<SupportOnline> SupportOnlines { set; get; }
        public DbSet<SystemConfig> SystemConfigs { set; get; }

        public DbSet<Tag> Tags { set; get; }

        public DbSet<Error> Errors { get; set; }

        public DbSet<VisitorStatistic> VisitorStatistics { set; get; }
        public DbSet<ContactDetail> contactDetails { get; set; }

        public DbSet<ApplicationGroup> ApplicationGroups { get; set; } 
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationRoleGroup> ApplicationRoleGroups { get; set; }
        public DbSet<ApplicationUserGroup> applicationUserGroups { get; set; }

       //dùng Identity thì phải có cái thằng này !
        public static TeduShopDbContext Create()
        {
            return new TeduShopDbContext();
        }    


        //ghi de phuong thuc trong lop ke thua
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //set cac cai key cho cac thang nay de phan quyen
            modelBuilder.Entity<IdentityUserRole>().HasKey(i => new {i.UserId,i.RoleId }).ToTable("ApplicationUserRoles");
            modelBuilder.Entity<IdentityUserLogin>().HasKey(i => i.UserId).ToTable("ApplicationUserLogins");
            modelBuilder.Entity<IdentityRole>().ToTable("ApplicationRoles");
            modelBuilder.Entity<IdentityUserClaim>().HasKey(i => i.UserId).ToTable("ApplicationUserClaims");
        }
    }
}
