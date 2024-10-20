using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Data;
using System.Security.Claims;
using System.Text;
using TutoRealBE;
using TutoRealBE.Context;
using TutoRealBE.Result;
using TutoRealBF;
using static TutoRealCommon.CommonConst;
using CE = TutoRealBE.Entity.CommonEntity;

namespace TutoRealCS.Controllers
{
    public class BaseController : Controller
    {
        private readonly ITutoRealBaseBF _baseBF;
        private MasterDataResult master = new MasterDataResult();

        public BaseController(ITutoRealBaseBF baseBF)
        {
            _baseBF = baseBF;
        }
        public async override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            //基本はこの状態
            ChangeLayout(true, true, true, "戻る", "");

            switch (filterContext.Controller)
            {
                case LoginController:           //ログイン
                    ChangeLayout(false, false, false, "", "");
                    break;
                case BookController:            //書籍
                    ChangeLayout(true, true, true, "戻る", "登録");
                    break;
                case MasterManageController:    //マスターメンテ
                    ChangeLayout(true, true, true, "戻る", "更新");
                    break;
                case EmpInfoController:            
                    ChangeLayout(true, true, true, "戻る", "登録");
                    break;
            }

            //永続保持情報を取得
            MasterDataResult? master = (MasterDataResult)GetSession(SESSIONKEY.MASTERDATAS);

            if (master is null)
            {
                //Master情報が取れない場合は再取得
                BaseContext context = new BaseContext();
                var result_bf = await _baseBF.Invoke(context);

                if (result_bf is not null && result_bf.Count() != 0)
                {
                    //TODO: 起動時に落ちるときと落ちない時がある
                    //セッションに格納する
                    MasterDataResult result = (MasterDataResult)result_bf.FirstOrDefault();
                    SetSession(HttpContext.Session, SESSIONKEY.MASTERDATAS, result);
                }
            }
            //ログインユーザーの取得
            AuthorityResult loginUser = (AuthorityResult)GetSession(SESSIONKEY.LOGINUSER);
            ViewBag.LoginUser = loginUser is null ? "" : loginUser.EmpId7+" : "+ loginUser.SeiKanji + loginUser.MeiKanji;
        }

        private void ChangeLayout(bool head, bool menu, bool foot, string lb, string rb)
        {
            ViewBag.Head = head;
            ViewBag.Menu = menu;
            ViewBag.Foot = foot;
            if (foot)
            {
                ViewBag.LButton = lb;
                ViewBag.RButton = rb;
            }
        }

        /// <summary>
        /// セッション情報にデータを格納する
        /// </summary>
        /// <param name="session">セッション情報</param>
        /// <param name="key">セッションキー</param>
        /// <param name="obj">データ</param>
        public static void SetSession(ISession session, string key, ParentContext obj)
        {
            //TODO: 起動時に落ちるときと落ちない時がある
            string serializedData = obj.Serialize();
            session.SetString(key, serializedData);
        }

        /// <summary>
        /// セッション情報を取得する
        /// </summary>
        /// <param name="key">セッションキー</param>
        /// <returns>セッションから取得したデータ</returns>
        public ParentContext GetSession(string key)
        {
            ParentContext? datas = null;
            try
            {
                switch (key)
                {
                    case SESSIONKEY.MASTERDATAS:
                        datas = GetSessionData<MasterDataResult>(HttpContext.Session, key);
                        break;
                    case SESSIONKEY.LOGINUSER:
                        datas = GetSessionData<AuthorityResult>(HttpContext.Session, key);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return datas;
        }

        /// <summary>
        /// セッションからデータを取得する
        /// </summary>
        /// <typeparam name="T">ジェネリック型</typeparam>
        /// <param name="session">セッション情報</param>
        /// <param name="key">セッションキー</param>
        /// <returns></returns>
        private static T GetSessionData<T>(ISession session, string key) where T : ParentContext, new()
        {
            string serializedData = session.GetString(key);
            if (string.IsNullOrEmpty(serializedData))
            {
                return null;
            }

            T obj = new T();
            obj.Deserialize(serializedData);
            return obj;
        }
    }
}