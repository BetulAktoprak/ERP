using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mg4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Operation",
                table: "Outboxes",
                newName: "OperationName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OperationName",
                table: "Outboxes",
                newName: "Operation");
        }
    }
}
