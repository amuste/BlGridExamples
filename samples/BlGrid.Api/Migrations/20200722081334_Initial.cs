using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlGrid.Api.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonId = table.Column<string>(nullable: false),
                    Index = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Balance = table.Column<string>(nullable: true),
                    Picture = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: true),
                    EyeColor = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Registered = table.Column<DateTime>(nullable: true),
                    Latitude = table.Column<decimal>(nullable: true),
                    Longitude = table.Column<decimal>(nullable: true),
                    Greeting = table.Column<string>(nullable: true),
                    FavoriteFruit = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
