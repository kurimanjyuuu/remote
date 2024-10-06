using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TutoRealBE;
using CC = TutoRealCommon.CommonConst;
using Dapper;
using TutoRealCommon;

namespace TutoRealDA
{
    public abstract class TutoRealBaseDA: IDBAccess
    {
        protected readonly IConfiguration _configuration;
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public TutoRealBaseDA(IDbConnection connection, IConfiguration configuration)
        {
            _configuration = configuration; // IConfiguration オブジェクトを受け取り、フィールドに設定
            _connection = new SqlConnection(_configuration.GetConnectionString(CommonConst.DBCONTEXT));
        }

        public async Task<IEnumerable<T>> Select<T>(string sql, object param = null)
        {
            // 接続が開かれていることを確認します。必要に応じて開くことができます。
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            // Dapper の QueryAsync を使用して、非同期的にデータを取得します。
            return await _connection.QueryAsync<T>(sql, param, _transaction);
        }

        public async Task<string> Insert<T>(string sql, object param = null)
        {
            return await _connection.ExecuteScalarAsync<string>(sql, param,_transaction);
        }

        public async Task<int> Update<T>(string sql, object param)
        {
            return await _connection.ExecuteAsync(sql, param, _transaction);
        }

        public async Task<int> Delete<T>(string sql, object param)
        {
            return await _connection.ExecuteAsync(sql, param, _transaction);
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = _connection.BeginTransaction();
            await Task.CompletedTask; // この行は実際には何もしませんが、非同期メソッドのシグネチャに合わせるために必要です。
        }

        public async Task CommitTransactionAsync()
        {
            _transaction?.Commit();
            await Task.CompletedTask;
        }

        public async Task RollbackTransactionAsync()
        {
            _transaction?.Rollback();
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<T>> ExecuteSP<T>(string procedureName, object param = null)
        {
            return await _connection.QueryAsync<T>(procedureName, param, commandType: CommandType.StoredProcedure, transaction: _transaction);
        }

        // IDisposableを実装してリソースを適切に解放する
        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
        }
    }
}