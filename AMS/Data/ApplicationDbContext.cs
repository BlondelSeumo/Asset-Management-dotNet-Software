using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AMS.Models;
using AMS.Models.CommonViewModel;

namespace AMS.Data
{
    public class ApplicationDbContext : AuditableIdentityContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ItemDropdownListViewModel>().HasNoKey();
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }

        public DbSet<SMTPEmailSetting> SMTPEmailSetting { get; set; }
        public DbSet<SendGridSetting> SendGridSetting { get; set; }
        public DbSet<DefaultIdentityOptions> DefaultIdentityOptions { get; set; }
        public DbSet<LoginHistory> LoginHistory { get; set; }

        //AMS
        public DbSet<Asset> Asset { get; set; }
        public DbSet<AssetAssigned> AssetAssigned { get; set; }
        public DbSet<AssetHistory> AssetHistory { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Designation> Designation { get; set; }
        public DbSet<AssetCategorie> AssetCategorie { get; set; }
        public DbSet<AssetSubCategorie> AssetSubCategorie { get; set; }
        public DbSet<AssetStatus> AssetStatus { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<SubDepartment> SubDepartment { get; set; }
        public DbSet<CompanyInfo> CompanyInfo { get; set; }
        public DbSet<UserInfoFromBrowser> UserInfoFromBrowser { get; set; }
        public DbSet<AssetIssue> AssetIssue { get; set; }
        public DbSet<AssetRequest> AssetRequest { get; set; }
        public DbSet<SubscriptionRequest> SubscriptionRequest { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<ManageUserRoles> ManageUserRoles { get; set; }
        public DbSet<ManageUserRolesDetails> ManageUserRolesDetails { get; set; }

        public DbSet<ItemDropdownListViewModel> ItemDropdownListViewModel { get; set; }
    }
}
