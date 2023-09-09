using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Movieverse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Mov6Mov8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EpisodeReviews");

            migrationBuilder.DropTable(
                name: "MediaReviews");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Platforms",
                type: "numeric(4)",
                precision: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,0)",
                oldPrecision: 4);

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    Rating = table.Column<short>(type: "smallint", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ByCritic = table.Column<bool>(type: "boolean", nullable: false),
                    Spoiler = table.Column<bool>(type: "boolean", nullable: false),
                    Modified = table.Column<bool>(type: "boolean", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    Banned = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Medias_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Medias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MediaId",
                table: "Reviews",
                column: "MediaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Platforms",
                type: "numeric(4,0)",
                precision: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4)",
                oldPrecision: 4);

            migrationBuilder.CreateTable(
                name: "EpisodeReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Banned = table.Column<bool>(type: "boolean", nullable: false),
                    ByCritic = table.Column<bool>(type: "boolean", nullable: false),
                    Content = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    EpisodeId = table.Column<int>(type: "integer", nullable: false),
                    Modified = table.Column<bool>(type: "boolean", nullable: false),
                    Rating = table.Column<short>(type: "smallint", nullable: false),
                    Spoiler = table.Column<bool>(type: "boolean", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpisodeReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EpisodeReviews_Episode_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Banned = table.Column<bool>(type: "boolean", nullable: false),
                    ByCritic = table.Column<bool>(type: "boolean", nullable: false),
                    Content = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Modified = table.Column<bool>(type: "boolean", nullable: false),
                    Rating = table.Column<short>(type: "smallint", nullable: false),
                    Spoiler = table.Column<bool>(type: "boolean", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaReviews_Medias_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Medias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EpisodeReviews_EpisodeId",
                table: "EpisodeReviews",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaReviews_MediaId",
                table: "MediaReviews",
                column: "MediaId");
        }
    }
}
