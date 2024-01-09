using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace V59Z7I_SOF_2023241.Migrations
{
    public partial class pics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "Products",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "Products",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
