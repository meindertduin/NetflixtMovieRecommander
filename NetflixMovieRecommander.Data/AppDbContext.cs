using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetflixMovieRecommander.Models;

namespace NetflixMovieRecommander.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Recommended> Recommendations { get; set; }
        public DbSet<WatchGroup> WatchGroups { get; set; }
        public DbSet<WatchItem> WatchItems { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<NetflixRecommended> NetflixRecommendations { get; set; }
        public DbSet<WatchGroupUserProfile> WatchGroupUserProfiles { get; set; }

        public DbSet<ProfileFile> ProfileFiles { get; set; }
        public DbSet<InboxMessage> InboxMessages { get; set; }

        
        // user inboxes
        public DbSet<UserInbox> UserInboxes { get; set; }
        public DbSet<UserInboxWatchGroupInviteMessage> UserInboxWatchGroupInviteMessages { get; set; }
        public DbSet<UserInboxMessageBase> GeneralMessages { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Rating>()
                .HasOne(x => x.Recommended)
                .WithMany(x => x.Ratings)
                .HasForeignKey(x => x.RecommendedForeignKey);

            modelBuilder.Entity<WatchGroupUserProfile>()
                .HasKey(x => new {x.UserProfileId, x.WatchGroupId});

            modelBuilder.Entity<WatchGroup>()
                .HasOne(x => x.Owner)
                .WithMany(x => x.OwnedWatchGroups)
                .HasForeignKey(x => x.OwnerId);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(x => x.UserProfile)
                .WithOne(x => x.ApplicationUser)
                .HasForeignKey<UserProfile>(x => x.Id);

            modelBuilder.Entity<WatchItem>()
                .HasOne(x => x.WatchGroup)
                .WithMany(x => x.WatchItems)
                .HasForeignKey(x => x.WatchGroupId);

            modelBuilder.Entity<WatchGroup>()
                .Property(x => x.AddedNames)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<ProfileFile>()
                .HasOne(x => x.UserProfile)
                .WithMany(x => x.ProfileFiles)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InboxMessage>()
                .HasOne(x => x.Sender)
                .WithMany(x => x.SendMessages)
                .HasForeignKey(x => x.SenderId);

            modelBuilder.Entity<InboxMessage>()
                .HasOne(x => x.Receiver)
                .WithMany(x => x.InboxMessages)
                .HasForeignKey(x => x.ReceiverId);

            modelBuilder.Entity<UserInboxMessageBase>()
                .HasOne(x => x.UserInbox)
                .WithMany(x => x.GeneralMessages)
                .HasForeignKey(x => x.UserInboxId);

            modelBuilder.Entity<UserInboxWatchGroupInviteMessage>()
                .HasOne(x => x.UserInbox)
                .WithMany(x => x.WatchGroupInviteMessages)
                .HasForeignKey(x => x.UserInboxId);

        }
    }
}