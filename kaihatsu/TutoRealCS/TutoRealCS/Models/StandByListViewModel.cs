using TutoRealBE;
using TutoRealBE.Context;
using TutoRealBE.Result;

namespace TutoRealCS.Models
{
    public class StandByListViewModel
    {
        #region 検索条件

        /// <summary>
        /// 検索条件リスト
        /// </summary>
        public StandByConditionContext Condition { get; set; } = new StandByConditionContext();

        #endregion
        #region 画面表示
        /// <summary>
        /// 検索結果
        /// </summary>
        public List<StandByListResult> DataList { get; set; } = new List<StandByListResult>();

        #endregion
    }
}