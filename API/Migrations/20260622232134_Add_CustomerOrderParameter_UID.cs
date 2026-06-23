using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerOrderManager.Business.Migrations
{
    /// <inheritdoc />
    public partial class Add_CustomerOrderParameter_UID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerOrderParameters",
                table: "CustomerOrderParameters");

            migrationBuilder.RenameTable(
                name: "CustomerOrderParameters",
                newName: "CustomerOrderParameter",
                newSchema: "CustomerOrderManager");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerOrderParameter_UID",
                schema: "CustomerOrderManager",
                table: "CustomerOrderParameter",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "newId()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerOrderParameter",
                schema: "CustomerOrderManager",
                table: "CustomerOrderParameter",
                column: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerOrderParameter",
                schema: "CustomerOrderManager",
                table: "CustomerOrderParameter");

            migrationBuilder.DropColumn(
                name: "CustomerOrderParameter_UID",
                schema: "CustomerOrderManager",
                table: "CustomerOrderParameter");

            migrationBuilder.RenameTable(
                name: "CustomerOrderParameter",
                schema: "CustomerOrderManager",
                newName: "CustomerOrderParameters");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerOrderParameters",
                table: "CustomerOrderParameters",
                column: "ID");
        }
    }
}
