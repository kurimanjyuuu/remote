using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutoRealDA
{
    public interface IDBAccess
    {
        // データの読み取り（SELECTクエリ）
        Task<IEnumerable<T>> Select<T>(string sql, object param = null);

        // データの挿入（INSERTクエリ）
        Task<string> Insert<T>(string sql, object param = null);

        // データの更新（UPDATEクエリ）
        Task<int> Update<T>(string sql, object param = null);

        // データの削除（DELETEクエリ）
        Task<int> Delete<T>(string sql, object param = null);

        // トランザクションの開始
        Task BeginTransactionAsync();

        // トランザクションのコミット
        Task CommitTransactionAsync();

        // トランザクションのロールバック
        Task RollbackTransactionAsync();

        // ストアドプロシージャの実行
        Task<IEnumerable<T>> ExecuteSP<T>(string procedureName, object param = null);
    }
}
