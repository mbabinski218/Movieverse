using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movieverse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<double>(
                name: "BasicStatistics_Rating",
                table: "Popularity",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<double>(
                name: "BasicStatistics_Rating",
                table: "Medias",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "BasicStatistics_Rating",
                table: "Episode",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<float>(
                name: "BasicStatistics_Rating",
                table: "Popularity",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<float>(
                name: "BasicStatistics_Rating",
                table: "Medias",
                type: "real",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "BasicStatistics_Rating",
                table: "Episode",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }
    }
}
