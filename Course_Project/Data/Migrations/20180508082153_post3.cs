using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Course_Project.Data.Migrations
{
    public partial class post3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Genre",
                table: "Posts",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "GenreName",
                table: "Categories",
                newName: "CategoryName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Posts",
                newName: "Genre");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "Categories",
                newName: "GenreName");
        }
    }
}
