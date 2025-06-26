using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithProductSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Image = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SaleNumber = table.Column<string>(type: "text", nullable: false),
                    SaleDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientName = table.Column<string>(type: "text", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchName = table.Column<string>(type: "text", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IsCancelled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaleItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    ProductName = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    Total = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    SaleId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleItems_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CartId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Discount = table.Column<decimal>(type: "numeric", nullable: false),
                    Total = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "CreatedAt", "Description", "Image", "Price", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Cervejas", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cerveja Brahma Duplo Malte Lata 350ml - Pack com 12 unidades", "https://www.ambev.com.br/images/products/brahma-duplo-malte-350ml.jpg", 35.88m, "Brahma Duplo Malte 350ml", null },
                    { 2, "Cervejas Premium", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cerveja Stella Artois Long Neck 275ml - Pack com 6 unidades", "https://www.ambev.com.br/images/products/stella-artois-275ml.jpg", 31.90m, "Stella Artois 275ml", null },
                    { 3, "Cervejas Premium", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cerveja Corona Extra Long Neck 330ml - Pack com 6 unidades", "https://www.ambev.com.br/images/products/corona-extra-330ml.jpg", 39.90m, "Corona Extra 330ml", null },
                    { 4, "Refrigerantes", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Refrigerante Guaraná Antarctica 2L - Pack com 6 unidades", "https://www.ambev.com.br/images/products/guarana-antarctica-2l.jpg", 42.00m, "Guaraná Antarctica 2L", null },
                    { 5, "Refrigerantes", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Refrigerante Pepsi Lata 350ml - Pack com 12 unidades", "https://www.ambev.com.br/images/products/pepsi-350ml.jpg", 28.80m, "Pepsi 350ml", null },
                    { 6, "Águas Saborizadas", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bebida H2OH! Limão 500ml - Pack com 6 unidades", "https://www.ambev.com.br/images/products/h2oh-limao-500ml.jpg", 24.00m, "H2OH! Limão 500ml", null },
                    { 7, "Isotônicos", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Isotônico Gatorade Laranja 500ml - Pack com 6 unidades", "https://www.ambev.com.br/images/products/gatorade-laranja-500ml.jpg", 32.90m, "Gatorade Laranja 500ml", null },
                    { 8, "Cervejas Premium", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cerveja Budweiser Lata 350ml - Pack com 12 unidades", "https://www.ambev.com.br/images/products/budweiser-350ml.jpg", 47.88m, "Budweiser 350ml", null },
                    { 9, "Cervejas", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cerveja Original Garrafa 600ml - Pack com 12 unidades", "https://www.ambev.com.br/images/products/original-600ml.jpg", 89.88m, "Original 600ml", null },
                    { 10, "Cervejas", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cerveja Antarctica Sub Zero Lata 473ml - Pack com 12 unidades", "https://www.ambev.com.br/images/products/antarctica-subzero-473ml.jpg", 35.88m, "Antarctica Sub Zero 473ml", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleItems_SaleId",
                table: "SaleItems",
                column: "SaleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "SaleItems");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
