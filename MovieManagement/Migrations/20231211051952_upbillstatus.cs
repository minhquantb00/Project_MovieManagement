using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieManagement.Migrations
{
    /// <inheritdoc />
    public partial class upbillstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BillStatusId",
                table: "bills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "billStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_billStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bills_BillStatusId",
                table: "bills",
                column: "BillStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_bills_billStatuses_BillStatusId",
                table: "bills",
                column: "BillStatusId",
                principalTable: "billStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bills_billStatuses_BillStatusId",
                table: "bills");

            migrationBuilder.DropTable(
                name: "billStatuses");

            migrationBuilder.DropIndex(
                name: "IX_bills_BillStatusId",
                table: "bills");

            migrationBuilder.DropColumn(
                name: "BillStatusId",
                table: "bills");
        }
    }
}
