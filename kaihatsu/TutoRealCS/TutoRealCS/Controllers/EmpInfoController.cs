using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
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
                        return await HandleRegister(formData);
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
            Debug.WriteLine("Registerボタンがクリックされました。");
            Debug.WriteLine($"DeptCode4: {formData.deptCode4}, seiKanji: {formData.seiKanji}, meiKanji: {formData.meiKanji}, seiKana: {formData.seiKana}, meiKana: {formData.meiKana}, mailAddress: {formData.mailAddress}");

            var empId = formData.empId7;
            var existingRecords = await CheckExistingRecord(empId); // 既存レコードの確認メソッドを追加

            if (existingRecords)
            {
                // 既存レコードが見つかれば更新処理
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
            else
            {
                // 既存レコードが見つからなければ新規登録
                Debug.WriteLine("対象レコードが見つかりません。新規登録に切り替えます。");
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
        }

        private async Task<bool> CheckExistingRecord(string empId)
        {
            var selectContext = new EmpInfoGetContext()
            {
                ProcessKbn = CE.ProcessKbn.Select,
                empId7 = empId,
            };

            var existingRecords = await _baseBF.Invoke(selectContext);
            return existingRecords.Any(); // 存在するかどうかを返す
        }

        private async Task<IActionResult> HandleSearch(EmpInfoViewModel formData)
        {
            Debug.WriteLine("Searchボタンがクリックされました。");
            Debug.WriteLine($"DeptCode4: {formData.deptCode4}, seiKanji: {formData.seiKanji}, meiKanji: {formData.meiKanji}, seiKana: {formData.seiKana}, meiKana: {formData.meiKana}, mailAddress: {formData.mailAddress}, joinDate: {formData.joinDate?.ToString("yyyy/MM/dd") ?? "null"}");

            var context = new EmpInfoGetContext()
            {
                ProcessKbn = CE.ProcessKbn.Select,
                empId7 = formData.empId7,
                deptCode4 = formData.deptCode4,
                seiKanji = formData.seiKanji,
                meiKanji = formData.meiKanji,
                seiKana = formData.seiKana,
                meiKana = formData.meiKana,
                mailAddress = formData.mailAddress,
                joinDate = formData.joinDate
            };

            try
            {
                var result_bf = await _baseBF.Invoke(context);
                Debug.WriteLine($"取得したデータ数: {result_bf.Count()}");

                var results = result_bf.Cast<EmpInfoGetResult>().ToList();
                foreach (var result in results)
                {
                    Debug.WriteLine($"取得したデータ: empId7={result.empId7}, joinDate={result.joinDate?.ToString("yyyy/MM/dd") ?? "null"}");
                }

                formData.DataList = results;
                Debug.WriteLine($"取得したデータ数: {formData.DataList.Count()}");


                return Json(new { success = true, message = "検索成功", data = results });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"エラーが発生しました: {ex}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        private async Task<IActionResult> HandleDelete(EmpInfoViewModel formData)
        {
            Debug.WriteLine("Deleteボタンがクリックされました。");

            // 入力値のバリデーション
            if (string.IsNullOrWhiteSpace(formData.empId7) || formData.retireDate == null)
            {
                return Json(new { success = false, message = "社員番号と退職日は必須です。" });
            }

            // retireDateをDateTime?に変換
            DateTime? retireDate = null;
            if (formData.retireDate.HasValue)
            {
                retireDate = formData.retireDate.Value; // null でない場合、値を取得
            }

            var context = new EmpInfoGetContext()
            {
                ProcessKbn = CE.ProcessKbn.Delete,
                empId7 = formData.empId7,
                retireDate = retireDate 
            };

            await _baseBF.Invoke(context);
            return Json(new { success = true, message = "削除成功" });
        }
    }
}