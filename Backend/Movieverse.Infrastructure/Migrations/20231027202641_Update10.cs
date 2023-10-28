using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movieverse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubscriptionId",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BoxOffice_Revenue",
                table: "Statistics",
                type: "numeric(12)",
                precision: 12,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(12,0)",
                oldPrecision: 12);

            migrationBuilder.AlterColumn<decimal>(
                name: "BoxOffice_OpeningWeekendWorldwide",
                table: "Statistics",
                type: "numeric(12)",
                precision: 12,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(12,0)",
                oldPrecision: 12);

            migrationBuilder.AlterColumn<decimal>(
                name: "BoxOffice_OpeningWeekendUs",
                table: "Statistics",
                type: "numeric(12)",
                precision: 12,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(12,0)",
                oldPrecision: 12);

            migrationBuilder.AlterColumn<decimal>(
                name: "BoxOffice_GrossWorldwide",
                table: "Statistics",
                type: "numeric(12)",
                precision: 12,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(12,0)",
                oldPrecision: 12);

            migrationBuilder.AlterColumn<decimal>(
                name: "BoxOffice_GrossUs",
                table: "Statistics",
                type: "numeric(12)",
                precision: 12,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(12,0)",
                oldPrecision: 12);

            migrationBuilder.AlterColumn<decimal>(
                name: "BoxOffice_Budget",
                table: "Statistics",
                type: "numeric(12)",
                precision: 12,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(12,0)",
                oldPrecision: 12);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "Users");

            migrationBuilder.AlterColumn<decimal>(
                name: "BoxOffice_Revenue",
                table: "Statistics",
                type: "numeric(12,0)",
                precision: 12,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(12)",
                oldPrecision: 12);

            migrationBuilder.AlterColumn<decimal>(
                name: "BoxOffice_OpeningWeekendWorldwide",
                table: "Statistics",
                type: "numeric(12,0)",
                precision: 12,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(12)",
                oldPrecision: 12);

            migrationBuilder.AlterColumn<decimal>(
                name: "BoxOffice_OpeningWeekendUs",
                table: "Statistics",
                type: "numeric(12,0)",
                precision: 12,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(12)",
                oldPrecision: 12);

            migrationBuilder.AlterColumn<decimal>(
                name: "BoxOffice_GrossWorldwide",
                table: "Statistics",
                type: "numeric(12,0)",
                precision: 12,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(12)",
                oldPrecision: 12);

            migrationBuilder.AlterColumn<decimal>(
                name: "BoxOffice_GrossUs",
                table: "Statistics",
                type: "numeric(12,0)",
                precision: 12,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(12)",
                oldPrecision: 12);

            migrationBuilder.AlterColumn<decimal>(
                name: "BoxOffice_Budget",
                table: "Statistics",
                type: "numeric(12,0)",
                precision: 12,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(12)",
                oldPrecision: 12);
        }
    }
}
