using System.Text.Json;
using Microsoft.AspNetCore.Http;
using TutoRealBE.Result;

namespace TutoRealBE.Context
{
    /// <summary>
    /// 書籍登録コンテキスト
    /// </summary>
    public class BookRegistContext : ParentContext
    {
        // シリアライズ処理を実装
        public override string Serialize()
        {
            return JsonSerializer.Serialize(this);
        }

        // デシリアライズ処理を実装
        public override void Deserialize(string serializedData)
        {
            AuthorityResult obj = JsonSerializer.Deserialize<AuthorityResult>(serializedData);
            if (obj != null)
            {
            }
        }
        /// <summary>
        /// 書籍登録ID（連番）
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 書籍名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// カテゴリ
        /// </summary>
        public int Category { get; set; }

        /// <summary>
        /// 保管場所
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
        /// 削除(破棄)フラグ
        /// </summary>
        public string DelFlg { get; set; } =  "0";

        /// <summary>
        /// 画像ファイル情報
        /// </summary>
        public IFormFile? ImageFile { get; set; }
        /// <summary>
        /// 詳細表示用
        /// </summary>
        public string LoadFilePath { get; set; } = string.Empty;
    }
}