using LoncotesLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using LoncotesLibrary.Models.DTOs;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// allows passing datetimes without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<LoncotesLibraryDbContext>(builder.Configuration["LoncotesLibraryDbConnectionString"]);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



//----------------------------------------------------------------


app.MapGet("/api/materials", (LoncotesLibraryDbContext db, int? materialTypeId, int? genreId) =>
{
    return db.Materials
        .Where(m =>
            (materialTypeId == null || m.MaterialTypeId == materialTypeId) &&
            (genreId == null || m.GenreId == genreId) &&
            m.OutOfCirculationSince == null
        )
        .Select(m => new MaterialDTO
        {
            Id = m.Id,
            MaterialName = m.MaterialName,
            MaterialTypeId = m.MaterialTypeId,
            MaterialType = new MaterialTypeDTO
            {
                Id = m.MaterialType.Id,
                Name = m.MaterialType.Name,
                CheckOutDays = m.MaterialType.CheckOutDays
            },
            CheckoutId = m.CheckoutId,
            Checkouts = m.Checkouts.Select(co => new CheckoutDTO
            {
                Id = co.Id,
                MaterialId = co.MaterialId,
                PatronId = co.PatronId,
                CheckoutDate = co.CheckoutDate,
                ReturnDate = co.ReturnDate
            }).ToList(), // Closing parenthesis for Checkouts list projection

            GenreId = m.GenreId,
            Genre = new GenreDTO
            {
                Id = m.GenreId,
                Name = m.Genre.Name // Correcting to fetch the Name property from Genre entity
            },
            OutOfCirculationSince = m.OutOfCirculationSince
        }).ToList();
});



//----------------------------------------------------------------

app.MapGet("/api/materials/available", (LoncotesLibraryDbContext db) =>
{
    return db.Materials
    .Where(m => m.OutOfCirculationSince == null)
    .Where(m => m.Checkouts.All(co => co.ReturnDate != null))
    .Select(material => new MaterialDTO
    {
        Id = material.Id,
        MaterialName = material.MaterialName,
        MaterialTypeId = material.MaterialTypeId,
        GenreId = material.GenreId,
        OutOfCirculationSince = material.OutOfCirculationSince
    })
    .ToList();
});

//----------------------------------------------------------------

//Add a Material
app.MapPost("/api/materials", (LoncotesLibraryDbContext db, Material newMaterial) =>
{

    // newMaterial.MaterialName = "Wallflowers";
    // newMaterial.MaterialTypeId = 1;
    // newMaterial.GenreId = 1;

    try
    {
        db.Materials.Add(newMaterial);
        db.SaveChanges();
        return Results.Created($"/api/reservations/{newMaterial.Id}", newMaterial);
    }
    catch (DbUpdateException)
    {
        return Results.BadRequest("Invalid data submitted");
    }
});
//----------------------------------------------------------------

app.MapPost("/api/checkouts", (LoncotesLibraryDbContext db, Checkout newCheckout) =>
{
    var DateNow = DateTime.Now;

    try
    {
        newCheckout.CheckoutDate = DateNow;

        db.Checkouts.Add(newCheckout);
        db.SaveChanges();
        return Results.Created($"/api/checkouts/{newCheckout.Id}", newCheckout);
    }
    catch (DbUpdateException)
    {
        return Results.BadRequest("Invalid data submitted");
    }
});

//----------------------------------------------------------------



app.MapGet("/api/patrons/{id}", (LoncotesLibraryDbContext db, int id) =>
{
    return db.Patrons
        .Include(p => p.Checkouts)
            .ThenInclude(co => co.Material)
                .ThenInclude(m => m.MaterialType)
        .Select(p => new PatronDTO
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Address = p.Address,
            Email = p.Email,
            IsActive = p.IsActive,
            Checkouts = p.Checkouts.Select(co => new CheckoutDTO
            {
                Id = co.Id,
                // Include other CheckoutDTO properties as needed
                Material = new MaterialDTO
                {
                    Id = co.Material.Id,
                    MaterialName = co.Material.MaterialName,
                    MaterialTypeId = co.Material.MaterialTypeId,
                    MaterialType = new MaterialTypeDTO
                    {
                        Id = co.Material.MaterialType.Id,
                        Name = co.Material.MaterialType.Name,
                        // Include other MaterialTypeDTO properties here
                    },
                    GenreId = co.Material.GenreId,
                    Genre = new GenreDTO
                    {
                        Id = co.Material.Genre.Id,
                        Name = co.Material.Genre.Name,
                        // Include other GenreDTO properties here
                    },
                    OutOfCirculationSince = co.Material.OutOfCirculationSince

                }
            }).ToList()
        })
        .FirstOrDefault(p => p.Id == id);


});

