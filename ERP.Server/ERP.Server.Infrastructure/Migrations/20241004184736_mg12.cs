﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mg12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "StockMovements",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "StockMovements");
        }
    }
}
