
using biometrics_app.Models;
using Microsoft.EntityFrameworkCore;

namespace biometrics_app.Data;
public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {

    }
    // DbSets for the Users and Fingerprint tables
    public DbSet<User> Users { get; set; }
    public DbSet<UserFingerprint> UserFingerprints { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the UserFingerprints table
        modelBuilder.Entity<UserFingerprint>()
            .HasKey(uf => uf.UserId);

        modelBuilder.Entity<UserFingerprint>()
            .HasOne(uf => uf.User)
            .WithOne(u => u.UserFingerprint)
            .HasForeignKey<UserFingerprint>(uf => uf.UserId);
    }
}
