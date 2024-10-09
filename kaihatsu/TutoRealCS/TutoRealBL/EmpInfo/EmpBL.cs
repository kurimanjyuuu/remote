using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using TutoRealBE;
using TutoRealBE.Context;
using TutoRealBE.Result;
using TutoRealDA;
using TutoRealDA.Emp;
using static TutoRealCommon.CommonConst;
using static TutoRealCommon.CommonFunctionLibrary;

namespace TutoRealBL
{
    // BLプロジェクト内
    public class EmpBL : TutoRealBaseBL
    {
        private readonly EmpDA _da;

        public EmpBL(TutoRealDbContext context, IConfiguration configuration) : base(context, configuration)
        {
            // TutoRealDbContext から IDbConnection を取得するロジックが必要です。
            IDbConnection dbConnection = context.Database.GetDbConnection();

            // DAのインスタンスを作成し、IDbConnectionを渡します。
            _da = new EmpDA(dbConnection, configuration);

        }

        public async Task<IEnumerable<ParentContext>> InsertAsync(EmpInfoGetContext context)
        {
            // INSERT実行
            List<GeneralResult> pk = (List<GeneralResult>)await _da.InsertAsync(context);
            return pk;
        }

        public async Task<IEnumerable<ParentContext>> UpdateAsync(EmpInfoGetContext context)
        {
            // UPDATE実行
            List<GeneralResult> pk = (List<GeneralResult>)await _da.UpdateAsync(context);
            return pk;
        }

        public async Task<IEnumerable<ParentContext>> SelectAsync(EmpInfoGetContext context)
        {
            // SELECT実行
            IEnumerable<EmpInfoGetResult> result = await _da.SelectAsync(context);
            return result;
        }

        public async Task<IEnumerable<ParentContext>> DeleteAsync(EmpInfoGetContext context)
        {
            // DELETE実行
            IEnumerable<EmpInfoGetResult> result = await _da.DeleteAsync(context);
            return result;
        }
    }
}