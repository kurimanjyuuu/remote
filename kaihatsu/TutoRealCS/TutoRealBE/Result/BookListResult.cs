using Microsoft.AspNetCore.Http;
using System.Text.Json;
using TutoRealBE.Context;

namespace TutoRealBE.Result
{
    /// <summary>
    /// 結果コンテキスト
    /// </summary>
    public class BookListResult : ParentContext
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
        /// カテゴリ名称
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// 保管場所
        /// </summary>
        public int Depository { get; set; }

        /// <summary>
        /// 保管場所名称
        /// </summary>
        public string DepositoryName { get; set; } = string.Empty;

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
        public string DelFlg { get; set; } = "0";

        #region 一覧個別プロパティ
        /// <summary>
        /// 書籍画像ファイル名称
        /// </summary>
        public string? ImageFileName { get; set; }
        /// <summary>
        /// 直近貸出日(NULL or sub_idが最大の貸出日)
        /// </summary>
        public DateTime? LendDate { get; set; }

        /// <summary>
        /// 返却日(NULL or sub_idが最大の返却日)
        /// </summary>
        public DateTime? ReturnDate { get; set; }

        /// <summary>
        /// 貸出可能な場合true、不可の場合false
        /// </summary>
        public bool isLend { get; set; } = false;

        /// <summary>
        /// 返却可能な場合true、不可の場合false
        /// </summary>
        public bool isReturn { get; set; } = false;
        #endregion

    }
}