using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class ProductionQueryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Productions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DiscoutLessPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Star = table.Column<int>(type: "int", nullable: true),
                    Review = table.Column<int>(type: "int", nullable: true),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Productions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Queries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QueryText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Queries", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Queries",
                columns: new[] { "Id", "CreateTime", "Description", "IsActive", "QueryText", "Type", "UpdateTime" },
                values: new object[,]
                {
                    { new Guid("2194ab2e-001e-4a24-a41e-7fd46d72c08c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Click next page in specified category page", true, "var records = Array.from(document.getElementsByTagName('li'));var paginationElement = records.filter(element => element.parentElement && element.parentElement.children[0].children[0].tagName === 'SPAN' );paginationElement[paginationElement.length - 1].click();paginationElement.map(item => item.innerHTML)", 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("6ba88697-1977-4e1e-90fb-27acbacf5985"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Get product lists from secified category page", true, "Array.from(document.getElementsByClassName('categories-list-box')[0].getElementsByTagName('a')).map(item => {return item.innerHTML})", 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("6c02b5ad-00c3-4f43-b262-bf3a023a795b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Get category urls from main page", true, "Array.from(document.getElementsByClassName('categories-list-box')[0].getElementsByTagName('a')).map(item => {return item.href})", 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("a364e8aa-155c-4467-b9af-1c2d6bae19ad"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Get category names from main page", true, "Array.from(document.getElementsByClassName('categories-list-box')[0].getElementsByTagName('a')).map(item => {return item.innerHTML})", 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("e0492bd1-a2d4-4080-84ab-af1e5220ef61"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check last page in specified category with using css class", true, "var records = Array.from(document.getElementsByTagName('li'));var paginationElement = records.filter(element => element.parentElement && element.parentElement.children[0].children[0].tagName === 'SPAN' );paginationElement[paginationElement.length - 1].classList.value", 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Productions_CategoryId",
                table: "Productions",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Productions");

            migrationBuilder.DropTable(
                name: "Queries");
        }
    }
}
