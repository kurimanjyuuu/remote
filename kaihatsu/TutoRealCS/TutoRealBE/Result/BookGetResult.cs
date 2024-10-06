using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using TutoRealBE.Entity;

namespace TutoRealBE.Context
{
    /// <summary>
    /// 書籍情報取得コンテキスト
    /// </summary>
    public class BookGetResult : ParentContext
    {
        // シリアライズ処理を実装
        public override string Serialize()
        {
            return JsonSerializer.Serialize(this);
        }

        // デシリアライズ処理を実装
        public override void Deserialize(string serializedData)
        {
            //AuthorityResult obj = JsonSerializer.Deserialize<AuthorityResult>(serializedData);
            //if (obj != null)
            //{
            //}
        }

        /// <summary>
        /// 書籍登録ID)
        /// </summary>
        public string? ID { get; set; }

        /// <summary>
        /// 書籍名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// カテゴリ(選択)
        /// </summary>
        public int Category { get; set; }

        /// <summary>
        /// 保管場所（選択）
        /// </summary>
        public string Depository { get; set; } = string.Empty;

        /// <summary>
        /// 所在不明フラグ
        /// </summary>
        public string RossFlg { get; set; } = "0";

        /// <summary>
        /// 備考
        /// </summary>
        public string Remarks { get; set; } = string.Empty;

        /// <summary>
        /// 画像ファイル情報
        /// </summary>
        public string ImageFileName { get; set; } = string.Empty;

        /// <summary>
        /// 廃棄フラグ
        /// </summary>
        public string DelFlg { get; set; } = "0";
    }
}
