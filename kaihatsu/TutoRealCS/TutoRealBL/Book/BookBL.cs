using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using TutoRealBE;
using TutoRealBE.Context;
using TutoRealBE.Result;
using TutoRealDA;
using TutoRealDA.Book;
using static TutoRealCommon.CommonConst;
using static TutoRealCommon.CommonFunctionLibrary;

namespace TutoRealBL
{
    // BLプロジェクト内
    public class BookBL : TutoRealBaseBL
    {
        private readonly BookDA _da;

        public BookBL(TutoRealDbContext context, IConfiguration configuration) : base(context, configuration)
        {
            // TutoRealDbContext から IDbConnection を取得するロジックが必要です。
            IDbConnection dbConnection = context.Database.GetDbConnection();

            // DAのインスタンスを作成し、IDbConnectionを渡します。
            _da = new BookDA(dbConnection, configuration);

        }

        public async Task<IEnumerable<ParentContext>> SelectAsync(BookConditionContext context)
        {
            // StandByDAを使用して認証データを取得
            IEnumerable<BookListResult> result = await _da.SelectAsync(context);

            return result;
        }

        public async Task<IEnumerable<ParentContext>> GetAsync(BookGetContext context)
        {
            // StandByDAを使用して認証データを取得
            IEnumerable<BookGetResult> result = await _da.GetAsync(context);

            return result;
        }

        public async Task<IEnumerable<ParentContext>> InsertAsync(BookRegistContext context)
        {
            //INSERT実行
            List<GeneralResult> pk = (List<GeneralResult>)await _da.InsertAsync(context);
            if (pk != null && pk.Count > 0 && pk[0].PK != null)
            {
                await FileSaveAsync(context.ImageFile, pk[0].PK);
            }
            return pk;
        }
        public async Task<IEnumerable<ParentContext>> UpdateAsync(BookRegistContext context)
        {
            //INSERT実行
            List<GeneralResult> pk = (List<GeneralResult>)await _da.UpdateAsync(context);
            if (pk != null && pk.Count > 0 && pk[0].PK != null)
            {
                await FileSaveAsync(context.ImageFile, pk[0].PK);
            }
            return pk;
        }
        public async Task<IEnumerable<ParentContext>> LendReturnAsync(BookLendReturnContext context)
        {
            List<GeneralResult> pk = new List<GeneralResult>();
            if (context.isLend)
            {
                pk = (List<GeneralResult>)await _da.LendAsync(context);
            }
            if (context.isReturn)
            {
                pk = (List<GeneralResult>)await _da.ReturnAsync(context);
            }
            return pk;
        }

        public async Task<IEnumerable<ParentContext>> FileSaveAsync(IFormFile? fileUpload,string pk)
        {
            if (fileUpload != null && fileUpload.Length > 0)
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), string.Format(FOLDERPATH.FOLDER_FMT, FOLDERPATH.WWWROOT, FOLDERPATH.IMG, FOLDERPATH.BOOK));

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                var fileName = PadLeftZero(pk, 6) + Right(fileUpload.FileName, 4);
                var fullPath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await fileUpload.CopyToAsync(stream);
                }

                return new List<GeneralResult>();
            }

            return new List<GeneralResult>();
        }
    }
}