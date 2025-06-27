using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithDataSeeder : Migration
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
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    Total = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CartId = table.Column<int>(type: "integer", nullable: false),
                    CustomerName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BranchName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SaleNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CustomerName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SaleDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    BranchName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsCancelled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sales_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SaleItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SaleId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    Total = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SaleItems_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "Password", "Phone", "Role", "Status", "UpdatedAt", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 6, 27, 12, 3, 36, 336, DateTimeKind.Utc).AddTicks(9910), "user.customer@ambev.com.br", "AQAAAAIAAYagAAAAELDlsJmD9BWJUhCLfBOmiQeSENGEFARlFshVJo9mcygDQoQFh0XRXsGUravrjsSfSw==", "99999999999", "Customer", "Active", null, "User.Customer" },
                    { 2, new DateTime(2025, 6, 27, 12, 3, 36, 434, DateTimeKind.Utc).AddTicks(7169), "user.manager@ambev.com.br", "AQAAAAIAAYagAAAAEF8uDwFQsXZV5Gwwe15GWb9qWyZpgzLWMgDhUmv7BXrJLlS/YGwaydzn3EZ1vB6/xw==", "99999999999", "Manager", "Active", null, "User.Manager" },
                    { 3, new DateTime(2025, 6, 27, 12, 3, 36, 582, DateTimeKind.Utc).AddTicks(8623), "user.admin@ambev.com.br", "AQAAAAIAAYagAAAAELp/iFXzZmlWAGU7pp4XOSeWDkUIr5evLTkJRWbblahmvzO4fntUOfJsg65C/7XuCA==", "99999999999", "Admin", "Active", null, "User.Admin" }
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
                name: "IX_Orders_CartId",
                table: "Orders",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleItems_ProductId",
                table: "SaleItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleItems_SaleId",
                table: "SaleItems",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_OrderId",
                table: "Sales",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "SaleItems");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
