using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerOrderManager.Business.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "CustomerOrderManager");

            migrationBuilder.CreateTable(
                name: "CustomerOrder",
                schema: "CustomerOrderManager",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    OrderValue = table.Column<double>(type: "float", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerOrder_UID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOrder", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerOrder",
                schema: "CustomerOrderManager");
        }
    }
}
