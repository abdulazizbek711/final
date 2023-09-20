using Microsoft.EntityFrameworkCore;
using ReviewApp.Models;

namespace ReviewApp.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    public DbSet<User?> Users { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Piece> Pieces { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<ReviewTag> ReviewTags { get; set; }
    public DbSet<UserTag> UserTags { get; set; }
    public DbSet<UserActions> UserActions { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Review>()
            .HasOne(r => r.Piece)
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.PieceId);

        // Define the ReviewTag join table
        modelBuilder.Entity<ReviewTag>()
            .HasKey(rt => new { rt.ReviewId, rt.TagId });

        modelBuilder.Entity<ReviewTag>()
            .HasOne(rt => rt.Review)
            .WithMany(r => r.ReviewTags)
            .HasForeignKey(rt => rt.ReviewId);

        modelBuilder.Entity<ReviewTag>()
            .HasOne(rt => rt.Tag)
            .WithMany(t => t.ReviewTags)
            .HasForeignKey(rt => rt.TagId);

        // Define the UserTag join table
        modelBuilder.Entity<UserTag>()
            .HasKey(ut => new { ut.UserId, ut.TagId });

        modelBuilder.Entity<UserTag>()
            .HasOne(ut => ut.User)
            .WithMany(u => u.UserTags)
            .HasForeignKey(ut => ut.UserId);

        modelBuilder.Entity<UserTag>()
            .HasOne(ut => ut.Tag)
            .WithMany(t => t.UserTags)
            .HasForeignKey(ut => ut.TagId);
    
        // Define other entity configurations as needed for your application

        base.OnModelCreating(modelBuilder);
    }

    
}