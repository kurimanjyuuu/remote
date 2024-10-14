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
            query.AppendLine($"INSERT INTO {TblSet(TBLNAME.M_EMPMASTER)} (EmpId7, SeiKanji, MeiKanji, SeiKana, MeiKana, DeptCode4, MailAddress, JoinDate) VALUES ");
            query.AppendLine("(");
            query.AppendLine("@EmpId7");
            query.AppendLine(",@SeiKanji");
            query.AppendLine(",@MeiKanji");
            query.AppendLine(",@SeiKana");
            query.AppendLine(",@MeiKana");
            query.AppendLine(",@DeptCode4");
            query.AppendLine(",@MailAddress");
            query.AppendLine(",@JoinDate");
            query.AppendLine(")");

            return query.ToString();
        }

        public async Task<IEnumerable<ParentContext>> InsertAsync(EmpInfoGetContext context)
        {
            string query = MakeInsertQuery(context);
            var parameters = new
            {
                @empId7 = context.empId7,
                deptCode4 = context.deptCode4,
                @seiKanji = context.seiKanji,
                @meiKanji = context.meiKanji,
                @seiKana = context.seiKana,
                @meiKana = context.meiKana,
                @mailAddress = context.mailAddress,
                @joinDate = context.joinDate,

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

        private static string MakeUpdateQuery(EmpInfoGetContext context)
        {
            StringBuilder query = new StringBuilder();

            // 更新クエリ
            query.AppendLine($"UPDATE {TblSet(TBLNAME.M_EMPMASTER)} SET ");
            query.AppendLine("SeiKanji = @SeiKanji,");
            query.AppendLine("MeiKanji = @MeiKanji,");
            query.AppendLine("SeiKana = @SeiKana,");
            query.AppendLine("MeiKana = @MeiKana,");
            query.AppendLine("DeptCode4 = @DeptCode4,");
            query.AppendLine("MailAddress = @MailAddress ");
            query.AppendLine("WHERE EmpId7 = @EmpId7;"); // EmpId7を条件として指定

            return query.ToString();
        }

        public async Task<IEnumerable<ParentContext>> UpdateAsync(EmpInfoGetContext context)
        {
            string query = MakeUpdateQuery(context);
            var parameters = new
            {
                @empId7 = context.  empId7,
                deptCode4 = context.deptCode4,
                @seiKanji = context.seiKanji,
                @meiKanji = context.meiKanji,
                @seiKana = context.seiKana,
                @meiKana = context.meiKana,
                @mailAddress = context.mailAddress,

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

        private static string MakeSelectQuery(EmpInfoGetContext context)
        {
            StringBuilder query = new StringBuilder();

            // SELECT *
            query.AppendLine("SELECT *");
            query.AppendLine($"FROM {TblSet(TBLNAME.M_EMPMASTER)}");

            // 取得条件
            query.AppendLine("WHERE EmpId7 = @EmpId7");

            return query.ToString();
        }

        public async Task<IEnumerable<EmpInfoGetResult>> SelectAsync(EmpInfoGetContext context)
        {
            string query = MakeSelectQuery(context);
            var parameters = new
            {
                @empId7 = context.empId7,
                deptCode4 = context.deptCode4,
                @seiKanji = context.seiKanji,
                @meiKanji = context.meiKanji,
                @seiKana = context.seiKana,
                @meiKana = context.meiKana,
                @mailAddress = context.mailAddress,
                @joinDate = context.joinDate,
            };
            IEnumerable<EmpInfoGetResult> result = await this.Select<EmpInfoGetResult>(query, parameters);
            return result;
        }

        private static string MakeDeleteQuery(EmpInfoGetContext context)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine($"DELETE FROM {TblSet(TBLNAME.M_EMPMASTER)}");
            query.AppendLine("WHERE EmpId7 = @EmpId7");

            return query.ToString();
        }

        public async Task<IEnumerable<EmpInfoGetResult>> DeleteAsync(EmpInfoGetContext context)
        {
            string query = MakeDeleteQuery(context);
            var parameters = new
            {
                @EmpId7 = context.empId7,
            };
            IEnumerable<EmpInfoGetResult> result = await this.Select<EmpInfoGetResult>(query, parameters);
            return result;
        }
    }
}