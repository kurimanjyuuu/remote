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

namespace TutoRealDA.Book
{
    public class BookDA : TutoRealBaseDA
    {
        private readonly IDbConnection _dbConnection;

        public BookDA(IDbConnection connection, IConfiguration configuration) : base(connection, configuration)
        {
            _dbConnection = connection;

        }

        public async Task<IEnumerable<BookListResult>> SelectAsync(BookConditionContext context)
        {
            string query = MakeSelectQuery(context);
            var parameters = new
            {
                @Name = $"%{context.Name}%",
                @Keyword = $"%{context.Keyword}%",
                @Category = context.SelectCategory,
                @Depository = context.SelectDepository,
            };
            IEnumerable<BookListResult> result = await this.Select<BookListResult>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<BookGetResult>> GetAsync(BookGetContext context)
        {
            string query = MakeGetQuery(context);
            var parameters = new
            {
                @id = $"{context.Id}",
            };
            IEnumerable<BookGetResult> result = await this.Select<BookGetResult>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<ParentContext>> InsertAsync(BookRegistContext context)
        {
            string query = MakeInsertQuery(context);
            var parameters = new
            {
                @Name = context.Name,
                @Category = context.Category,
                @Depository = context.RossFlg == "1" ? "0" : context.Depository,
                @RossFlg = context.RossFlg,
                @Remarks = context.Remarks,
                @DelFlg = context.DelFlg
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

        public async Task<IEnumerable<ParentContext>> UpdateAsync(BookRegistContext context)
        {
            string query = MakeInsertQuery(context);
            var parameters = new
            {
                @tempId = context.Id,
                @Name = context.Name,
                @Category = context.Category,
                @Depository = context.RossFlg == "1" ? "0" : context.Depository,
                @RossFlg = context.RossFlg,
                @Remarks = context.Remarks,
                @DelFlg = context.DelFlg
            };
            string cnt = await this.Insert<string>(query, parameters);

            List<GeneralResult> ret = new List<GeneralResult>();
            GeneralResult r = new GeneralResult()
            {
                ErrorCode = ERRORCODE.SUCCESS,
                ErrorMsg = ERRORMSG.NONE,
                PK = ToS(context.Id),
            };
            ret.Add(r);
            return ret;
        }

        public async Task<IEnumerable<ParentContext>> LendAsync(BookLendReturnContext context)
        {
            string query = MakeLendQuery(context);
            var parameters = new
            {
                @Id = context.Id,
                @LendUser = context.EmpId7
            };

            string cnt = await this.Insert<string>(query, parameters);

            List<GeneralResult> ret = new List<GeneralResult>();
            GeneralResult r = new GeneralResult()
            {
                ErrorCode = ERRORCODE.SUCCESS,
                ErrorMsg = ERRORMSG.NONE,
                PK = ToS(context.Id),
            };
            ret.Add(r);
            return ret;
        }

        public async Task<IEnumerable<ParentContext>> ReturnAsync(BookLendReturnContext context)
        {
            string query = MakeReturnQuery(context);
            var parameters = new
            {
                @Id = context.Id,
            };
            int cnt = await this.Update<int>(query, parameters);

            List<GeneralResult> ret = new List<GeneralResult>();
            GeneralResult r = new GeneralResult()
            {
                ErrorCode = ERRORCODE.SUCCESS,
                ErrorMsg = ERRORMSG.NONE,
                PK = ToS(context.Id),
            };
            ret.Add(r);
            return ret;
        }
        private static string MakeSelectQuery(BookConditionContext context)
        {
            StringBuilder query = new StringBuilder();

            //sub_id が最大のレコードにrn:1になるようなtempTableを作成
            query.AppendLine("WITH LS_MAX AS (");
            query.AppendLine("  SELECT   LS.Id");
            query.AppendLine("          ,LS.Sub_id");
            query.AppendLine("          ,LS.LendDate");
            query.AppendLine("          ,LS.ReturnDate");
            query.AppendLine("          ,ROW_NUMBER() OVER (PARTITION BY LS.Id ORDER BY LS.Sub_id DESC) as RN");
            query.AppendLine($"  FROM {TblSet(TBLNAME.T_LENDSTATUS)} LS");
            query.AppendLine(")");

            //SELECT
            query.AppendLine("SELECT     BM.Id");
            query.AppendLine("          ,BM.Name");
            query.AppendLine("          ,BM.Category");
            query.AppendLine("          ,BM.ImageFileName");
            query.AppendLine("          ,BM.Depository");
            query.AppendLine("          ,BM.RossFlg");
            query.AppendLine("          ,BM.Remarks");
            query.AppendLine("          ,BM.DelFlg");
            query.AppendLine("          ,LS.LendDate");
            query.AppendLine("          ,LS.ReturnDate");
            query.AppendLine($"FROM {TblSet(TBLNAME.T_BOOKLIST)} BM");
            query.AppendLine("LEFT JOIN LS_MAX LS");
            query.AppendLine("ON BM.Id = LS.Id");
            query.AppendLine("AND LS.RN = 1");
            //検索条件
            //カテゴリ
            query.Append("WHERE (BM.Category ");
            if (context.SelectCategory == 0)
                query.AppendLine(" > @Category )");
            else
                query.AppendLine(" & @Category != 0 )");

            if (!string.IsNullOrWhiteSpace(context.SelectDepository))
            {
                query.AppendLine(" AND BM.Depository = @Depository");
            }

            //書籍名称
            if (!string.IsNullOrWhiteSpace(context.Name))
                query.AppendLine(" AND BM.Name like @Name");

            //キーワード
            if (!string.IsNullOrWhiteSpace(context.Keyword))
                query.AppendLine(" AND (BM.Name like @Keyword OR BM.Remarks like @Keyword)");

            //貸出可能のみ
            if (context.OkFlg)
                query.AppendLine(" AND (LS.LendDate IS NULL OR LS.ReturnDate IS NOT NULL) ");

            query.AppendLine(" ORDER BY BM.Id ASC");
            return query.ToString();
        }

        private static string MakeGetQuery(BookGetContext context)
        {
            StringBuilder query = new StringBuilder();

            //SELECT
            query.AppendLine("SELECT     Id");
            query.AppendLine("          ,Name");
            query.AppendLine("          ,Category");
            query.AppendLine("          ,ImageFileName");
            query.AppendLine("          ,Depository");
            query.AppendLine("          ,RossFlg");
            query.AppendLine("          ,Remarks");
            query.AppendLine("          ,DelFlg");
            query.AppendLine("          ,UpdateDatetime");
            query.AppendLine($"FROM {TblSet(TBLNAME.T_BOOKLIST)}");
            //取得条件
            query.AppendLine("WHERE Id = @id");

            return query.ToString();
        }

        private static string MakeInsertQuery(BookRegistContext context)
        {
            StringBuilder query = new StringBuilder();
            if (0.Equals(context.Id))
            {
                //登録用ID採番
                query.AppendLine("DECLARE @tempID int;");
                query.AppendLine($"SET @tempID = (SELECT COUNT(1)+1 FROM {TblSet(TBLNAME.T_BOOKLIST)});");
            }
            else
            {
                query.AppendLine($"DELETE FROM {TblSet(TBLNAME.T_BOOKLIST)} ");
                query.AppendLine($"WHERE Id = @tempID");
            }
            //登録クエリ
            query.AppendLine($"INSERT INTO {TblSet(TBLNAME.T_BOOKLIST)} VALUES ");
            query.AppendLine("(");
            query.AppendLine("@tempID");
            query.AppendLine(",@Name");
            query.AppendLine(",@Category");
            if ((context.ImageFile is null || string.IsNullOrWhiteSpace(context.ImageFile.FileName)) && string.IsNullOrWhiteSpace(context.LoadFilePath))
            {
                query.AppendLine(",NULL");
            }
            else
            {
                string fileName = context.ImageFile is null ? context.LoadFilePath : context.ImageFile.FileName;
                query.AppendLine($",format(@tempID,'000000') +'{Right(fileName, 4)}'");
            }
            query.AppendLine(",@Depository");
            query.AppendLine(",@RossFlg");
            query.AppendLine(",@Remarks");
            query.AppendLine(",@DelFlg");
            query.AppendLine(",GETDATE()");
            query.AppendLine(")");
            
            query.AppendLine("SELECT @tempID");

            return query.ToString();
        }

        private static string MakeLendQuery(BookLendReturnContext context)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine($"INSERT INTO {TblSet(TBLNAME.T_LENDSTATUS)} ( ");
            query.AppendLine("Id");
            query.AppendLine(",Sub_id");
            query.AppendLine(",LendDate");
            query.AppendLine(",ReturnDate");
            query.AppendLine(",LendUser");
            query.AppendLine(",UpdateDatetime )");

            query.AppendLine("SELECT ");
            query.AppendLine("@Id");
            query.AppendLine(",ISNULL(MAX(sub_id),0) + 1");
            query.AppendLine(",GETDATE()");
            query.AppendLine(",NULL");
            query.AppendLine(",@LendUser");
            query.AppendLine(",GETDATE()");
            query.AppendLine($"FROM {TblSet(TBLNAME.T_LENDSTATUS)}");
            query.AppendLine("WHERE Id = @Id");
            return query.ToString();
        }

        private static string MakeReturnQuery(BookLendReturnContext context)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine($"UPDATE {TblSet(TBLNAME.T_LENDSTATUS)} ");
            query.AppendLine("SET ReturnDate = GETDATE()");
            query.AppendLine("WHERE Id = @Id ");
            query.AppendLine($"AND Sub_id = (SELECT MAX(Sub_id) FROM {TblSet(TBLNAME.T_LENDSTATUS)} WHERE Id = @Id)");

            return query.ToString();
        }
    }
}