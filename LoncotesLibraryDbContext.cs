using Microsoft.EntityFrameworkCore;
using LoncotesLibrary.Models;

public class LoncotesLibraryDbContext : DbContext
{
    //Each Db set corresponds to and represents a table in the database
    public DbSet<Checkout> Checkouts { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<MaterialType> MaterialTypes { get; set; }
    public DbSet<Patron> Patrons { get; set; }

    public LoncotesLibraryDbContext(DbContextOptions<LoncotesLibraryDbContext> context) : base(context)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed data with Genre types
        modelBuilder.Entity<Genre>().HasData(new Genre[]
        {
            new Genre { Id = 1, Name = "Action" },
            new Genre { Id = 2, Name = "Adventure" },
            new Genre { Id = 3, Name = "Biography" },
            new Genre { Id = 4, Name = "Comedy" },
            new Genre { Id = 5, Name = "Crime" },
            new Genre { Id = 6, Name = "Drama" },
            new Genre { Id = 7, Name = "Fantasy" },
            new Genre { Id = 8, Name = "Historical Fiction" },
            new Genre { Id = 9, Name = "Horror" },
            new Genre { Id = 10, Name = "Mystery" },
            new Genre { Id = 11, Name = "Romance" },
            new Genre { Id = 12, Name = "Science Fiction" },
            new Genre { Id = 13, Name = "Self-Help" },
            new Genre { Id = 14, Name = "Thriller" },
            new Genre { Id = 15, Name = "Travel" }
        // Add more genres as needed
        });

        modelBuilder.Entity<Patron>().HasData(new Patron[]
        {
            new Patron { Id = 1, FirstName = "John", LastName = "Smith", Address = "123 Main St", Email = "john@example.com", IsActive = true },
            new Patron { Id = 2, FirstName = "Alice", LastName = "Johnson", Address = "456 Elm St", Email = "alice@example.com", IsActive = true },
            new Patron { Id = 3, FirstName = "Michael", LastName = "Garcia", Address = "789 Oak St", Email = "michael@example.com", IsActive = true },
            new Patron { Id = 4, FirstName = "Emily", LastName = "Brown", Address = "101 Pine St", Email = "emily@example.com", IsActive = true },
            new Patron { Id = 5, FirstName = "Sophia", LastName = "Davis", Address = "210 Cedar St", Email = "sophia@example.com", IsActive = true }
        });

        modelBuilder.Entity<MaterialType>().HasData(new MaterialType[]
        {
            new MaterialType { Id = 1, Name = "Book", CheckOutDays = 14 },
            new MaterialType { Id = 2, Name = "DVD", CheckOutDays = 7 },
            new MaterialType { Id = 3, Name = "Magazine", CheckOutDays = 7 },
            new MaterialType { Id = 4, Name = "Journal", CheckOutDays = 14 },
            new MaterialType { Id = 5, Name = "Newspaper", CheckOutDays = 7 }
        });

        // 10 instances of Material with names corresponding to types
        modelBuilder.Entity<Material>().HasData(new Material[]
        {
            new Material { Id = 1, MaterialName = "The Matrix", MaterialTypeId = 1, GenreId = 1, OutOfCirculationSince = null }, // DVD
            new Material { Id = 2, MaterialName = "The Catcher in the Rye", MaterialTypeId = 2, GenreId = 2, OutOfCirculationSince = null }, // Book
            new Material { Id = 3, MaterialName = "National Geographic", MaterialTypeId = 3, GenreId = 3, OutOfCirculationSince = null }, // Magazine
            new Material { Id = 4, MaterialName = "Science", MaterialTypeId = 4, GenreId = 4, OutOfCirculationSince = null }, // Journal
            new Material { Id = 5, MaterialName = "New York Times", MaterialTypeId = 5, GenreId = 5, OutOfCirculationSince = null }, // Newspaper
            new Material { Id = 6, MaterialName = "Inception", MaterialTypeId = 1, GenreId = 6, OutOfCirculationSince = null }, // DVD
            new Material { Id = 7, MaterialName = "To Kill a Mockingbird", MaterialTypeId = 2, GenreId = 7, OutOfCirculationSince = null }, // Book
            new Material { Id = 8, MaterialName = "Time", MaterialTypeId = 3, GenreId = 8, OutOfCirculationSince = null }, // Magazine
            new Material { Id = 9, MaterialName = "Nature", MaterialTypeId = 4, GenreId = 9, OutOfCirculationSince = null }, // Journal
            new Material { Id = 10, MaterialName = "The Washington Post", MaterialTypeId = 5, GenreId = 10, OutOfCirculationSince = null } // Newspaper
        });

        // 5 instances of Checkout
        modelBuilder.Entity<Checkout>().HasData(new Checkout[]
        {
            new Checkout { Id = 1, MaterialId = 1, PatronId = 1, CheckoutDate = DateTime.Now, ReturnDate = null },
            new Checkout { Id = 2, MaterialId = 2, PatronId = 2, CheckoutDate = DateTime.Now, ReturnDate = null },
            new Checkout { Id = 3, MaterialId = 3, PatronId = 3, CheckoutDate = DateTime.Now, ReturnDate = null },
            new Checkout { Id = 4, MaterialId = 4, PatronId = 4, CheckoutDate = DateTime.Now, ReturnDate = null },
            new Checkout { Id = 5, MaterialId = 5, PatronId = 5, CheckoutDate = DateTime.Now, ReturnDate = null },
            new Checkout { Id = 6, MaterialId = 6, PatronId = 1, CheckoutDate = DateTime.Now, ReturnDate = null },
            // new Checkout { Id = 7, MaterialId = 7, PatronId = 2, CheckoutDate = DateTime.Now, ReturnDate = null },
            // new Checkout { Id = 8, MaterialId = 8, PatronId = 3, CheckoutDate = DateTime.Now, ReturnDate = null },
            // new Checkout { Id = 9, MaterialId = 9, PatronId = 4, CheckoutDate = DateTime.Now, ReturnDate = null },
            // new Checkout { Id = 10, MaterialId = 10, PatronId = 5, CheckoutDate = DateTime.Now, ReturnDate = null }
        });


    }

}