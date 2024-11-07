using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Customer.Api.Migrations
{
    public partial class Init_Update_CustomerDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Customer",
                table: "Customer");

            migrationBuilder.RenameTable(
                name: "Customer",
                newName: "Customers");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_UserName",
                table: "Customers",
                newName: "IX_Customers_UserName");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_EmailAddress",
                table: "Customers",
                newName: "IX_Customers_EmailAddress");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Customer");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_UserName",
                table: "Customer",
                newName: "IX_Customer_UserName");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_EmailAddress",
                table: "Customer",
                newName: "IX_Customer_EmailAddress");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customer",
                table: "Customer",
                column: "Id");
        }
    }
}
