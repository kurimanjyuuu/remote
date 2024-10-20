using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using TutoRealBE.Entity;

namespace TutoRealBE.Context
{
    /// <summary>
    /// 社員情報取得コンテキスト
    /// </summary>
    public class EmpInfoGetResult : ParentContext
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
        public DateTime? joinDate { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        /// <summary>
        /// 退職日
        /// </summary>
        public DateTime? retireDate { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);

        /// <summary>
        ///　更新日時
        /// </summary>
        public string updateDatetime { get; set; } = string.Empty;

    }
}
