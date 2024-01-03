using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieManagement.Migrations
{
    /// <inheritdoc />
    public partial class addv30 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movies_rates_RateId",
                table: "movies");

            migrationBuilder.AlterColumn<int>(
                name: "RateId",
                table: "movies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_movies_rates_RateId",
                table: "movies",
                column: "RateId",
                principalTable: "rates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movies_rates_RateId",
                table: "movies");

            migrationBuilder.AlterColumn<int>(
                name: "RateId",
                table: "movies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_movies_rates_RateId",
                table: "movies",
                column: "RateId",
                principalTable: "rates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
