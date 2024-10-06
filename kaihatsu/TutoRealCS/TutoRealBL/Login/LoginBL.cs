using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using TutoRealBE;
using TutoRealBE.Context;
using TutoRealBE.Result;
using TutoRealDA;
using TutoRealDA.Login;

namespace TutoRealBL
{
    // BLプロジェクト内
    public class LoginBL : TutoRealBaseBL
    {
        private readonly LoginDA _da;

        public LoginBL(TutoRealDbContext context, IConfiguration configuration) : base(context, configuration)
        {
            // TutoRealDbContext から IDbConnection を取得するロジックが必要です。
            IDbConnection dbConnection = context.Database.GetDbConnection();

            // LoginDAのインスタンスを作成し、IDbConnectionを渡します。
            _da = new LoginDA(dbConnection, configuration);

        }

        public async Task<IEnumerable<ParentContext>> SelectAsync(LoginContext loginContext)
        {
            // LoginDAを使用して認証データを取得
            IEnumerable< AuthorityResult> authorityData = await _da.SelectAsync(loginContext);

            // ここでauthorityDataを使用して認証ロジックを実装
            // 例: パスワードの検証など

            return authorityData;
        }
    }
}