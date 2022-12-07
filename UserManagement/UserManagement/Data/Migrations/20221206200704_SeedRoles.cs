using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace UserManagement.Data.Migrations
{
    public partial class SeedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table : "Roles",
                columns : new[] {"Id" , "Name", "NormalizedName", "ConcurrencyStamp" },
                values : new object[] {Guid.NewGuid().ToString(),"User", "User".ToString(), Guid.NewGuid().ToString() },
                schema : "Security"
                );

            migrationBuilder.InsertData(
            table: "Roles",
            columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
            values: new object[] { Guid.NewGuid().ToString(), "Admin", "Admin".ToString(), Guid.NewGuid().ToString() },
            schema: "Security"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete From [Security].[Roles]");
        }
    }
}
