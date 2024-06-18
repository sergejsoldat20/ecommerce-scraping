using backend.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace backend.Data
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<Product> Products { get; set; }
		public DbSet<Account> Accounts { get; set; }
		public DbSet<ProductHistory> ProductHistories { get; set; }
		public DbSet<Brand> Brands { get; set; }
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Product>(entity =>
			{
				entity.HasKey(x => x.Id);
				entity.Property(x => x.Name).HasMaxLength(200);
				entity.Property(x => x.ProductUrl).HasMaxLength(300);
				entity.Property(x => x.PhotoUrl).HasMaxLength(300);
				entity.Property(x => x.Price);
				entity.Property(x => x.PriceWithDiscount);
				entity.Property(x => x.Gender);
			});

			modelBuilder.Entity<Brand>(entity =>
			{
				entity.HasKey(x => x.Id);
				entity.Property(x => x.Name).HasMaxLength(40);

				entity
				.HasMany(x => x.Products)
				.WithOne(x => x.Brand)
				.HasForeignKey(x => x.BrandId);

			});

			modelBuilder.Entity<Account>(entity =>
			{
				entity.HasKey(x => x.Id);
				entity.Property(x => x.FirstName).HasMaxLength(50);
				entity.Property(x => x.LastName).HasMaxLength(50);
				entity.Property(x => x.Username).HasMaxLength(50);
				entity.Property(x => x.Email).HasMaxLength(100);
				entity.Property(x => x.Password).HasMaxLength(300);
				entity.Property(x => x.Role).HasMaxLength(20);
				entity.Property(x => x.IsEmailConfirmed);
				entity.Property(x => x.PhoneNumber).HasMaxLength(20);
			});

			modelBuilder.Entity<ProductHistory>(entity =>
			{
				entity.HasKey(x => x.Id);
				entity.Property(x => x.OldValue);
				entity.Property(x => x.NewValue);
				entity.Property(x => x.ChangeGroup);
				entity.Property(x => x.Timestamp);

				entity
				.HasOne(x => x.Product)
				.WithMany(x => x.OldValuesList)
				.HasForeignKey(x => x.ProductId);
			});
 		}
	}
}
