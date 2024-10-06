using TutoRealCS.Models.Data;

namespace TutoRealCS.Models
{
    public class SampleViewModel
    {
        #region 検索条件

        /// <summary>
        /// 検索用：都道府県
        /// </summary>
        public string? SearchPrefecture { get; set; }

        /// <summary>
        /// 検索用：ラーメン名
        /// </summary>
        public string? SearchName { get; set; }

        #endregion

        #region 画面表示

        /// <summary>
        /// データ更新日
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// ラーメン情報
        /// </summary>
        public List<SampleData> RamenList { get; set; } = new List<SampleData>();

        #endregion
    }
}