//----------------------------------------------------------------

app.MapPut("/api/patrons/{id}/deactivate", (LoncotesLibraryDbContext db, int id) =>
{
    var patronToUpdate = db.Patrons.FirstOrDefault(p => p.Id == id);

    if (patronToUpdate == null)
    {
        return Results.NotFound();
    }

    patronToUpdate.IsActive = false;

    try
    {
        db.SaveChanges();
        return Results.NoContent(); // Successfully updated
    }
    catch (DbUpdateException)
    {
        return Results.BadRequest("Failed to update patron status");
    }

});

//----------------------------------------------------------------

app.MapPut("/api/patrons/{id}/activate", (LoncotesLibraryDbContext db, int id) =>
{
    var patronToUpdate = db.Patrons.FirstOrDefault(p => p.Id == id);

    if (patronToUpdate == null)
    {
        return Results.NotFound();
    }

    patronToUpdate.IsActive = true;

    try
    {
        db.SaveChanges();
        return Results.NoContent(); // Successfully updated
    }
    catch (DbUpdateException)
    {
        return Results.BadRequest("Failed to update patron status");
    }

});

//----------------------------------------------------------------

app.MapPut("/api/materials/{id}/soft-delete", (LoncotesLibraryDbContext db, int id) =>
{
    // Retrieve the material by ID
    var materialToUpdate = db.Materials.FirstOrDefault(m => m.Id == id);

    if (materialToUpdate == null)
    {
        return Results.NotFound();
    }

    // Set the OutOfCirculationSince property to DateTime.Now to mark as out of circulation
    materialToUpdate.OutOfCirculationSince = DateTime.Now;

    try
    {
        db.SaveChanges();
        return Results.NoContent(); // Successfully updated
    }
    catch (DbUpdateException)
    {
        return Results.BadRequest("Failed to update material status");
    }
});

//----------------------------------------------------------------
// Return a material

app.MapPut("/api/materials/return/{checkoutId}", (LoncotesLibraryDbContext db, int checkoutId) =>
{
    // Retrieve the checkout from the database
    var checkout = db.Checkouts.FirstOrDefault(c => c.Id == checkoutId);

    if (checkout == null)
    {
        return Results.NotFound("Checkout not found.");
    }

    // Set the return date of the checkout to today's date
    checkout.ReturnDate = DateTime.Today;

    // Save changes to the database
    db.SaveChanges();

    return Results.Ok("Checkout marked as returned successfully.");
});


//----------------------------------------------------------------

app.MapPut("/api/patrons/{id}", (LoncotesLibraryDbContext db, int id, Patron patron) =>
{
    Patron patronToUpdate = db.Patrons.SingleOrDefault(patron => patron.Id == id);
    if (patronToUpdate == null)
    {
        return Results.NotFound();
    }
    patronToUpdate.Address = patron.Address;
    patronToUpdate.Email = patron.Email;
    // patronToUpdate.IsActive = patron.IsActive;

    db.SaveChanges();
    return Results.NoContent();
});


//----------------------------------------------------------------

app.MapGet("/api/patrons", (LoncotesLibraryDbContext db) =>
{
    return db.Patrons
    .Select(p => new PatronDTO
    {
        Id = p.Id,
        FirstName = p.FirstName,
        LastName = p.LastName,
        Email = p.Email,
        IsActive = p.IsActive
    }).ToList();
});

//----------------------------------------------------------------

app.MapGet("/api/genres", (LoncotesLibraryDbContext db) =>
{
    return db.Genres
    .Select(g => new GenreDTO
    {
        Id = g.Id,
        Name = g.Name,

    }).ToList();
});

//----------------------------------------------------------------



