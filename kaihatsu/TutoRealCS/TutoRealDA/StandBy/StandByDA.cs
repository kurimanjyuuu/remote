using System.Linq;
using System.Threading.Tasks;
using TutoRealDA;
using TutoRealBE.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Text;
using static TutoRealCommon.CommonConst;
using static TutoRealCommon.CommonFunctionLibrary;
using System.Data;
using TutoRealBE;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data.Common;
using TutoRealBE.Result;

namespace TutoRealDA.StandBy
{
    public class StandByDA : TutoRealBaseDA
    {
        private readonly IDbConnection _dbConnection;

        public StandByDA(IDbConnection connection, IConfiguration configuration) : base(connection, configuration)
        {
            _dbConnection = connection;

        }

        public async Task<IEnumerable<StandByListResult>> SelectAsync(StandByConditionContext context)
        {
            string query = MakeQueryString(context);
            var parameters = new
            {
                EmpId7 = $"%{context.EmpId7}%",
                Name = $"%{context.Name}%",
                StartDate = context.StartDate,
                EndDate = context.EndDate,
            };
            IEnumerable<StandByListResult>  result = await this.Select<StandByListResult>(query, parameters);
            return result;
        }

        private static string MakeQueryString(StandByConditionContext context)
        {
            StringBuilder query = new StringBuilder().AppendLine("SELECT sl.EmpId7 AS EmpId7");
            query.AppendLine("       , MAX(em.SeiKanji) AS SeiKanji");
            query.AppendLine("       , MAX(em.MeiKanji) AS MeiKanji");
            query.AppendLine("       , MAX(sl.StartDate) AS StartDate");
            query.AppendLine("       , MAX(sl.EndDate) AS EndDate");
            query.AppendLine("       , MAX(em.MailAddress) AS MailAddress");
            query.AppendLine($"FROM {TblSet(TBLNAME.T_STANDBYLIST)} sl");
            query.AppendLine($"LEFT JOIN {TblSet(TBLNAME.M_EMPMASTER)}  em");
            query.AppendLine($"ON sl.EmpId7 = em.EmpId7");
            query.AppendLine($"WHERE ((sl.StartDate >= @StartDate AND sl.EndDate <= @EndDate)");
            query.AppendLine($"    OR (sl.StartDate <= @EndDate AND sl.EndDate IS NULL))");
            if (!string.IsNullOrWhiteSpace(context.EmpId7))
            {
                query.AppendLine($"AND sl.EmpId7 like @EmpId7");
            }
            if (!string.IsNullOrWhiteSpace(context.Name))
            {
                query.AppendLine($"AND (em.SeiKanji like @Name");
                query.AppendLine($"OR   em.MeiKanji like @Name");
                query.AppendLine($"OR   em.SeiKana like @Name");
                query.AppendLine($"OR   em.MeiKana like @Name)");
            }
            query.AppendLine($"GROUP BY sl.EmpId7");

            return query.ToString();

        }
    }
}