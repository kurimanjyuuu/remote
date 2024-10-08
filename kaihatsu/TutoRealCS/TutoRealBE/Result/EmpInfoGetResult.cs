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
        public string EmpId7 { get; set; } = string.Empty;

        /// <summary>
        /// 部署コード
        /// </summary>
        public string DeptCode4 { get; set; } = string.Empty;

        /// <summary>
        /// 姓
        /// </summary>
        public string SeiKanji { get; set; } = string.Empty;

        /// <summary>
        /// 名
        /// </summary>
        public string MeiKanji { get; set; } = string.Empty;

        /// <summary>
        /// せい
        /// </summary>
        public string SeiKana { get; set; } = string.Empty;

        /// <summary>
        /// めい
        /// </summary>
        public string MeiKana { get; set; } = string.Empty;

        /// <summary>
        /// メールアドレス
        /// </summary>
        public string MailAddress { get; set; } = string.Empty;
    }
}
