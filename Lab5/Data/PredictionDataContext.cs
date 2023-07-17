namespace Lab5.Data
{
    using Lab5.Models;
    using Microsoft.EntityFrameworkCore;

    public class PredictionDataContext : DbContext
    {
        public PredictionDataContext(DbContextOptions<PredictionDataContext> options) : base(options)
        {
        }

        public DbSet<Prediction> Predictions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set table names to singular
            modelBuilder.Entity<Prediction>().ToTable("Prediction");
        }
    }
}
