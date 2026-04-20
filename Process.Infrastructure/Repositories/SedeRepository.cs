using Dapper;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Infrastructure.Data;
using System.Data;

namespace Process.Infrastructure.Repositories
{
    public class SedeRepository(SQLServerConnectionFactory connectionFactory): ISedeRepository
    {
        private readonly IDbConnection _connection = connectionFactory.CreateConnection();
        public async Task<Sede?> GetSedeById(long sedeId)
        {
            string sql = "SELECT SedeId, Nombre, ConvenioId, Area, Activo, CreatedAt, UpdatedAt, Correo, Telefono, IdComercio, IdTerminal FROM referencia.Sede WHERE SedeId = @Id";
            return await _connection.QueryFirstOrDefaultAsync<Sede>(sql, new { Id = sedeId });
        }
    }
}
