using System.Text.Json;

namespace TutoRealBE.Result
{
    /// <summary>
    /// 戻り待機者一覧結果コンテキスト
    /// </summary>
    public class StandByListResult : ParentContext
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
        /// 社員番号
        /// </summary>
        public string EmpId7 { get; set; } = string.Empty;

        /// <summary>
        /// 姓漢字
        /// </summary>
        public string SeiKanji { get; set; } = string.Empty;

        /// <summary>
        /// 名漢字
        /// </summary>
        public string MeiKanji { get; set; } = string.Empty;

        /// <summary>
        /// 姓カナ
        /// </summary>
        public string? SeiKana { get; set; }

        /// <summary>
        /// 名カナ
        /// </summary>
        public string? MeiKana { get; set; }

        /// <summary>
        /// 待機開始日
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 待機開始日(yyyy/mm/dd)
        /// </summary>
        public string StartDateYMD
        {
            get { return StartDate?.ToString("yyyy/MM/dd") ?? string.Empty; }
        }

        /// <summary>
        /// 待機終了日
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 待機終了日(yyyy/mm/dd)
        /// </summary>
        public string EndDateYMD
        {
            get { return EndDate?.ToString("yyyy/MM/dd") ?? string.Empty; }
        }

        /// <summary>
        /// メールアドレス
        /// </summary>
        public string MailAddress { get; set; } = string.Empty;
    }
}