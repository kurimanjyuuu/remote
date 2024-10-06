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
        public string EmpId7 { get; set; } = string.Empty; // char

        /// <summary>
        /// 部署コード
        /// </summary>  
        public string DeptCode4 { get; set; } = string.Empty; // nchar

        /// <summary>
        /// 姓
        /// </summary>
        public string Seikanji { get; set; } = string.Empty;

        /// <summary>
        /// 名
        /// </summary>
        public string Meikanji { get; set; } = string.Empty;

        /// <summary>
        /// せい
        /// </summary>
        public string Seikana { get; set; } = string.Empty;

        /// <summary>
        /// めい
        /// </summary>
        public string Meikana { get; set; } = string.Empty;

        /// <summary>
        /// メールアドレス
        /// </summary>
        public string MailAddress { get; set; } = string.Empty;
    }
}
