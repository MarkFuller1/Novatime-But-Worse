using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TODOApi.Migrations
{
    public partial class AddLookupTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    employee_hours_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "text", nullable: true),
                    last_name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employees", x => x.employee_hours_id);
                });

            migrationBuilder.CreateTable(
                name: "hours",
                columns: table => new
                {
                    hours_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    week_ending_date = table.Column<long>(type: "bigint", nullable: false),
                    hours = table.Column<int>(type: "integer", nullable: false),
                    employee_hours_id = table.Column<int>(type: "integer", nullable: false),
                    employee_hoursemployee_hours_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hours", x => x.hours_id);
                    table.ForeignKey(
                        name: "fk_hours_employees_employee_hoursemployee_hours_id",
                        column: x => x.employee_hoursemployee_hours_id,
                        principalTable: "employees",
                        principalColumn: "employee_hours_id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_hours_employee_hoursemployee_hours_id",
                table: "hours",
                column: "employee_hoursemployee_hours_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hours");

            migrationBuilder.DropTable(
                name: "employees");
        }
    }
}
