using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyAPISolution.SampleAPI.Migrations
{
    /// <inheritdoc />
    public partial class TblPegawai_Tambah_KolomTelp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NoTelp",
                table: "Pegawais",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Pegawais",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoTelp",
                table: "Pegawais");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Pegawais");
        }
    }
}
