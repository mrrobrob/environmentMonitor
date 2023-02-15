using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace environmentMonitor.Migrations
{
    /// <inheritdoc />
    public partial class SplitDataSourceFromDataRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DataSourceId",
                table: "DataRecords",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "DataSources",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MachineId = table.Column<string>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSources", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataRecords_DataSourceId",
                table: "DataRecords",
                column: "DataSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSources_MachineId_Key",
                table: "DataSources",
                columns: new[] { "MachineId", "Key" });

            migrationBuilder.Sql(@"
                INSERT INTO DataSources (MachineId, Key) 
                SELECT MachineId, Key FROM DataRecords
            ");

            migrationBuilder.Sql(@"
                UPDATE DataRecords 
                    SET DataSourceId = ds.[Id] 
                FROM DataSources ds, DataRecords dr 
                WHERE dr.[MachineId] = ds.[MachineId] AND dr.[Key] = ds.[Key]
            ");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "DataRecords");

            migrationBuilder.DropColumn(
                name: "MachineId",
                table: "DataRecords");

            migrationBuilder.AddForeignKey(
                name: "FK_DataRecords_DataSources_DataSourceId",
                table: "DataRecords",
                column: "DataSourceId",
                principalTable: "DataSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataRecords_DataSources_DataSourceId",
                table: "DataRecords");

            migrationBuilder.DropTable(
                name: "DataSources");

            migrationBuilder.DropIndex(
                name: "IX_DataRecords_DataSourceId",
                table: "DataRecords");

            migrationBuilder.DropColumn(
                name: "DataSourceId",
                table: "DataRecords");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "DataRecords",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MachineId",
                table: "DataRecords",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
