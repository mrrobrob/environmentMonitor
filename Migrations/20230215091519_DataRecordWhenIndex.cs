using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace environmentMonitor.Migrations
{
    /// <inheritdoc />
    public partial class DataRecordWhenIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DataRecords_When",
                table: "DataRecords",
                column: "When");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DataRecords_When",
                table: "DataRecords");
        }
    }
}
