using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsignmentSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_defaultCommissionPercentage_Vendor_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DefaultCommissionPercentage",
                table: "Vendors",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultCommissionPercentage",
                table: "Vendors");
        }
    }
}
