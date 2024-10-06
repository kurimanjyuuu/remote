using Microsoft.AspNetCore.Mvc;
using TutoRealBE.Context;
using TutoRealBE.Result;
using TutoRealBF;
using TutoRealCommon;
using TutoRealCS.Models;
using static TutoRealBE.Entity.CommonEntity;
using static TutoRealCommon.CommonConst;
using CFL = TutoRealCommon.CommonFunctionLibrary;

namespace TutoRealCS.Controllers
{
    public class LoginController : BaseController
    {
        private readonly ITutoRealBaseBF _baseBF;

        public LoginController(ITutoRealBaseBF baseBF) : base(baseBF)
        {
            _baseBF = baseBF;
        }

        /// <summary>
        /// ログイン画面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginUserViewModel());
        }
        /// <summary>
        /// ログアウト処理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Logout()
        {
            return RedirectToAction("Login", "Login");
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel formData)
        {
            //ゲストログインの場合はメインに飛ぶ
            if (formData.LoginUser.GuestKbn)
            {
                return RedirectToAction("Index", "Main");
            }

            //ゲスト設定をリセット
            formData.LoginUser.GuestKbn = false;

            //Formの情報をcontextに格納
            LoginContext context = new LoginContext
            {
                EmpId7 = formData.LoginUser.EmpId7,
                ProcessKbn = ProcessKbn.Select
            };

            //共通BFの呼び出し
            var result_bf = await _baseBF.Invoke(context);
            //結果から対象コンテキストの取得

            ViewBag.HideCommon = true;
            if (result_bf.Count() == 0)
            {
                formData.LoginUser.Password = string.Empty;
                //入力したIDがDBにない
                formData.LoginUser.ErrorMsg = string.Format(CommonConst.ERRORMSG.RECORD_ZERO, "社員番号");
                return View(formData);
            }
            else
            {
                //DB取得結果
                AuthorityResult result = (AuthorityResult)result_bf.FirstOrDefault();
                string cryptographyString = CFL.ToHash(formData.LoginUser.Password);
                if (!cryptographyString.Equals(result.Password))
                {
                    //パスワード入力誤り
                    formData.LoginUser.Password = string.Empty;
                    formData.LoginUser.ErrorMsg = CommonConst.ERRORMSG.PASSWORD_ERR;
                    return View(formData);
                }
                SetSession(HttpContext.Session, "LoginUser", result);
                AuthorityResult loginUser = (AuthorityResult)GetSession(SESSIONKEY.LOGINUSER);
            }
            return RedirectToAction("Index", "Main");
        }
    }
}