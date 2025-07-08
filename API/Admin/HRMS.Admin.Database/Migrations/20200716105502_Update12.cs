using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMS.Admin.Database.Migrations
{
    public partial class Update12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationConfiguration_NotificationTypes_NotificationTyp~",
                table: "NotificationConfiguration");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationTypes_Categories_CategoryMasterId",
                table: "NotificationTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationTypes",
                table: "NotificationTypes");

            migrationBuilder.RenameTable(
                name: "NotificationTypes",
                newName: "NotificationType");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationTypes_CategoryMasterId",
                table: "NotificationType",
                newName: "IX_NotificationType_CategoryMasterId");

            migrationBuilder.AlterColumn<string>(
                name: "SystemInfo",
                table: "NotificationType",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NotificationTypeId",
                table: "NotificationType",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "NotificationDescription",
                table: "NotificationType",
                type: "varchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NotificationCode",
                table: "NotificationType",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "NotificationType",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "NotificationType",
                type: "varchar(100)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "NotificationType",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "NotificationType",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationType",
                table: "NotificationType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationConfiguration_NotificationType_NotificationType~",
                table: "NotificationConfiguration",
                column: "NotificationTypeId",
                principalTable: "NotificationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationType_Categories_CategoryMasterId",
                table: "NotificationType",
                column: "CategoryMasterId",
                principalTable: "Categories",
                principalColumn: "CategoryMasterId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationConfiguration_NotificationType_NotificationType~",
                table: "NotificationConfiguration");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationType_Categories_CategoryMasterId",
                table: "NotificationType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationType",
                table: "NotificationType");

            migrationBuilder.RenameTable(
                name: "NotificationType",
                newName: "NotificationTypes");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationType_CategoryMasterId",
                table: "NotificationTypes",
                newName: "IX_NotificationTypes_CategoryMasterId");

            migrationBuilder.AlterColumn<string>(
                name: "SystemInfo",
                table: "NotificationTypes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NotificationTypeId",
                table: "NotificationTypes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "NotificationDescription",
                table: "NotificationTypes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NotificationCode",
                table: "NotificationTypes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "NotificationTypes",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "NotificationTypes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "NotificationTypes",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "NotificationTypes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationTypes",
                table: "NotificationTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationConfiguration_NotificationTypes_NotificationTyp~",
                table: "NotificationConfiguration",
                column: "NotificationTypeId",
                principalTable: "NotificationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationTypes_Categories_CategoryMasterId",
                table: "NotificationTypes",
                column: "CategoryMasterId",
                principalTable: "Categories",
                principalColumn: "CategoryMasterId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
