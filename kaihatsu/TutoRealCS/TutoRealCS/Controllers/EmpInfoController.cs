using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
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
            return View("EmpInfo");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Regist([FromBody] EmpInfoViewModel formData)
        {
            Debug.WriteLine($"受け取ったデータ: empId7: {formData.empId7}, ActionType: {formData.ActionType}");

            try
            {
                if (formData == null)
                {
                    Debug.WriteLine("フォームデータがnullです。");
                    return Json(new { success = false, message = "データが不正です" });
                }

                Debug.WriteLine($"ActionType: {formData.ActionType}");

                switch (formData.ActionType)
                {
                    case "Register":
                        return await HandleRegister(formData);
                    case "Update":
                        return await HandleUpdate(formData);
                    case "Search":
                        return await HandleSearch(formData);
                    case "Delete":
                        return await HandleDelete(formData);
                    default:
                        Debug.WriteLine("不正なリクエスト: " + formData.ActionType);
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
            var empId = formData.empId7;

            // 社員番号のバリデーション
            if (!Regex.IsMatch(empId, @"^[0-9]{7}$"))
            {
                return Json(new { success = false, message = "社員番号は7桁で入力してください。" });
            }

            // 社員番号が既に存在するかチェック
            var existingRecords = await CheckExistingRecord(empId);
            if (existingRecords)
            {
                return Json(new { success = false, message = "既に登録されています。" });
            }

            // 登録処理
            var context = new EmpInfoGetContext()
            {
                ProcessKbn = CE.ProcessKbn.Insert,
                empId7 = empId,
                deptCode4 = formData.deptCode4,
                seiKanji = formData.seiKanji,
                meiKanji = formData.meiKanji,
                seiKana = formData.seiKana,
                meiKana = formData.meiKana,
                mailAddress = formData.mailAddress,
                joinDate = formData.joinDate,
            };

            try
            {
                await _baseBF.Invoke(context);
                return Json(new { success = true, message = "登録成功" });
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine($"SQLエラー: {sqlEx.Message}");
                return Json(new { success = false, message = "データベースエラー: " + sqlEx.Message });
            }
        }

        private async Task<IActionResult> HandleUpdate(EmpInfoViewModel formData)
        {
            var empId = formData.empId7;

            // 社員番号のバリデーション
            if (!Regex.IsMatch(empId, @"^[0-9]{7}$"))
            {
                return Json(new { success = false, message = "社員番号は7桁で入力してください。" });
            }

            // 現在のデータを取得
            var currentData = await GetCurrentData(empId);
            if (currentData == null)
            {
                return Json(new { success = false, message = "指定された社員番号に該当する情報が見つかりませんでした。" });
            }

            // 検索で取得したupdateDatetimeをチェック
            var searchedUpdateDatetime = formData.updateDatetime;

            // 既存のupdateDatetimeが変更されていないか確認（楽観ロック）
            if (currentData.updateDatetime != searchedUpdateDatetime)
            {
                return Json(new { success = false, message = "更新失敗: 他のユーザーによって変更されています。" });
            }

            // 更新処理
            var context = new EmpInfoGetContext()
            {
                ProcessKbn = CE.ProcessKbn.Update,
                empId7 = empId,
                deptCode4 = formData.deptCode4,
                seiKanji = formData.seiKanji,
                meiKanji = formData.meiKanji,
                seiKana = formData.seiKana,
                meiKana = formData.meiKana,
                mailAddress = formData.mailAddress,
                retireDate = formData.retireDate,
            };

            try
            {
                await _baseBF.Invoke(context);
                return Json(new { success = true, message = "更新成功" });
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine($"SQLエラー: {sqlEx.Message}");
                return Json(new { success = false, message = "データベースエラー: " + sqlEx.Message });
            }
        }

        private async Task<bool> CheckExistingRecord(string empId)
        {
            var selectContext = new EmpInfoGetContext()
            {
                ProcessKbn = CE.ProcessKbn.Select,
                empId7 = empId,
            };

            var existingRecords = await _baseBF.Invoke(selectContext);
            return existingRecords.Any();
        }

        private async Task<EmpInfoGetResult> GetCurrentData(string empId)
        {
            var context = new EmpInfoGetContext()
            {
                ProcessKbn = CE.ProcessKbn.Select,
                empId7 = empId,
            };

            var existingRecords = await _baseBF.Invoke(context);
            return existingRecords.FirstOrDefault() as EmpInfoGetResult;
        }

        private async Task<IActionResult> HandleSearch(EmpInfoViewModel formData)
        {
            var empId = formData.empId7;

            var context = new EmpInfoGetContext
            {
                ProcessKbn = CE.ProcessKbn.Select,
                empId7 = empId
            };

            var results = await _baseBF.Invoke(context);
            var searchResults = results.Cast<EmpInfoGetResult>().ToList();

            if (searchResults.Count > 0)
            {
                var search = searchResults.First();
                return Json(new
                {
                    success = true,
                    message = "検索成功",
                    data = new
                    {
                        empId7 = search.empId7,
                        deptCode4 = search.deptCode4,
                        seiKanji = search.seiKanji,
                        meiKanji = search.meiKanji,
                        seiKana = search.seiKana,
                        meiKana = search.meiKana,
                        mailAddress = search.mailAddress,
                        joinDate = search.joinDate?.ToString("yyyy-MM-dd"),
                        retireDate = search.retireDate?.ToString("yyyy-MM-dd"),
                        updateDatetime = search.updateDatetime // ここで取得したupdateDatetimeを返す
                    }
                });
            }
            else
            {
                return Json(new { success = false, message = "指定された社員番号に該当する情報は見つかりませんでした。" });
            }
        }

        private async Task<IActionResult> HandleDelete(EmpInfoViewModel formData)
        {
            Debug.WriteLine("Deleteボタンがクリックされました。");

            if (formData == null)
            {
                Debug.WriteLine("formDataはnullです。");
                return Json(new { success = false, message = "データが正しく受信されていません。" });
            }

            string empId = formData.empId7;

            var context = new EmpInfoGetContext
            {
                ProcessKbn = CE.ProcessKbn.Delete,
                empId7 = empId
            };

            Debug.WriteLine($"一括削除処理を実行: empId={empId}");

            await _baseBF.Invoke(context);
            return Json(new { success = true, message = "削除しました。" });
        }
    }

    public static class DateTimeExtensions
    {
        public static DateTime TruncateToSeconds(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }
    }
}
