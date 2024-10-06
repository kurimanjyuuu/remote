using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using TutoRealBE;
using TutoRealBE.Result;
using TutoRealDA;
using TutoRealDA.Master;

namespace TutoRealBL.Master
{
    // BLプロジェクト内
    public class MasterBL : TutoRealBaseBL
    {
        private readonly MasterDA _da;

        public MasterBL(TutoRealDbContext context, IConfiguration configuration) : base(context, configuration)
        {
            IDbConnection dbConnection = context.Database.GetDbConnection();

            // DAのインスタンスを作成し、IDbConnectionを渡します。
            _da = new MasterDA(dbConnection, configuration);

        }

        public async Task<IEnumerable<ParentContext>> SelectAsync(ParentContext parentContext)
        {
            // DAを使用して認証データを取得
            IEnumerable<MasterDataResult> result = await _da.SelectAsync(parentContext);

            // ここでauthorityDataを使用して認証ロジックを実装
            // 例: パスワードの検証など

            return result;
        }
    }
}
