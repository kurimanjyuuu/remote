using System.Text.Json;

namespace TutoRealBE.Context
{
    /// <summary>
    /// 戻り待機者検索条件コンテキスト
    /// </summary>
    public class StandByConditionContext : ParentContext
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
        public string? EmpId7 { get; set; }

        /// <summary>
        /// 姓名カナ
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 待機開始日
        /// </summary>
        public DateTime? StartDate { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); // 当月1日;

        /// <summary>
        /// 待機終了日
        /// </summary>
        public DateTime? EndDate { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1); // 翌月1日から1日引くと当月末日
    }
}