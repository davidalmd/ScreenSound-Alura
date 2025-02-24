using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScreenSound.Migrations
{
    /// <inheritdoc />
    public partial class PopularTabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("Artistas", new string[] { "Nome", "Bio", "FotoPerfil" }, new object[] { "Djavan", "Djavan foi um grande cantor brasileiro", "https://djavan.com.br/content/uploads/2018/11/oceano-672x0-c-default.jpg" });
            migrationBuilder.InsertData("Artistas", new string[] { "Nome", "Bio", "FotoPerfil" }, new object[] { "Elis Regina", "Elis Regina foi uma grande cantora brasileira", "https://fly.metroimg.com/upload/q_85,w_700/https://uploads.metroimg.com/wp-content/uploads/2016/12/06150505/elisreginacabelo.jpg" });
            migrationBuilder.InsertData("Artistas", new string[] { "Nome", "Bio", "FotoPerfil" }, new object[] { "Tim Maia", "Tim Maia foi um grande cantor brasileiro", "https://dicionariompb.com.br/wp-content/uploads/2021/04/191074645_5530875286983580_5116672794115711532_n.jpg" });
            migrationBuilder.InsertData("Artistas", new string[] { "Nome", "Bio", "FotoPerfil" }, new object[] { "Jorge Vercílio", "Jorge Vercílio foi um grande cantor brasileiro", "https://caras.com.br/media/uploads/2024/10/jorge-vercillo-cantor.jpg" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Artistas");
        }
    }
}
