using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieManagement.Migrations
{
    /// <inheritdoc />
    public partial class uppromotion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bills_promotions_PromotionId",
                table: "bills");

            migrationBuilder.AlterColumn<int>(
                name: "PromotionId",
                table: "bills",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_bills_promotions_PromotionId",
                table: "bills",
                column: "PromotionId",
                principalTable: "promotions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bills_promotions_PromotionId",
                table: "bills");

            migrationBuilder.AlterColumn<int>(
                name: "PromotionId",
                table: "bills",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_bills_promotions_PromotionId",
                table: "bills",
                column: "PromotionId",
                principalTable: "promotions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
