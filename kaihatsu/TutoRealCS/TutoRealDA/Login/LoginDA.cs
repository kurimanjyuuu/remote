using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text;
using TutoRealBE.Context;
using TutoRealBE.Result;
using static TutoRealCommon.CommonConst;
using static TutoRealCommon.CommonFunctionLibrary;

namespace TutoRealDA.Login
{
    public class LoginDA : TutoRealBaseDA
    {
        private readonly IDbConnection _dbConnection;

        public LoginDA(IDbConnection connection, IConfiguration configuration) : base(connection, configuration)
        {
            _dbConnection = connection;

        }

        public async Task<IEnumerable<AuthorityResult>> SelectAsync(LoginContext context)
        {
            string query = MakeQueryString(context);
            var parameters = new { EmpId7 = context.EmpId7 };
            IEnumerable<AuthorityResult> result = await this.Select<AuthorityResult>(query, parameters);
            return result;
        }

        private static string MakeQueryString(LoginContext logincontext)
        {
            StringBuilder query = new StringBuilder().AppendLine("SELECT   lu.EmpId7");
            query.AppendLine("       , lu.Password");
            query.AppendLine("       , em.SeiKanji");
            query.AppendLine("       , em.MeiKanji");
            query.AppendLine("       , em.MailAddress");
            query.AppendLine($"FROM {TblSet(TBLNAME.T_LOGINUSER)} lu");
            query.AppendLine($"LEFT JOIN {TblSet(TBLNAME.M_EMPMASTER)} em");
            query.AppendLine("ON lu.EmpId7 = em.EmpId7");
            query.AppendLine($"WHERE lu.EmpId7 = @EmpId7");

            return query.ToString();

        }
    }
}