using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace netCoreApp.Repository
{

    public class Context : DbContext
    {
        private static readonly ILoggerFactory _LoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddFilter((log, logLevel) =>
            {
                if (logLevel == LogLevel.Information && log == "Microsoft.EntityFrameworkCore.Database.Command")
                    return true;

                return false;
            });

            builder.AddConsole();
        });

        public Context() :
            base(CreateOptions())
        {
        }

        private static DbContextOptions<Context> CreateOptions()
            => new DbContextOptionsBuilder<Context>()
            .UseSqlServer("Server=localhost\\SQLEXPRESS;Database=IEnumerableIQueryableDatabase;Trusted_Connection=True")
            .UseLoggerFactory(_LoggerFactory)
            .EnableSensitiveDataLogging()  
            .Options;

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<DocumentType> DocumentTypes { get; set; }

        public DbSet<Facture> Factures { get; set; }

        public DbSet<FactureDetail> FactureDetails { get; set; }

        public IEnumerable<T> GetAllIEnumerable<T>()
            where T : class
        {
            return Set<T>();
        }

        public IQueryable<T> GetAllIQueryable<T>()
            where T : class
        {
            return Set<T>();
        }
    }

    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Product> ProductNavigations { get; set; }

    }

    public class DocumentType
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Facture> FactureNavigations { get; set; }

    }

    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public Category CategoryNavigation { get; set; }

        public ICollection<FactureDetail> FactureDetailNavigations { get; set; }
    }

    public class Facture
    {
        public int Id { get; set; }

        public decimal TotalAmmonut { get; set; }

        public int DocumentTypeId { get; set; }

        public DateTime BuyDate { get; set; }

        public DocumentType DocumentTypeNavigation { get; set; }

        public ICollection<FactureDetail> FactureDetailNavigations { get; set; }
    }

    public class FactureDetail
    {
        public int Id { get; set; }

        public int FactureId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public Facture FactureNavigation { get; set; }

        public Product ProductNavigation { get; set; }
    }
}
