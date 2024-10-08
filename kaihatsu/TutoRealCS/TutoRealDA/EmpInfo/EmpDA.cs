using Microsoft.Extensions.Configuration;
using System.Data;
using System.Diagnostics;
using System.Security.Policy;
using System.Text;
using TutoRealBE;
using TutoRealBE.Context;
using TutoRealBE.Result;
using static TutoRealCommon.CommonConst;
using static TutoRealCommon.CommonFunctionLibrary;

namespace TutoRealDA.Emp
{
    public class EmpDA : TutoRealBaseDA
    {
        private readonly IDbConnection _dbConnection;

        public EmpDA(IDbConnection connection, IConfiguration configuration) : base(connection, configuration)
        {
            _dbConnection = connection;
        }

        private static string MakeInsertQuery(EmpInfoGetContext context)
        {
            StringBuilder query = new StringBuilder();

            // 登録クエリ
            query.AppendLine($"INSERT INTO {TblSet(TBLNAME.M_EMPMASTER)} (EmpId7, Seikanji, Meikanji, Seikana, Meikana, DeptCode4, MailAddress) VALUES ");
            query.AppendLine("(");
            query.AppendLine("@EmpId7");
            query.AppendLine(",@Seikanji");
            query.AppendLine(",@Meikanji");
            query.AppendLine(",@Seikana");
            query.AppendLine(",@Meikana");
            query.AppendLine(",@DeptCode4");
            query.AppendLine(",@MailAddress");
            query.AppendLine(")");

            return query.ToString();
        }

        public async Task<IEnumerable<ParentContext>> InsertAsync(EmpInfoGetContext context)
        {
            string query = MakeInsertQuery(context);
            var parameters = new
            {
                @EmpId7 = context.EmpId7,
                DeptCode4 = context.DeptCode4,
                @Seikanji = context.Seikanji,
                @Meikanji = context.Meikanji,
                @Seikana = context.Seikana,
                @Meikana = context.Meikana,
                @MailAddress = context.MailAddress,
            
            };
            string key = await this.Insert<string>(query, parameters);

            List<GeneralResult> ret = new List<GeneralResult>();
            GeneralResult r = new GeneralResult()
            {
                ErrorCode = ERRORCODE.SUCCESS,
                ErrorMsg = ERRORMSG.NONE,
                PK = key,
            };
            ret.Add(r);
            return ret;
        }
    }
}