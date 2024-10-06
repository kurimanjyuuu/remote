using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TutoRealBE.Context;
using TutoRealBE.Entity;
using TutoRealBE.Result;
using TutoRealBF;
using TutoRealCS.Models;
using static TutoRealCommon.CommonConst;
using CC = TutoRealCommon.CommonConst;
using CE = TutoRealBE.Entity.CommonEntity;
using static TutoRealCommon.CommonFunctionLibrary;
using System.Text.Json;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using System.Collections;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace TutoRealCS.Controllers
{
    public class BookController : BaseController
    {
        private readonly ITutoRealBaseBF _baseBF;

        public BookController(ITutoRealBaseBF baseBF) : base(baseBF)
        {
            _baseBF = baseBF;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string id)
        {
            //永続保持情報を取得
            MasterDataResult? master = (MasterDataResult)GetSession(SESSIONKEY.MASTERDATAS);

            if (master is null)
            {
                //Master情報が取れない場合は再ログイン
                return RedirectToAction("Login", "Login");
            }

            BookRegistViewModel retVM = new();

            //書籍idが連携された場合は情報を取得する
            if (string.IsNullOrWhiteSpace(id))
            {
                retVM = new BookRegistViewModel()
                {
                    title = "書籍登録",
                    CategoryList = master.CategoryMaster,
                    DepositoryList = SetDepositorys(master, ""),
                    RossFlg = false,
                };
            }
            else
            {
                BookGetContext context = new BookGetContext()
                {
                    Id = id,
                    ProcessKbn = CE.ProcessKbn.Select
                };

                //共通BFの呼び出し
                var result_bf = await _baseBF.Invoke(context);

                if (result_bf.Count() != 0)
                {
                    //DB取得結果
                    BookGetResult result = (BookGetResult)result_bf.FirstOrDefault();

                    retVM = new BookRegistViewModel()
                    {
                        title = "書籍詳細",
                        ID = id,
                        Name = result.Name,
                        RossFlg = result.RossFlg != "0",
                        Remarks = result.Remarks,
                        SelectCategory = result.Category,
                        CategoryList = SetCategorys(master.CategoryMaster, result.Category),
                        DepositoryList = SetDepositorys(master, result.Depository),
                        DelFlg = result.DelFlg != "0",
                        LoadFilePath = result.ImageFileName
                    };
                    //Ross(所在不明)の場合は設定しない
                    if (!retVM.RossFlg){
                        retVM.SelectDepository = SetDdlValue<DepositoryEntity>(master.DepositoryMaster.FirstOrDefault(row => row.Id == ToI(result.Depository)));
                    }
                }
            }

            return View(CC.VIEWNAME.BOOKREGIST, retVM);
        }
        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Search(BookListViewModel formData)
        {
            //永続保持情報を取得
            MasterDataResult master = (MasterDataResult)GetSession(SESSIONKEY.MASTERDATAS);

            BookConditionContext context = new BookConditionContext()
            {
                ProcessKbn = CE.ProcessKbn.Select,
                Name = formData.Name,
                SelectCategory = formData.SelectCategory,
                Keyword = formData.KeyWord,
                SelectDepository = GetDdlValue(formData.SelectDepository, "Id"),
                OkFlg = formData.OKFlg,
            };

            //共通BFの呼び出し(DBアクセス・Fileアクセス)
            //共通BFの呼び出し
            var result_bf = await _baseBF.Invoke(context);

            if (result_bf.Count() != 0)
            {
                //DB取得結果
                List<BookListResult> results = (List<BookListResult>)result_bf;

                foreach (var result in results)
                {
                    foreach (var masterBit in master.CategoryMaster)
                    {
                        if ((result.Category & masterBit.Bitvalue) != 0)
                        {
                            result.CategoryName = string.IsNullOrWhiteSpace(result.CategoryName) ? masterBit.Name : result.CategoryName + "<br >" + masterBit.Name;
                        };
                    }
                    foreach (var masterBit in master.DepositoryMaster)
                    {
                        if (result.Depository.Equals(masterBit.Id))
                        {
                            result.DepositoryName = masterBit.BaseName + "<br >" + masterBit.FloorName + "<br >" + masterBit.AreaName + "<br >" + masterBit.DetailName;
                        };
                    }
                    //所在不明、廃棄のいずれかのフラグが立っていれば貸出不可
                    if ("0".Equals(result.RossFlg) && "0".Equals(result.DelFlg))
                    {
                        //貸出可能なのは「所在不明ではない」「廃棄ではない」状態で
                        //「一度も貸し出しされていない(貸出日=NULL)」または「返却日が設定されている」
                        result.isLend = result.LendDate is null || result.ReturnDate is not null;
                    }
                    //廃棄のフラグが立っていれば返却不可
                    if ("0".Equals(result.DelFlg))
                    {
                        //返却可能なのは「廃棄ではない」状態で
                        //貸し出し日が設定されていて(貸出日=NULL以外)かつ返却日が設定されていない
                        result.isReturn = result.LendDate is not null && result.ReturnDate is null;
                    }
                }
                formData.DataList.AddRange(results);

            }

            //画面情報の復元
            formData.CategoryList = SetCategorys(master.CategoryMaster, formData.SelectCategory);
            formData.DepositoryList = SetDepositorys(master, ToS(formData.SelectDepository));


            return View(CC.VIEWNAME.BOOKLIST, formData);
        }

        [HttpPost]
        public async Task<IActionResult> Regist(IFormFile fileInput, BookRegistViewModel formData)
        {
            BookRegistContext context = new BookRegistContext()
            {
                Id = ToI(formData.ID),
                ProcessKbn = string.IsNullOrWhiteSpace(formData.ID) ? CE.ProcessKbn.Insert : CE.ProcessKbn.Update,
                Name = formData.Name,
                Category = formData.SelectCategory,
                Depository = formData.RossFlg ? string.Empty : GetDdlValue(formData.SelectDepository, "Id"),
                Remarks = formData.Remarks,
                DelFlg = formData.DelFlg ? "1" : "0",
                ImageFile = fileInput,
                LoadFilePath = formData.LoadFilePath,
                RossFlg = formData.RossFlg ? "1" : "0"
            };

            //共通BFの呼び出し(DBアクセス)
            await _baseBF.Invoke(context);

            //永続保持情報を取得
            MasterDataResult master = (MasterDataResult)GetSession(SESSIONKEY.MASTERDATAS);

            //画面情報の復元
            formData.CategoryList = SetCategorys(master.CategoryMaster, formData.SelectCategory);
            formData.DepositoryList = SetDepositorys(master, formData.SelectDepository);

            return View(CC.VIEWNAME.BOOKREGIST, formData);
        }

        [HttpPost]
        public async Task<IActionResult> Lend(string id , BookListViewModel formData)
        {
            AuthorityResult loginUser = (AuthorityResult)GetSession(SESSIONKEY.LOGINUSER);

            BookLendReturnContext context = new BookLendReturnContext()
            {
                Id = ToI(id),
                EmpId7 = loginUser.EmpId7,
                isLend = true,
                isReturn = false,
                ProcessKbn = CE.ProcessKbn.Update
            };

            //共通BFの呼び出し(DBアクセス)
            await _baseBF.Invoke(context);

            return View(CC.VIEWNAME.BOOKLIST, formData);
        }
        
        [HttpPost]
        public async Task<IActionResult> Return(string id , BookListViewModel formData)
        {
            AuthorityResult loginUser = (AuthorityResult)GetSession(SESSIONKEY.LOGINUSER);

            BookLendReturnContext context = new BookLendReturnContext()
            {
                Id = ToI(id),
                EmpId7 = loginUser.EmpId7,
                isLend = false,
                isReturn = true,
                ProcessKbn = CE.ProcessKbn.Update
            };

            //共通BFの呼び出し(DBアクセス)
            await _baseBF.Invoke(context);

            return View(CC.VIEWNAME.BOOKLIST, formData);
        }
        private static string DepositoryText(DepositoryEntity dm)
        {
            string format = "{0}{1}{2}{3}";
            return string.Format(format, dm.BaseName, dm.FloorName, dm.AreaName, dm.DetailName);
        }

        private static List<SelectListItem> SetDepositorys(MasterDataResult master, string select)
        {
            //プルダウンのListを生成する
            //拠点・フロア・エリア・詳細箇所
            List<SelectListItem> depositorys = new List<SelectListItem>();

            foreach (var item in master.DepositoryMaster)
            {
                depositorys.Add(new SelectListItem()
                {
                    Value = SetDdlValue<DepositoryEntity>(item),
                    Text = DepositoryText(item),
                    Selected = item.Id == ToI(select) ? true : false
                });
            }
            return depositorys;
        }

        private static List<CategoryItemsEntity> SetCategorys(List<CategoryItemsEntity> categorys, int select)
        {
            foreach (var entity in categorys)
            {
                if ((select & entity.Bitvalue) == entity.Bitvalue)
                {
                    entity.CheckStatus = true;
                }
            }
            return categorys;
        }
    }
}