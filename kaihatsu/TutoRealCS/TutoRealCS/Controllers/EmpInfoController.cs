using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TutoRealBE.Context;
using TutoRealBE.Entity;
using TutoRealBE.Result;
using TutoRealBF;
using TutoRealCS.Models;
using static TutoRealCommon.CommonConst;
using CC = TutoRealCommon.CommonConst;
using CE = TutoRealBE.Entity.CommonEntity;

namespace TutoRealCS.Controllers

{
    public class EmpInfoController : BaseController
    {
        private readonly ITutoRealBaseBF _baseBF;
        public EmpInfoController(ITutoRealBaseBF baseBF) : base(baseBF)
        {
            _baseBF = baseBF;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View("EmpInfo"); // ビューを返す
        }
        [HttpPost]
        public async Task<IActionResult> Regist(EmpInfoViewModel formData)
        {
            EmpInfoGetContext context = new EmpInfoGetContext()
            {
                ProcessKbn = CE.ProcessKbn.Insert,
                EmpId7 = formData.EmpId7,
                DeptCode4 = formData.DeptCode4,
                Seikanji = formData.Seikanji,
                Meikanji = formData.Meikanji,
                Seikana = formData.Seikana,
                Meikana = formData.Meikana,
                MailAddress = formData.MailAddress,
            };
            //共通BFの呼び出し(DBアクセス)
            await _baseBF.Invoke(context);

            //永続保持情報を取得
            MasterDataResult master = (MasterDataResult)GetSession(SESSIONKEY.MASTERDATAS);

            // ViewModelの初期化
            EmpInfoViewModel empVM = new();

            return View("EmpInfo", formData); // ビューを返す
        }
    }
}