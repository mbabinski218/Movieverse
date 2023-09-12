using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movieverse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Mov10Metrics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Position",
                table: "Popularity",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Change",
                table: "Popularity",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Views",
                table: "Popularity",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Platforms",
                type: "numeric(4)",
                precision: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,0)",
                oldPrecision: 4);

            migrationBuilder.AlterColumn<int>(
                name: "CurrentPosition",
                table: "Medias",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Views",
                table: "Popularity");

            migrationBuilder.AlterColumn<long>(
                name: "Position",
                table: "Popularity",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "Change",
                table: "Popularity",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Platforms",
                type: "numeric(4,0)",
                precision: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4)",
                oldPrecision: 4);

            migrationBuilder.AlterColumn<int>(
                name: "CurrentPosition",
                table: "Medias",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
