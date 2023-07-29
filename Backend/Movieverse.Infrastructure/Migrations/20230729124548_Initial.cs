using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Movieverse.Domain.Common.Types;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Movieverse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:role", "director,writer,actor,creator,producer,composer,cinematographer,editor,art_director,costume_designer,makeup_artist,sound_designer,other");

            migrationBuilder.CreateTable(
                name: "Award",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ImageId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Award", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Path = table.Column<string>(type: "text", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    MediaCount = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CurrentPosition = table.Column<long>(type: "bigint", nullable: true),
                    PosterId = table.Column<Guid>(type: "uuid", nullable: true),
                    TrailerId = table.Column<Guid>(type: "uuid", nullable: true),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    SequelId = table.Column<Guid>(type: "uuid", nullable: true),
                    SequelTitle = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PrequelId = table.Column<Guid>(type: "uuid", nullable: true),
                    PrequelTitle = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Details_Runtime = table.Column<int>(type: "integer", nullable: true),
                    Details_Certificate = table.Column<int>(type: "integer", nullable: true),
                    Details_Storyline = table.Column<string>(type: "text", nullable: true),
                    Details_Tagline = table.Column<string>(type: "text", nullable: true),
                    Details_ReleaseDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Details_CountryOfOrigin = table.Column<string>(type: "text", nullable: true),
                    Details_Language = table.Column<string>(type: "text", nullable: true),
                    Details_FilmingLocations = table.Column<string>(type: "text", nullable: true),
                    TechnicalSpecs_Color = table.Column<string>(type: "text", nullable: true),
                    TechnicalSpecs_AspectRatio = table.Column<string>(type: "text", nullable: true),
                    TechnicalSpecs_SoundMix = table.Column<string>(type: "text", nullable: true),
                    TechnicalSpecs_Camera = table.Column<string>(type: "text", nullable: true),
                    TechnicalSpecs_NegativeFormat = table.Column<string>(type: "text", nullable: true),
                    BasicStatistics_Rating = table.Column<int>(type: "integer", nullable: true),
                    BasicStatistics_Votes = table.Column<long>(type: "bigint", nullable: true),
                    BasicStatistics_UserReviews = table.Column<long>(type: "bigint", nullable: true),
                    BasicStatistics_CriticReviews = table.Column<long>(type: "bigint", nullable: true),
                    BasicStatistics_InWatchlistCount = table.Column<long>(type: "bigint", nullable: true),
                    SeasonCount = table.Column<int>(type: "integer", nullable: true),
                    EpisodeCount = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Information_FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Information_LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Information_Age = table.Column<int>(type: "integer", nullable: false),
                    LifeHistory_BirthPlace = table.Column<string>(type: "text", nullable: true),
                    LifeHistory_BirthDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LifeHistory_DeathPlace = table.Column<string>(type: "text", nullable: true),
                    LifeHistory_DeathDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LifeHistory_CareerStart = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LifeHistory_CareerEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Biography = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    FunFacts = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Platforms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LogoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(2)", precision: 2, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platforms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Information_FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Information_LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Information_Age = table.Column<int>(type: "integer", nullable: false),
                    AvatarId = table.Column<Guid>(type: "uuid", nullable: true),
                    PersonId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GenreMediaIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GenreId = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenreMediaIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GenreMediaIds_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaContentIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaContentIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaContentIds_Medias_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Medias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaGenreIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false),
                    GenreId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaGenreIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaGenreIds_Medias_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Medias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaPlatformIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlatformId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaPlatformIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaPlatformIds_Medias_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Medias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    Rating = table.Column<short>(type: "smallint", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ByCritic = table.Column<bool>(type: "boolean", nullable: false),
                    Spoiler = table.Column<bool>(type: "boolean", nullable: false),
                    Modified = table.Column<bool>(type: "boolean", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    Banned = table.Column<bool>(type: "boolean", nullable: false),
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Season",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SeriesId = table.Column<Guid>(type: "uuid", nullable: false),
                    SeasonNumber = table.Column<long>(type: "bigint", nullable: false),
                    EpisodeCount = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Season", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Season_Medias_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Medias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false),
                    PersonId = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<Role>(type: "role", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Staff_Medias_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Medias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Statistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false),
                    BoxOffice_Budget = table.Column<decimal>(type: "numeric", nullable: false),
                    BoxOffice_Revenue = table.Column<decimal>(type: "numeric", nullable: false),
                    BoxOffice_GrossUs = table.Column<decimal>(type: "numeric", nullable: false),
                    BoxOffice_GrossWorldwide = table.Column<decimal>(type: "numeric", nullable: false),
                    BoxOffice_OpeningWeekendUs = table.Column<decimal>(type: "numeric", nullable: false),
                    BoxOffice_OpeningWeekendWorldwide = table.Column<decimal>(type: "numeric", nullable: false),
                    BoxOffice_Theaters = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Statistics_Medias_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Medias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonContentIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonContentIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonContentIds_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlatformMediaIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlatformId = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformMediaIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformMediaIds_Platforms_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "Platforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsInWatchlist = table.Column<bool>(type: "boolean", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaInfo_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Episode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SeasonId = table.Column<int>(type: "integer", nullable: false),
                    EpisodeNumber = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Details_Runtime = table.Column<int>(type: "integer", nullable: true),
                    Details_Certificate = table.Column<int>(type: "integer", nullable: true),
                    Details_Storyline = table.Column<string>(type: "text", nullable: true),
                    Details_Tagline = table.Column<string>(type: "text", nullable: true),
                    Details_ReleaseDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Details_CountryOfOrigin = table.Column<string>(type: "text", nullable: true),
                    Details_Language = table.Column<string>(type: "text", nullable: true),
                    Details_FilmingLocations = table.Column<string>(type: "text", nullable: true),
                    BasicStatistics_Rating = table.Column<int>(type: "integer", nullable: false),
                    BasicStatistics_Votes = table.Column<long>(type: "bigint", nullable: false),
                    BasicStatistics_UserReviews = table.Column<long>(type: "bigint", nullable: false),
                    BasicStatistics_CriticReviews = table.Column<long>(type: "bigint", nullable: false),
                    BasicStatistics_InWatchlistCount = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Episode_Season_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Season",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Popularity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StatisticsId = table.Column<int>(type: "integer", nullable: false),
                    BasicStatistics_Rating = table.Column<int>(type: "integer", nullable: false),
                    BasicStatistics_Votes = table.Column<long>(type: "bigint", nullable: false),
                    BasicStatistics_UserReviews = table.Column<long>(type: "bigint", nullable: false),
                    BasicStatistics_CriticReviews = table.Column<long>(type: "bigint", nullable: false),
                    BasicStatistics_InWatchlistCount = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Position = table.Column<long>(type: "bigint", nullable: true),
                    Change = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Popularity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Popularity_Statistics_StatisticsId",
                        column: x => x.StatisticsId,
                        principalTable: "Statistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StatisticsAward",
                columns: table => new
                {
                    StatisticsId = table.Column<int>(type: "integer", nullable: false),
                    AwardId = table.Column<int>(type: "integer", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Place = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatisticsAward", x => new { x.StatisticsId, x.AwardId });
                    table.ForeignKey(
                        name: "FK_StatisticsAward_Award_AwardId",
                        column: x => x.AwardId,
                        principalTable: "Award",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StatisticsAward_Statistics_StatisticsId",
                        column: x => x.StatisticsId,
                        principalTable: "Statistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EpisodeContentIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EpisodeId = table.Column<int>(type: "integer", nullable: false),
                    ContentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpisodeContentIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EpisodeContentIds_Episode_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EpisodeReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    Rating = table.Column<short>(type: "smallint", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ByCritic = table.Column<bool>(type: "boolean", nullable: false),
                    Spoiler = table.Column<bool>(type: "boolean", nullable: false),
                    Modified = table.Column<bool>(type: "boolean", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    Banned = table.Column<bool>(type: "boolean", nullable: false),
                    EpisodeId = table.Column<int>(type: "integer", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Episode_SeasonId",
                table: "Episode",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_EpisodeContentIds_EpisodeId",
                table: "EpisodeContentIds",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_EpisodeReviews_EpisodeId",
                table: "EpisodeReviews",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_GenreMediaIds_GenreId",
                table: "GenreMediaIds",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaContentIds_MediaId",
                table: "MediaContentIds",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaGenreIds_MediaId",
                table: "MediaGenreIds",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaInfo_UserId",
                table: "MediaInfo",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaPlatformIds_MediaId",
                table: "MediaPlatformIds",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaReviews_MediaId",
                table: "MediaReviews",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonContentIds_PersonId",
                table: "PersonContentIds",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformMediaIds_PlatformId",
                table: "PlatformMediaIds",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_Popularity_StatisticsId",
                table: "Popularity",
                column: "StatisticsId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Season_SeriesId",
                table: "Season",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_MediaId",
                table: "Staff",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_MediaId",
                table: "Statistics",
                column: "MediaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StatisticsAward_AwardId",
                table: "StatisticsAward",
                column: "AwardId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contents");

            migrationBuilder.DropTable(
                name: "EpisodeContentIds");

            migrationBuilder.DropTable(
                name: "EpisodeReviews");

            migrationBuilder.DropTable(
                name: "GenreMediaIds");

            migrationBuilder.DropTable(
                name: "MediaContentIds");

            migrationBuilder.DropTable(
                name: "MediaGenreIds");

            migrationBuilder.DropTable(
                name: "MediaInfo");

            migrationBuilder.DropTable(
                name: "MediaPlatformIds");

            migrationBuilder.DropTable(
                name: "MediaReviews");

            migrationBuilder.DropTable(
                name: "PersonContentIds");

            migrationBuilder.DropTable(
                name: "PlatformMediaIds");

            migrationBuilder.DropTable(
                name: "Popularity");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "StatisticsAward");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Episode");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Platforms");

            migrationBuilder.DropTable(
                name: "Award");

            migrationBuilder.DropTable(
                name: "Statistics");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Season");

            migrationBuilder.DropTable(
                name: "Medias");
        }
    }
}