// checkouts, materials, material types
app.MapGet("/api/materials/{id}", (LoncotesLibraryDbContext db, int id) =>
{
    return db.Materials
        .Include(m => m.Checkouts)
            .ThenInclude(co => co.Patron)
        .Include(m => m.MaterialType)
        .Include(m => m.Genre)
        .Select(m => new MaterialDTO
        {
            Id = m.Id,
            MaterialName = m.MaterialName,
            MaterialTypeId = m.MaterialTypeId,
            MaterialType = new MaterialTypeDTO
            {
                Id = m.MaterialType.Id,
                Name = m.MaterialType.Name
            },
            CheckoutId = m.CheckoutId, // Assuming CheckoutId is a foreign key in Material
            Checkouts = m.Checkouts.Select(co => new CheckoutDTO
            {
                Id = co.Id,
                CheckoutDate = co.CheckoutDate,
                ReturnDate = co.ReturnDate,
                Patron = new PatronDTO
                {
                    Id = co.Patron.Id,
                    FirstName = co.Patron.FirstName,
                    LastName = co.Patron.LastName,
                    Address = co.Patron.Address,
                    Email = co.Patron.Email
                }
            }).ToList(),
            GenreId = m.GenreId,
            Genre = new GenreDTO
            {
                Id = m.Genre.Id,
                Name = m.Genre.Name
            },
            OutOfCirculationSince = m.OutOfCirculationSince
        })
        .FirstOrDefault(m => m.Id == id);


});


//----------------------------------------------------------------
//Get Material Types

app.MapGet("/api/materialtypes", (LoncotesLibraryDbContext db) =>
{
    // This sets up an endpoint for handling GET requests to /api/materialtypes

    // db is an instance of LoncotesLibraryDbContext, which allows access to the database
    // Inside this endpoint, it queries the database to retrieve Material Types

    return db.MaterialTypes
        // Accesses the MaterialTypes DbSet in the LoncotesLibraryDbContext

        .Select(mt => new MaterialTypeDTO
        {
            // For each MaterialType in the DbSet, it projects it to a new MaterialTypeDTO

            Id = mt.Id,                 // Copies the Id property from MaterialType to MaterialTypeDTO
            Name = mt.Name,             // Copies the Name property from MaterialType to MaterialTypeDTO
            CheckOutDays = mt.CheckOutDays // Copies the CheckOutDays property from MaterialType to MaterialTypeDTO
        })
        .ToList(); // Executes the query and materializes the result as a List<MaterialTypeDTO>
});


//----------------------------------------------------------------

//get: define a route that responds to HTTP GET requests.
//("/api/checkouts is the endpoint where the GET request will be sent.
//The second parameter is a function that describes what should happen when a GET request 
//is made to the specified route. This function takes parameters according to your requirements 
//(often the DbContext for database access) and contains the logic to handle the request.
app.MapGet("/api/checkouts", (LoncotesLibraryDbContext db) =>
{
    //db.Checkouts retrieves all records from the Checkouts table.
    return db.Checkouts
    .Include(co => co.Material)
    //.Select() method is used to project each Checkout entity to a CheckoutDTO object.
    .Select(co => new CheckoutDTO
    {
        Id = co.Id,
        MaterialId = co.MaterialId,
        Material = new MaterialDTO
        {
            MaterialName = co.Material.MaterialName
        },
        PatronId = co.PatronId,
        CheckoutDate = co.CheckoutDate,
        ReturnDate = co.ReturnDate

    }).ToList();
});

//----------------------------------------------------------------
app.MapGet("/api/checkouts/overdue", (LoncotesLibraryDbContext db) =>
{
    return db.Checkouts
    .Include(p => p.Patron)
    .Include(co => co.Material)
    .ThenInclude(m => m.MaterialType)
    .Where(co =>
        (DateTime.Today - co.CheckoutDate).Days >
        co.Material.MaterialType.CheckOutDays &&
        co.ReturnDate == null)
        .Select(co => new CheckoutDTO
        {
            Id = co.Id,
            MaterialId = co.MaterialId,
            Material = new MaterialDTO
            {
                Id = co.Material.Id,
                MaterialName = co.Material.MaterialName,
                MaterialTypeId = co.Material.MaterialTypeId,
                MaterialType = new MaterialTypeDTO
                {
                    Id = co.Material.MaterialTypeId,
                    Name = co.Material.MaterialType.Name,
                    CheckOutDays = co.Material.MaterialType.CheckOutDays
                },
                GenreId = co.Material.GenreId,
                OutOfCirculationSince = co.Material.OutOfCirculationSince
            },
            PatronId = co.PatronId,
            Patron = new PatronDTO
            {
                Id = co.Patron.Id,
                FirstName = co.Patron.FirstName,
                LastName = co.Patron.LastName,
                Address = co.Patron.Address,
                Email = co.Patron.Email,
                IsActive = co.Patron.IsActive
            },
            CheckoutDate = co.CheckoutDate,
            ReturnDate = co.ReturnDate
        })
    .ToList();
});


app.Run();

