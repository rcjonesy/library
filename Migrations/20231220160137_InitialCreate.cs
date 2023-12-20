using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LoncotesLibrary.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaterialTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CheckOutDays = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patrons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    CheckoutId = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patrons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaterialName = table.Column<string>(type: "text", nullable: false),
                    MaterialTypeId = table.Column<int>(type: "integer", nullable: false),
                    CheckoutId = table.Column<int>(type: "integer", nullable: false),
                    GenreId = table.Column<int>(type: "integer", nullable: false),
                    OutOfCirculationSince = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    FineAmount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materials_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Materials_MaterialTypes_MaterialTypeId",
                        column: x => x.MaterialTypeId,
                        principalTable: "MaterialTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Checkouts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaterialId = table.Column<int>(type: "integer", nullable: false),
                    PatronId = table.Column<int>(type: "integer", nullable: false),
                    CheckoutDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Paid = table.Column<bool>(type: "boolean", nullable: false),
                    FineAmount = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checkouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Checkouts_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Checkouts_Patrons_PatronId",
                        column: x => x.PatronId,
                        principalTable: "Patrons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Action" },
                    { 2, "Adventure" },
                    { 3, "Biography" },
                    { 4, "Comedy" },
                    { 5, "Crime" },
                    { 6, "Drama" },
                    { 7, "Fantasy" },
                    { 8, "Historical Fiction" },
                    { 9, "Horror" },
                    { 10, "Mystery" },
                    { 11, "Romance" },
                    { 12, "Science Fiction" },
                    { 13, "Self-Help" },
                    { 14, "Thriller" },
                    { 15, "Travel" }
                });

            migrationBuilder.InsertData(
                table: "MaterialTypes",
                columns: new[] { "Id", "CheckOutDays", "Name" },
                values: new object[,]
                {
                    { 1, 14, "Book" },
                    { 2, 7, "DVD" },
                    { 3, 7, "Magazine" },
                    { 4, 14, "Journal" },
                    { 5, 7, "Newspaper" }
                });

            migrationBuilder.InsertData(
                table: "Patrons",
                columns: new[] { "Id", "Address", "CheckoutId", "Email", "FirstName", "IsActive", "LastName" },
                values: new object[,]
                {
                    { 1, "123 Main St", 0, "john@example.com", "John", true, "Smith" },
                    { 2, "456 Elm St", 0, "alice@example.com", "Alice", true, "Johnson" },
                    { 3, "789 Oak St", 0, "michael@example.com", "Michael", true, "Garcia" },
                    { 4, "101 Pine St", 0, "emily@example.com", "Emily", true, "Brown" },
                    { 5, "210 Cedar St", 0, "sophia@example.com", "Sophia", true, "Davis" }
                });

            migrationBuilder.InsertData(
                table: "Materials",
                columns: new[] { "Id", "CheckoutId", "FineAmount", "GenreId", "MaterialName", "MaterialTypeId", "OutOfCirculationSince" },
                values: new object[,]
                {
                    { 1, 0, 0m, 1, "The Matrix", 1, null },
                    { 2, 0, 0m, 2, "The Catcher in the Rye", 2, null },
                    { 3, 0, 0m, 3, "National Geographic", 3, null },
                    { 4, 0, 0m, 4, "Science", 4, null },
                    { 5, 0, 0m, 5, "New York Times", 5, null },
                    { 6, 0, 0m, 6, "Inception", 1, null },
                    { 7, 0, 0m, 7, "To Kill a Mockingbird", 2, null },
                    { 8, 0, 0m, 8, "Time", 3, null },
                    { 9, 0, 0m, 9, "Nature", 4, null },
                    { 10, 0, 0m, 10, "The Washington Post", 5, null }
                });

            migrationBuilder.InsertData(
                table: "Checkouts",
                columns: new[] { "Id", "CheckoutDate", "FineAmount", "MaterialId", "Paid", "PatronId", "ReturnDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 12, 20, 10, 1, 37, 260, DateTimeKind.Local).AddTicks(7777), null, 1, false, 1, null },
                    { 2, new DateTime(2023, 12, 20, 10, 1, 37, 260, DateTimeKind.Local).AddTicks(7809), null, 2, false, 2, null },
                    { 3, new DateTime(2023, 12, 20, 10, 1, 37, 260, DateTimeKind.Local).AddTicks(7811), null, 3, false, 3, null },
                    { 4, new DateTime(2023, 12, 20, 10, 1, 37, 260, DateTimeKind.Local).AddTicks(7840), null, 4, false, 4, null },
                    { 5, new DateTime(2023, 12, 20, 10, 1, 37, 260, DateTimeKind.Local).AddTicks(7844), null, 5, false, 5, null },
                    { 6, new DateTime(2023, 12, 20, 10, 1, 37, 260, DateTimeKind.Local).AddTicks(7846), null, 6, false, 1, null },
                    { 7, new DateTime(2023, 12, 20, 10, 1, 37, 260, DateTimeKind.Local).AddTicks(7848), null, 7, false, 2, null },
                    { 8, new DateTime(2023, 12, 20, 10, 1, 37, 260, DateTimeKind.Local).AddTicks(7849), null, 8, false, 3, null },
                    { 9, new DateTime(2023, 12, 20, 10, 1, 37, 260, DateTimeKind.Local).AddTicks(7851), null, 9, false, 4, null },
                    { 10, new DateTime(2023, 12, 20, 10, 1, 37, 260, DateTimeKind.Local).AddTicks(7853), null, 10, false, 5, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Checkouts_MaterialId",
                table: "Checkouts",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Checkouts_PatronId",
                table: "Checkouts",
                column: "PatronId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_GenreId",
                table: "Materials",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_MaterialTypeId",
                table: "Materials",
                column: "MaterialTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Checkouts");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Patrons");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "MaterialTypes");
        }
    }
}
