using System.Net.Mail;
using System.Text.Json;

namespace TutoRealBE.Context
{
    /// <summary>
    /// 登録コンテキスト
    /// </summary>
    public class EmpInfoGetContext : ParentContext
    {
        // シリアライズ処理を実装
        public override string Serialize()
        {
            return JsonSerializer.Serialize(this);
        }

        // デシリアライズ処理を実装
        public override void Deserialize(string serializedData)
        {
            var obj = JsonSerializer.Deserialize<EmpInfoGetContext>(serializedData);
            if (obj != null)
            {
                EmpId7 = obj.EmpId7;
                DeptCode4 = obj.DeptCode4;
                Seikanji = obj.Seikanji;
                Meikanji = obj.Meikanji;
                Seikana = obj.Seikana;
                Meikana = obj.Meikana;
                MailAddress = obj.MailAddress;
            }
        }

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
