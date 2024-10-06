using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Collections.Generic;
using TutoRealBE;
using TutoRealBE.Context;
using TutoRealBE.Result;
using TutoRealDA.Emp;
using static TutoRealCommon.CommonConst;
using static TutoRealCommon.CommonFunctionLibrary;
using TutoRealDA;

namespace TutoRealBL
{
    public class EmpBL : TutoRealBaseBL
    {
        private readonly EmpDA _da;

        public EmpBL(TutoRealDbContext context, IConfiguration configuration) : base(context, configuration)
        {
            IDbConnection dbConnection = context.Database.GetDbConnection();
            _da = new EmpDA(dbConnection, configuration);
        }

        public async Task<IEnumerable<ParentContext>> SelectAsync(EmpInfoGetContext context)
        {
            IEnumerable<EmpInfoGetResult> result = await _da.SelectAsync(context);

            // EmpInfoGetResultをParentContextにキャスト
            return result.Cast<ParentContext>(); // もしくは
        }

        public async Task<IEnumerable<GeneralResult>> InsertAsync(EmpInfoGetContext context)
        {
            List<GeneralResult> pk = (List<GeneralResult>)await _da.InsertAsync(context);
            if (pk != null && pk.Count > 0 && pk[0].PK != null)
            {
                
            }
            return pk;
        }

        public async Task<IEnumerable<GeneralResult>> UpdateAsync(EmpInfoGetContext context)
        {
            List<GeneralResult> pk = (List<GeneralResult>)await _da.UpdateAsync(context);
            if (pk != null && pk.Count > 0 && pk[0].PK != null)
            {
              
            }
            return pk;
        }

        public async Task<IEnumerable<GeneralResult>> DeleteAsync(EmpInfoGetContext context)
        {
            return await _da.DeleteAsync(context);
        }

        public async Task<IEnumerable<GeneralResult>> FileSaveAsync(IFormFile? fileUpload, string pk)
        {
            if (fileUpload != null && fileUpload.Length > 0)
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(),
                    string.Format(FOLDERPATH.FOLDER_FMT, FOLDERPATH.WWWROOT, FOLDERPATH.IMG, FOLDERPATH.BOOK));

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

        internal async Task<IEnumerable<EmpInfoGetResult>> GetAsync(EmpInfoGetContext context)
        {
            return await _da.SelectAsync(context);
        }
    }
}
