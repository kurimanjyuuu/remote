using Microsoft.AspNetCore.Mvc.Rendering;
using TutoRealBE;
using TutoRealBE.Context;
using TutoRealBE.Entity;
using TutoRealBE.Result;

namespace TutoRealCS.Models
{
    public class BookRegistViewModel : BaseViewModel
    {
        /// <summary>
        /// ID(新規はブランク、更新は書籍登録ID)
        /// </summary>
        public string? ID { get; set; }

        /// <summary>
        /// 書籍名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// カテゴリ(選択)
        /// </summary>
        public int SelectCategory { get; set; }

        /// <summary>
        /// カテゴリ(リスト)
        /// </summary>
        public List<CategoryItemsEntity> CategoryList { get; set; } = new List<CategoryItemsEntity>();

        /// <summary>
        /// 保管場所（選択）
        /// </summary>
        public string SelectDepository { get; set; } = string.Empty;

        /// <summary>
        /// 保管場所(リスト)
        /// </summary>
        public List<SelectListItem> DepositoryList { get; set; } = new List<SelectListItem>();

        /// <summary>
        /// 所在不明フラグ
        /// </summary>
        public bool RossFlg { get; set; } = false;

        /// <summary>
        /// 備考
        /// </summary>
        public string Remarks { get; set; } = string.Empty;

        /// <summary>
        /// 画像ファイル情報
        /// </summary>
        public IFormFile? ImageFile { get; set; }

        /// <summary>
        /// 詳細表示用
        /// </summary>
        public string LoadFilePath { get; set; } = string.Empty;

        /// <summary>
        /// 廃棄フラグ
        /// </summary>
        public bool DelFlg { get; set; } = false;
    }
}