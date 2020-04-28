using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLMediaKitService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tb_User",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SORT_ID = table.Column<long>(nullable: false),
                    CREATE_TIME = table.Column<long>(nullable: false),
                    UPDATE_TIME = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Account = table.Column<string>(maxLength: 5, nullable: false),
                    Password = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_User", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_User_Account",
                table: "Tb_User",
                column: "Account",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tb_User_SORT_ID",
                table: "Tb_User",
                column: "SORT_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tb_User");
        }
    }
}
