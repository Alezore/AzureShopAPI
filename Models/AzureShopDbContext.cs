using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace AzureShopAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public float Weight { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }  //relation non consenti avec category l annomalie
    }


    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; } = new List<Product>(); //initialise le product parce que sinon tu boude commes une petite copine jalouse 
        //commantaire de plus tard je sais comment le fixer mtn mais je suis le motto "if it works don't fix it" donc ca va rester comme ca
    }


    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public List<Address> Addresses { get; set; } = new(); // Initialisation pour éviter les null et ajouter des clustomer sans adressou order
        public List<Order> Orders { get; set; } = new();
    }

    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; } //optionelle parce que on perpare l immobilier ta vu (en vrai ca marche pas sinon pour mes teste donc voila) CA MARCHE QUAND MEME
    }

    public class AddressCreateDto
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public int CustomerId { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public decimal TotalAmount { get; set; }

    }

    public class OrderItem
    {
        public int Id { get; set; } 
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderCreateDto
    {
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemCreateDto> OrderItems { get; set; }
    }

    public class OrderItemCreateDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class AzureShopDbContext : DbContext
    {
        public AzureShopDbContext(DbContextOptions<AzureShopDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<OrderItem>()
            //  .HasKey(op => new { op.OrderId, op.ProductId });
            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => oi.Id);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
