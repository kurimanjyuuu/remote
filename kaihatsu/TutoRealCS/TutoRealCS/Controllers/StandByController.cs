using TutoRealCS.Models;
using Microsoft.AspNetCore.Mvc;
using CFL = TutoRealCommon.CommonFunctionLibrary;
using CC = TutoRealCommon.CommonConst;
using CE = TutoRealBE.Entity.CommonEntity;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Data;
using TutoRealBE.Context;
using TutoRealBE.Result;
using TutoRealBF;

namespace TutoRealCS.Controllers
{
    public class StandByController : BaseController
    {
        private readonly ITutoRealBaseBF _baseBF;

        public StandByController(ITutoRealBaseBF baseBF) : base(baseBF)
        {
            _baseBF = baseBF;
        }

        [HttpGet]
        public IActionResult Index()
        {
            StandByListViewModel retVM = new StandByListViewModel();
            return View(CC.VIEWNAME.STANDBYLIST, retVM);
        }
        [HttpPost]
        public async Task<IActionResult> Search(StandByListViewModel formData)
        {
            formData.DataList.Clear();
            StandByConditionContext context = new StandByConditionContext()
            {
                EmpId7 = formData.Condition.EmpId7,
                Name = formData.Condition.Name,
                StartDate = formData.Condition.StartDate,
                EndDate = formData.Condition.EndDate,
                ProcessKbn = CE.ProcessKbn.Select
            };

            //共通BFの呼び出し
            var result_bf = await _baseBF.Invoke(context);

            if (result_bf.Count() == 0)
            {
                //入力したIDがDBにない
                return View(formData);
            }
            else
            {
                //DB取得結果
                List<StandByListResult> result = (List<StandByListResult>)result_bf;
                formData.DataList = new List<StandByListResult>();
                foreach (var row in result)
                {
                    StandByListResult rowdata = new StandByListResult()
                    {
                        EmpId7 = row.EmpId7,
                        SeiKanji = row.SeiKanji,
                        MeiKanji = row.MeiKanji,
                        StartDate = row.StartDate,
                        EndDate = row.EndDate,
                        MailAddress = row.MailAddress,
                    };
                    formData.DataList.Add(rowdata);
                }

                return View(CC.VIEWNAME.STANDBYLIST, formData);
            }

        }
    }
}
