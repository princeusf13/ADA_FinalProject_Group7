using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_Project.Migrations
{
    /// <inheritdoc />
    public partial class AIInteractionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AIInteractions",
                columns: table => new
                {
                    InteractionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TopicID = table.Column<int>(type: "int", nullable: false),
                    InteractionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaterialID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIInteractions", x => x.InteractionID);
                    table.ForeignKey(
                        name: "FK_AIInteractions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AIInteractions_CourseTopics_TopicID",
                        column: x => x.TopicID,
                        principalTable: "CourseTopics",
                        principalColumn: "TopicID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AIInteractions_Materials_MaterialID",
                        column: x => x.MaterialID,
                        principalTable: "Materials",
                        principalColumn: "MaterialID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AIInteractions_MaterialID",
                table: "AIInteractions",
                column: "MaterialID");

            migrationBuilder.CreateIndex(
                name: "IX_AIInteractions_TopicID",
                table: "AIInteractions",
                column: "TopicID");

            migrationBuilder.CreateIndex(
                name: "IX_AIInteractions_UserId",
                table: "AIInteractions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AIInteractions");
        }
    }
}
