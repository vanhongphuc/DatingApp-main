using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace API.Data
{
    //ket noi giua database voi entities
    public class DataContext : IdentityDbContext<AppUser,AppRole,int, 
    IdentityUserClaim<int>,AppUserRole, IdentityUserLogin<int>,
    IdentityRoleClaim<int>,IdentityUserToken<int>>
    {
    
        public DataContext(DbContextOptions options) : base(options)
        {
        }

            // tao cau noi giua app va csdl
     
        public DbSet <UserLike> Likes { get; set; }

        public DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserLike>()
            .HasKey(k => new{k.SourceUserId,k.LikedUserId});


            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

             builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
            

            //xác định mối quan hệ 
            builder.Entity<UserLike>()
            .HasOne(s=>s.SourceUser)
            .WithMany(l=>l.LikedUsers)
            .HasForeignKey(s=>s.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
            .HasOne(u=>u.Recipient)
            .WithMany(m=>m.MessagesReceived)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
            .HasOne(u=>u.Sender)
            .WithMany(m=>m.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}