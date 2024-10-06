using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using TutoRealBE;
using TutoRealBE.Context;
using TutoRealBE.Result;
using TutoRealBL;
using TutoRealDA;
using TutoRealDA.StandBy;

namespace TutoRealBL
{
    // BLプロジェクト内
    public class StandByBL : TutoRealBaseBL
    {
        private readonly StandByDA _da;

        public StandByBL(TutoRealDbContext context, IConfiguration configuration) : base(context, configuration)
        {
            // TutoRealDbContext から IDbConnection を取得するロジックが必要です。
            IDbConnection dbConnection = context.Database.GetDbConnection();

            // DAのインスタンスを作成し、IDbConnectionを渡します。
            _da = new StandByDA(dbConnection, configuration);

        }

        public async Task<IEnumerable<ParentContext>> SelectAsync(StandByConditionContext context)
        {
            // StandByDAを使用して認証データを取得
            IEnumerable<StandByListResult> result = await _da.SelectAsync(context);

            return result;
        }
    }
}