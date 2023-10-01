using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movieverse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update1 : Migration
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

            migrationBuilder.AlterColumn<Guid>(
                name: "LogoId",
                table: "Platforms",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
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

            migrationBuilder.AlterColumn<Guid>(
                name: "LogoId",
                table: "Platforms",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
