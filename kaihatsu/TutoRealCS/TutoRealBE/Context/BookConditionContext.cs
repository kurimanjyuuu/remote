using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using TutoRealBE.Entity;

namespace TutoRealBE.Context
{
    /// <summary>
    /// 戻り待機者検索条件コンテキスト
    /// </summary>
    public class BookConditionContext : ParentContext
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
        /// 書籍名称
        /// </summary>
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// カテゴリ
        /// </summary>
        public int SelectCategory { get; set; }

        /// <summary>
        /// キーワード
        /// </summary>
        public string? Keyword { get; set; } = string.Empty;

        /// <summary>
        /// 保管場所
        /// </summary>
        public string SelectDepository { get; set; } = string.Empty;

        /// <summary>
        /// 貸出可能のみCheck
        /// </summary>
        public bool OkFlg { get; set; } = false;
    }
}