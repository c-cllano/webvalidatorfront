using Microsoft.Data.SqlClient;
using Process.Domain.Interfaces;
using System.Data;

namespace Process.Application.Context
{
    public class DapperUnitOfWork : IUnitOfWork
    {

        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;

        public DapperUnitOfWork(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public int Commit()
        {
            try
            {
                //Successfully committed
                _transaction.Commit();
                return 1;
            }
            catch
            {
                //Indicates a failure
                _transaction.Rollback();
                return 0;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = _connection.BeginTransaction();
            }
        }

        public void Dispose()
        {
            _transaction?.Commit();
            _connection?.Close();
        }

    }
}
