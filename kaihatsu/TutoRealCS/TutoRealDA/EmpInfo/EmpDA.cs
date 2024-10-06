using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper;
using TutoRealBE.Context;
using TutoRealBE.Result;
using System.Text;


namespace TutoRealDA.Emp
{
    public class EmpDA : TutoRealBaseDA
    {
        private readonly IDbConnection _dbConnection;

        public EmpDA(IDbConnection connection, IConfiguration configuration) : base(connection, configuration)
        {
            _dbConnection = connection;
        }

        private static string TblSet(string tableName)
        {
            return tableName;
        }

        // Selectメソッド
        public async Task<IEnumerable<EmpInfoGetResult>> SelectAsync(EmpInfoGetContext context)
        {
            // バリデーションチェック: すべてのフィールドがnullまたはデフォルトであってはならない
            if (string.IsNullOrEmpty(context.EmpId7))
                throw new ArgumentException("EmpId7は必須です。");

            if (string.IsNullOrEmpty(context.DeptCode4))
                throw new ArgumentException("DeptCode4は必須です。");

            if (string.IsNullOrEmpty(context.Seikanji))
                throw new ArgumentException("Seikanjiは必須です。");

            if (string.IsNullOrEmpty(context.Meikanji))
                throw new ArgumentException("Meikanjiは必須です。");

            if (string.IsNullOrEmpty(context.Seikana))
                throw new ArgumentException("Seikanaは必須です。");

            if (string.IsNullOrEmpty(context.Meikana))
                throw new ArgumentException("Meikanaは必須です。");

            if (string.IsNullOrEmpty(context.MailAddress))
                throw new ArgumentException("MailAddressは必須です。");

            // コンテキストに基づいてクエリ文字列を生成
            string query = MakeSelectQuery(context);

            // パラメータを動的に保持するためのディクショナリを作成
            var parameters = new Dictionary<string, object>
            {
                { "EmpId7", context.EmpId7 },
                { "DeptCode4", context.DeptCode4 },
                { "Seikanji", $"%{context.Seikanji}%" },
                { "Meikanji", $"%{context.Meikanji}%" },
                { "Seikana", $"%{context.Seikana}%" },
                { "Meikana", $"%{context.Meikana}%" },
                { "MailAddress", $"%{context.MailAddress}%" }
            };

            // パラメータを使ってクエリを実行
            return await this.Select<EmpInfoGetResult>(query, parameters);
        }

        private string MakeSelectQuery(EmpInfoGetContext context)
        {
            var whereClause = new StringBuilder("1=1"); // 初期条件

            // 各プロパティが指定されている場合、クエリに条件を追加
            if (!string.IsNullOrEmpty(context.EmpId7)) // EmpId7が空でない場合
            {
                whereClause.Append(" AND EmpId7 = @EmpId7");
            }
            if (!string.IsNullOrEmpty(context.DeptCode4)) // DeptCode4が空でない場合
            {
                whereClause.Append(" AND DeptCode4 = @DeptCode4");
            }
            if (!string.IsNullOrEmpty(context.Seikanji)) // Seikanjiが空でない場合
            {
                whereClause.Append(" AND Seikanji LIKE @Seikanji");
            }
            if (!string.IsNullOrEmpty(context.Meikanji)) // Meikanjiが空でない場合
            {
                whereClause.Append(" AND Meikanji LIKE @Meikanji");
            }
            if (!string.IsNullOrEmpty(context.Seikana)) // Seikanaが空でない場合
            {
                whereClause.Append(" AND Seikana LIKE @Seikana");
            }
            if (!string.IsNullOrEmpty(context.Meikana)) // Meikanaが空でない場合
            {
                whereClause.Append(" AND Meikana LIKE @Meikana");
            }
            if (!string.IsNullOrEmpty(context.MailAddress)) // MailAddressが空でない場合
            {
                whereClause.Append(" AND MailAddress LIKE @MailAddress");
            }

            // クエリを構築
            var query = new StringBuilder($"SELECT * FROM {TblSet("M_EmpMaster")} WHERE {whereClause}");
            return query.ToString();
        }

        // Insertメソッド
        public async Task<IEnumerable<GeneralResult>> InsertAsync(EmpInfoGetContext context)
        {
            string query = $"INSERT INTO {TblSet("M_EmpMaster")} (EmpId7, DeptCode4, Seikanji, Meikanji, Seikana, Meikana, MailAddress) " +
                            "VALUES (@EmpId7, @DeptCode4, @Seikanji, @Meikanji, @Seikana, @Meikana, @MailAddress)";
            var parameters = new
            {
                EmpId7 = context.EmpId7,
                DeptCode4 = context.DeptCode4,
                Seikanji = context.Seikanji,
                Meikanji = context.Meikanji,
                Seikana = context.Seikana,
                Meikana = context.Meikana,
                MailAddress = context.MailAddress,
            };

            try
            {
                Console.WriteLine(query);
                return await ExecuteAsync(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Insertエラー: {ex.Message}");
                throw;
            }
        }

        // Updateメソッド
        public async Task<IEnumerable<GeneralResult>> UpdateAsync(EmpInfoGetContext context)
        {
            string query = $"UPDATE {TblSet("M_EmpMaster")} " +
                           $"SET DeptCode4 = @DeptCode4, Seikanji = @Seikanji, Meikanji = @Meikanji, Seikana = @Seikana, Meikana = @Meikana, MailAddress = @MailAddress " +
                           $"WHERE EmpId7 = @EmpId7";
            var parameters = new
            {
                EmpId7 = context.EmpId7,
                DeptCode4 = context.DeptCode4,
                Seikanji = context.Seikanji,
                Meikanji = context.Meikanji,
                Seikana = context.Seikana,
                Meikana = context.Meikana,
                MailAddress = context.MailAddress,
            };
            return await ExecuteAsync(query, parameters); // 実行
        }

        // Deleteメソッド
        public async Task<IEnumerable<GeneralResult>> DeleteAsync(EmpInfoGetContext context)
        {
            string query = $"DELETE FROM {TblSet("M_EmpMaster")} WHERE EmpId7 = @EmpId7";
            var parameters = new
            {
                EmpId7 = context.EmpId7,
            };
            return await ExecuteAsync(query, parameters); // 実行
        }

        // ExecuteAsyncメソッド
        private async Task<IEnumerable<GeneralResult>> ExecuteAsync(string query, object parameters)
        {
            var results = await _dbConnection.ExecuteAsync(query, parameters);
            return new List<GeneralResult>
            {
                new GeneralResult { Success = results > 0 }
            };
        }
    }
}
