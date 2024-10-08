using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectTest.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelasPedidoEnderecoEntrega : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "endereco_entrega",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    cep = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    rua = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    numero = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    bairro = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    complemento = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    cidade = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    estado = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    usuario_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: false, comment: "The id of the user who did create"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "When this entity was created in this DB"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this entity was modified the last time"),
                    update_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "The id of the user who did the last modification"),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "The field that identifies that the entity was deleted"),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this entity was deleted in this DB"),
                    deleted_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "The id of the user who did the delete")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_endereco_entrega", x => x.id);
                    table.ForeignKey(
                        name: "FK_endereco_entrega_usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pedido",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    numero_pedido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    data_entrega = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_entrega = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    usuario_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: false, comment: "The id of the user who did create"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "When this entity was created in this DB"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this entity was modified the last time"),
                    update_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "The id of the user who did the last modification"),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "The field that identifies that the entity was deleted"),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this entity was deleted in this DB"),
                    deleted_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "The id of the user who did the delete")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pedido", x => x.id);
                    table.ForeignKey(
                        name: "FK_pedido_usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_endereco_entrega_usuario_id",
                table: "endereco_entrega",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_pedido_usuario_id",
                table: "pedido",
                column: "usuario_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "endereco_entrega");

            migrationBuilder.DropTable(
                name: "pedido");
        }
    }
}
