using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reservations.Api.Migrations
{
    /// <inheritdoc />
    public partial class anotherfixtohistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationHistory_Books_BookId",
                table: "ReservationHistory");

            migrationBuilder.DropColumn(
                name: "ReturnDate",
                table: "ReservationHistory");

            migrationBuilder.RenameColumn(
                name: "ReservationDate",
                table: "ReservationHistory",
                newName: "EventDate");

            migrationBuilder.RenameColumn(
                name: "ReservationComment",
                table: "ReservationHistory",
                newName: "Comment");

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "ReservationHistory",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Event",
                table: "ReservationHistory",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationHistory_Books_BookId",
                table: "ReservationHistory",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationHistory_Books_BookId",
                table: "ReservationHistory");

            migrationBuilder.DropColumn(
                name: "Event",
                table: "ReservationHistory");

            migrationBuilder.RenameColumn(
                name: "EventDate",
                table: "ReservationHistory",
                newName: "ReservationDate");

            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "ReservationHistory",
                newName: "ReservationComment");

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "ReservationHistory",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnDate",
                table: "ReservationHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationHistory_Books_BookId",
                table: "ReservationHistory",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");
        }
    }
}
