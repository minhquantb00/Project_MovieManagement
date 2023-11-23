using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieManagement.Migrations
{
    /// <inheritdoc />
    public partial class updateData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PriceTicket",
                table: "tickets",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<double>(
                name: "TotalMoney",
                table: "bills",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceTicket",
                table: "tickets");

            migrationBuilder.AlterColumn<double>(
                name: "TotalMoney",
                table: "bills",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }
    }
}
