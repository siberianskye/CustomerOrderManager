using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerOrderManager.Business.Migrations
{
    /// <inheritdoc />
    public partial class DefaultGUID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerOrder_UID",
                schema: "CustomerOrderManager",
                table: "CustomerOrder",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "newId()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerOrder_UID",
                schema: "CustomerOrderManager",
                table: "CustomerOrder",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "newId()");
        }
    }
}
