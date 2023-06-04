using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSEPractice2.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        private string DirectorRoleId = Guid.NewGuid().ToString();
        private string AdminRoleId = Guid.NewGuid().ToString();
        private string CashierRoleId = Guid.NewGuid().ToString();
        private string AccountantRoleId = Guid.NewGuid().ToString();
        private string InstructorRoleId = Guid.NewGuid().ToString();
        private string AnimatorRoleId = Guid.NewGuid().ToString();
        private string ClientRoleId = Guid.NewGuid().ToString();

        private string UserAdminId = Guid.NewGuid().ToString();
        private string UserDirectorId = Guid.NewGuid().ToString();
        private string UserCashierId = Guid.NewGuid().ToString();
        private string UserAccountantId = Guid.NewGuid().ToString();
        private string UserInstructorId = Guid.NewGuid().ToString();
        private string UserAnimatorId = Guid.NewGuid().ToString();
        private string UserClientId = Guid.NewGuid().ToString();

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            SeedRolesSQL(migrationBuilder);
            SeedUser(migrationBuilder);
            SeedUserRoles(migrationBuilder);

        }

        // Добавление ролей
        private void SeedRolesSQL(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"INSERT INTO ""AspNetRoles"" (""Id"", ""Name"", ""NormalizedName"", ""ConcurrencyStamp"")
                    VALUES ('{AdminRoleId}', 'Administrator', 'ADMINISTRATOR', null);");
            migrationBuilder.Sql($@"INSERT INTO ""AspNetRoles"" (""Id"", ""Name"", ""NormalizedName"", ""ConcurrencyStamp"")
                    VALUES ('{DirectorRoleId}', 'Генеральный директор', 'ГЕНЕРАЛЬНЫЙ ДИРЕКТОР', null);");
            migrationBuilder.Sql($@"INSERT INTO ""AspNetRoles"" (""Id"", ""Name"", ""NormalizedName"", ""ConcurrencyStamp"")
                    VALUES ('{CashierRoleId}', 'Кассир', 'КАССИР', null);");
            migrationBuilder.Sql($@"INSERT INTO ""AspNetRoles"" (""Id"", ""Name"", ""NormalizedName"", ""ConcurrencyStamp"")
                    VALUES ('{AccountantRoleId}', 'Бухгалтер', 'БУХГАЛТЕР', null);");
            migrationBuilder.Sql($@"INSERT INTO ""AspNetRoles"" (""Id"", ""Name"", ""NormalizedName"", ""ConcurrencyStamp"")
                    VALUES ('{InstructorRoleId}', 'Инструктор-спасатель', 'ИНСТРУКТОР-СПАСАТЕЛЬ', null);");
            migrationBuilder.Sql($@"INSERT INTO ""AspNetRoles"" (""Id"", ""Name"", ""NormalizedName"", ""ConcurrencyStamp"")
                    VALUES ('{AnimatorRoleId}', 'Аниматор', 'АНИМАТОР', null);");
            migrationBuilder.Sql($@"INSERT INTO ""AspNetRoles"" (""Id"", ""Name"", ""NormalizedName"", ""ConcurrencyStamp"")
                    VALUES ('{ClientRoleId}', 'Клиент', 'КЛИЕНТ', null);");

        }

        // Добавление пользователей 
        protected void SeedUser(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @$"INSERT INTO ""AspNetUsers"" (""Id"", ""FirstName"", ""LastName"", ""UserName"", ""NormalizedUserName"",
""Email"", ""NormalizedEmail"", ""EmailConfirmed"", ""PasswordHash"", ""SecurityStamp"", ""ConcurrencyStamp"", 
""PhoneNumber"", ""PhoneNumberConfirmed"", ""TwoFactorEnabled"", ""LockoutEnd"", ""LockoutEnabled"", ""AccessFailedCount"")
VALUES
('{UserAdminId}', 'Алексей', 'Крыжовников', 'admin@aqua.com', 'ADMIN@AQUA.COM', 'admin@aqua.com', 'ADMIN@AQUA.COM', true, 'AQAAAAEAACcQAAAAEC0jBqzBta9UlOhedPdJco8I+1KFxeGMCe1VnDRM8yCaGa7nE0DdBWGGp5q6CRKMwA==',
'CIPS32QVK76SN43OABYT2JGI6FCWKLCP', 'd879b9f4-cdcf-4136-bdeb-15c05116e0dc', NULL, false, false, NULL, true, 0), 
('{UserDirectorId}', 'Дмитрий', 'Кондратьев', 'director@aqua.com', 'DIRECTOR@AQUA.COM', 'director@aqua.com', 'DIRECTOR@AQUA.COM', true, 'AQAAAAEAACcQAAAAEDZlN2Q+YqcTqwJlIlQxbEnrc4iz2hNDB/3sXDHdqE7Zaikkf0eW88mTKg2k0O2U7A==',
'P6RXKI5WG2P74MG5DAZNN5GCSMWG2BI7', '4345e109-e1c8-4449-88d4-c68c464eacaa', NULL, false, false, NULL, true, 0), 
('{UserCashierId}', 'Евгения', 'Ивакина', 'cashier@aqua.com', 'CASHIER@AQUA.COM', 'cashier@aqua.com', 'CASHIER@AQUA.COM', true, 'AQAAAAEAACcQAAAAECM2w6zdeGNOxnrjNbJKL3x9QGtwkVqz0XRt1xRmRY/WAO6dYKRBfu1XFnhQcWD+Rg==',
'J2FJB2FXK27BPT7FXZWSFWTWHEAI2BM5', '2f5dd0fa-086e-489b-8a9f-aa1d22ee096c', NULL, false, false, NULL, true, 0), 
('{UserAccountantId}', 'Лидия', 'Сорокина', 'accountant@aqua.com', 'ACCOUNTANT@AQUA.COM', 'accountant@aqua.com', 'ACCOUNTANT@AQUA.COM', true, 'AQAAAAEAACcQAAAAEH2HZ2CuNKrUb8fQizLXVufzv/vJGLtD/JsKFwRN9CTDFi5gIyuVHdKhiaH8yCTmjw==',
'M6TATXMJ44JUKGDZKPGOR3XLJ3B5Y2NM', '1102919f-a613-46c8-a7ba-3d2b58b4e42c', NULL, false, false, NULL, true, 0), 
('{UserInstructorId}', 'Игнат', 'Ильменко', 'instructor@aqua.com', 'INSTRUCTOR@AQUA.COM', 'instructor@aqua.com', 'INSTRUCTOR@AQUA.COM', true, 'AQAAAAEAACcQAAAAEBrhP1uA7r6FOM0B4sIR4epsmW8jvHs6C6zv4FMOLl4Do+/mcHTIv0vGDZ++3lDJdQ==',
'U6LROBMZEMDM7JCS5V32ZNRYSAIB4CK4', '4d938730-412c-4397-8ed3-6e93d78edea2', NULL, false, false, NULL, true, 0), 
('{UserAnimatorId}', 'Ирина', 'Ягодкина', 'animator@aqua.com', 'ANIMATOR@AQUA.COM', 'animator@aqua.com', 'ANIMATOR@AQUA.COM', true, 'AQAAAAEAACcQAAAAEE45Qu5dwBTM3lpUXfecCaWumP5Lu5fEsVoqLScDehNvQxpiCbWEFJs6itcrF3fzBQ==',
'6WZHPF3PF3F5OYHNYBWD2G3GMBKIQDQV', '8133beda-14d3-4bb2-b740-39186395ba0f', NULL, false, false, NULL, true, 0),
('{UserClientId}', 'Гость', 'Гостьев', 'client@aqua.com', 'CLIENT@AQUA.COM', 'client@aqua.com', 'CLIENT@AQUA.COM', true, 'AQAAAAEAACcQAAAAEGReruNguX7uPWbBzgQl1BY3bjtZH2gDD2o1mlX4H/vSCVdI5bbhJ857ADWRa/8eag==',
'MQZZZG5HRPDVFBV3SFCOAQOF6BWEWDXA', '81fc0605-23f7-4b83-babd-cdbd323d36f0', NULL, false, false, NULL, true, 0);") ;
        }

        // Одному пользователю можно назначить несколько ролей, добавив еще запись с другой строкой
        private void SeedUserRoles(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"
                INSERT INTO ""AspNetUserRoles"" (""UserId"", ""RoleId"")
                VALUES ('{UserDirectorId}', '{DirectorRoleId}'), 
                ('{UserAnimatorId}', '{AnimatorRoleId}'),
                ('{UserAccountantId}', '{AccountantRoleId}'),
                ('{UserAdminId}', '{AdminRoleId}'),
                ('{UserCashierId}', '{CashierRoleId}'),
                ('{UserInstructorId}', '{InstructorRoleId}'),
                ('{UserClientId}', '{ClientRoleId}');");
        }
    }
}
