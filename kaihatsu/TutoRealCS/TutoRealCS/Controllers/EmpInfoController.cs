using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Regist([FromBody] EmpInfoViewModel formData)
        {
            try
            {
                if (formData == null)
                {
                    return Json(new { success = false, message = "データが不正です" });
                }

                switch (formData.ActionType)
                {
                    case "Register":
                        return await HandleRegister(formData);
                    case "Search":
                        return await HandleSearch(formData);
                    case "Delete":
                        return await HandleDelete(formData);
                    default:
                        return Json(new { success = false, message = "不正なリクエスト" });
                }
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine($"SQLエラー: {sqlEx.Message}");
                return Json(new { success = false, message = "データベースエラー: " + sqlEx.Message });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"エラー: {ex.Message}");
                return Json(new { success = false, message = "内部サーバーエラー: " + ex.Message });
            }
        }

        private async Task<IActionResult> HandleRegister(EmpInfoViewModel formData)
        {
            Debug.WriteLine("Registerボタンがクリックされました。");

            var context = new EmpInfoGetContext()
            {
                ProcessKbn = string.IsNullOrWhiteSpace(formData.EmpId7) ? CE.ProcessKbn.Insert : CE.ProcessKbn.Update,
                EmpId7 = formData.EmpId7,
                DeptCode4 = formData.DeptCode4,
                Seikanji = formData.Seikanji,
                Meikanji = formData.Meikanji,
                Seikana = formData.Seikana,
                Meikana = formData.Meikana,
                MailAddress = formData.MailAddress,
            };

            await _baseBF.Invoke(context);
            return Json(new { success = true, message = "登録成功" });
        }

        private async Task<IActionResult> HandleSearch(EmpInfoViewModel formData)
        {
            Debug.WriteLine("Searchボタンがクリックされました。");
            Debug.WriteLine($"DeptCode4: {formData.DeptCode4}, Seikanji: {formData.Seikanji}, Meikanji: {formData.Meikanji}, Seikana: {formData.Seikana}, Meikana: {formData.Meikana}, MailAddress: {formData.MailAddress}");

            var context = new EmpInfoGetContext()
            {
                ProcessKbn = CE.ProcessKbn.Select,
                EmpId7 = formData.EmpId7,
                DeptCode4 = formData.DeptCode4,
                Seikanji = formData.Seikanji,
                Meikanji = formData.Meikanji,
                Seikana = formData.Seikana,
                Meikana = formData.Meikana,
                MailAddress = formData.MailAddress,
            };

            var result_bf = await _baseBF.Invoke(context);
            Debug.WriteLine($"取得したデータ数: {result_bf.Count()}"); 
            var results = result_bf.Cast<EmpInfoGetResult>().ToList();
            formData.DataList = results;

            return View("EmpInfo", formData); 
        }

        private async Task<IActionResult> HandleDelete(EmpInfoViewModel formData)
        {
            Debug.WriteLine("Deleteボタンがクリックされました。");

            var context = new EmpInfoGetContext()
            {
                ProcessKbn = CE.ProcessKbn.Delete,
                EmpId7 = formData.EmpId7
            };

            await _baseBF.Invoke(context);
            return Json(new { success = true, message = "削除成功" });
        }
    }
}
