using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using TutoRealBE;
using TutoRealBE.Context;
using TutoRealBE.Entity;
using TutoRealBE.Result;

namespace TutoRealCS.Models
{
    public class EmpInfoViewModel : BaseViewModel
    {
        /// <summary>
        /// 社員番号
        /// </summary>  
        public string empId7 { get; set; } = string.Empty; // char

        /// <summary>
        /// 部署コード
        /// </summary>  
        public string deptCode4 { get; set; } = string.Empty; // nchar

        /// <summary>
        /// 姓
        /// </summary>
        public string seiKanji { get; set; } = string.Empty;

        /// <summary>
        /// 名
        /// </summary>
        public string meiKanji { get; set; } = string.Empty;

        /// <summary>
        /// せい
        /// </summary>
        public string seiKana { get; set; } = string.Empty;

        /// <summary>
        /// めい
        /// </summary>
        public string meiKana { get; set; } = string.Empty;

        /// <summary>
        /// メールアドレス
        /// </summary>
        public string mailAddress { get; set; } = string.Empty;

        /// <summary>
        /// 入社日
        /// </summary>
        public DateTime? joinDate { get; set; } = new DateTime (DateTime.Now.Year, DateTime.Now.Month, 1);

        /// <summary>
        /// 退職日
        /// </summary>
        public DateTime? retireDate { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1); 


        /// <summary>
        ///　更新日時
        /// </summary>
        public string updateDatetime { get; set; } = string.Empty;

        /// <summary>
        /// アクションタイプ
        /// </summary>
        public string ActionType { get; set; } = string.Empty;
    
    }
}
