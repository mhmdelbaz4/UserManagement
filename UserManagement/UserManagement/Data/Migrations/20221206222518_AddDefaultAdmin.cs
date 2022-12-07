using Microsoft.EntityFrameworkCore.Migrations;

namespace UserManagement.Data.Migrations
{
    public partial class AddDefaultAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [Security].[Users] ([Id], [FirstName], [LastName], [ProfilePicture], [UserName], [NormalizedUserName], [Email]," +
                " [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled]" +
                ", [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) " +
                "VALUES (N'e9159671-dfa0-43f9-ab37-aa85ad02c8a2', N'Abdo', N'Elsayed', NULL, N'admin', N'ADMIN', N'admin@test', N'ADMIN@TEST'," +
                " 0, N'AQAAAAEAACcQAAAAEAvNrfBTiRE4AT14KxV5eDqXB8OyCZgg45vQMDK26xn2jhf+2bgFq78YgQPBTPAheQ==', N'F6NXP3WS52NBQDO46VA2RBOL7AHYZQFQ'," +
                " N'5e5b4f0e-70b4-45f0-9343-c083eb5f91b5', NULL, 0, 0, NULL, 1, 0)");

            migrationBuilder.Sql("insert into [Security].[UserRoles] (UserId,RoleId) select 'e9159671-dfa0-43f9-ab37-aa85ad02c8a2' , Id from [Security].[Roles]");
        

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from [Security].[UserRoles] where UserId = 'e9159671-dfa0-43f9-ab37-aa85ad02c8a2'");
        }
    }
}
