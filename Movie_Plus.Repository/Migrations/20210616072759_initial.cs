using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Movie_Plus.Repository.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "MoviePlus");

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                schema: "MoviePlus",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: false),
                    ProfilePicture = table.Column<byte[]>(nullable: true),
                    Code = table.Column<long>(nullable: true),
                    Puntuation = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movie",
                schema: "MoviePlus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    KindOfMovie = table.Column<string>(nullable: false),
                    Country = table.Column<string>(nullable: false),
                    Actors = table.Column<string>(nullable: false),
                    Ranking = table.Column<double>(nullable: false),
                    PropagandisticAndEconomics = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movie_Local",
                schema: "MoviePlus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Local_Name = table.Column<string>(nullable: false),
                    Rows = table.Column<int>(nullable: false),
                    Columns = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie_Local", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "MoviePlus",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suggestions",
                schema: "MoviePlus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Vigentsuggestion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suggestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "MoviePlus",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserType",
                schema: "MoviePlus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(nullable: false),
                    Discount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CreditCard",
                schema: "MoviePlus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    Number = table.Column<long>(nullable: false),
                    Money = table.Column<double>(nullable: false),
                    Code = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditCard_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalSchema: "MoviePlus",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                schema: "MoviePlus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "MoviePlus",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                schema: "MoviePlus",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "MoviePlus",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                schema: "MoviePlus",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "MoviePlus",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Horary",
                schema: "MoviePlus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MovieId = table.Column<int>(nullable: false),
                    Movie_LocalId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    ReservedTickets = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    PriceInPoints = table.Column<int>(nullable: false),
                    PointsForBuying = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Horary_Movie_MovieId",
                        column: x => x.MovieId,
                        principalSchema: "MoviePlus",
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Horary_Movie_Local_Movie_LocalId",
                        column: x => x.Movie_LocalId,
                        principalSchema: "MoviePlus",
                        principalTable: "Movie_Local",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                schema: "MoviePlus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "MoviePlus",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "MoviePlus",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "MoviePlus",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "MoviePlus",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Buys_Ticket",
                schema: "MoviePlus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    HoraryId = table.Column<int>(nullable: true),
                    CreditCardId = table.Column<int>(nullable: true),
                    NumberOfEntrance = table.Column<int>(nullable: true),
                    Payment = table.Column<int>(nullable: true),
                    Voucher = table.Column<string>(nullable: true),
                    VoucherSeats = table.Column<string>(nullable: true),
                    VoucherUserTypes = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    PayWithPoints = table.Column<bool>(nullable: true),
                    PayCompleted = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buys_Ticket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buys_Ticket_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalSchema: "MoviePlus",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Buys_Ticket_CreditCard_CreditCardId",
                        column: x => x.CreditCardId,
                        principalSchema: "MoviePlus",
                        principalTable: "CreditCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Buys_Ticket_Horary_HoraryId",
                        column: x => x.HoraryId,
                        principalSchema: "MoviePlus",
                        principalTable: "Horary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reserved_Seat",
                schema: "MoviePlus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Buy_TicketId = table.Column<int>(nullable: false),
                    Row = table.Column<int>(nullable: false),
                    Column = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reserved_Seat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reserved_Seat_Buys_Ticket_Buy_TicketId",
                        column: x => x.Buy_TicketId,
                        principalSchema: "MoviePlus",
                        principalTable: "Buys_Ticket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "MoviePlus",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "MoviePlus",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Buys_Ticket_ApplicationUserId",
                schema: "MoviePlus",
                table: "Buys_Ticket",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Buys_Ticket_CreditCardId",
                schema: "MoviePlus",
                table: "Buys_Ticket",
                column: "CreditCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Buys_Ticket_HoraryId",
                schema: "MoviePlus",
                table: "Buys_Ticket",
                column: "HoraryId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditCard_ApplicationUserId",
                schema: "MoviePlus",
                table: "CreditCard",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Horary_MovieId",
                schema: "MoviePlus",
                table: "Horary",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Horary_Movie_LocalId",
                schema: "MoviePlus",
                table: "Horary",
                column: "Movie_LocalId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserved_Seat_Buy_TicketId",
                schema: "MoviePlus",
                table: "Reserved_Seat",
                column: "Buy_TicketId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "MoviePlus",
                table: "Role",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                schema: "MoviePlus",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                schema: "MoviePlus",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                schema: "MoviePlus",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "MoviePlus",
                table: "UserRoles",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reserved_Seat",
                schema: "MoviePlus");

            migrationBuilder.DropTable(
                name: "RoleClaims",
                schema: "MoviePlus");

            migrationBuilder.DropTable(
                name: "Suggestions",
                schema: "MoviePlus");

            migrationBuilder.DropTable(
                name: "User",
                schema: "MoviePlus");

            migrationBuilder.DropTable(
                name: "UserClaims",
                schema: "MoviePlus");

            migrationBuilder.DropTable(
                name: "UserLogins",
                schema: "MoviePlus");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "MoviePlus");

            migrationBuilder.DropTable(
                name: "UserTokens",
                schema: "MoviePlus");

            migrationBuilder.DropTable(
                name: "UserType",
                schema: "MoviePlus");

            migrationBuilder.DropTable(
                name: "Buys_Ticket",
                schema: "MoviePlus");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "MoviePlus");

            migrationBuilder.DropTable(
                name: "CreditCard",
                schema: "MoviePlus");

            migrationBuilder.DropTable(
                name: "Horary",
                schema: "MoviePlus");

            migrationBuilder.DropTable(
                name: "AspNetUsers",
                schema: "MoviePlus");

            migrationBuilder.DropTable(
                name: "Movie",
                schema: "MoviePlus");

            migrationBuilder.DropTable(
                name: "Movie_Local",
                schema: "MoviePlus");
        }
    }
}
