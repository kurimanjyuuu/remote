using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using TutoRealBE;
using TutoRealBE.Context;
using TutoRealBE.Entity;
using TutoRealBE.Result;

namespace TutoRealCS.Models
{
    public class BookListViewModel
    {
        /// <summary>
        /// 選択した書籍ID
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 書籍名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// カテゴリ(選択)
        /// </summary>
        public int SelectCategory { get; set; }

        [JsonIgnore]
        /// <summary>
        /// カテゴリ(リスト)
        /// </summary>
        public List<CategoryItemsEntity> CategoryList { get; set; } = new List<CategoryItemsEntity>();

        /// <summary>
        /// キーワード
        /// </summary>
        public string KeyWord { get; set; } = string.Empty;

        /// <summary>   
        /// 保管場所（選択）
        /// </summary>
        public string SelectDepository { get; set; } = string.Empty;

        [JsonIgnore]
        /// <summary>
        /// 保管場所(リスト)
        /// </summary>
        public List<SelectListItem> DepositoryList { get; set; } = new List<SelectListItem>();

        /// <summary>
        /// 貸出可チェック
        /// </summary>
        public bool OKFlg { get; set; } = false;

        [JsonIgnore]
        public List<BookListResult> DataList = new List<BookListResult>();
    }
}