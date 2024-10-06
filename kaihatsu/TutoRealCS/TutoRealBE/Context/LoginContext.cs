using System.Text.Json;

namespace TutoRealBE.Context
{
    /// <summary>
    /// ログイン入力情報コンテキスト
    /// </summary>
    public class LoginContext : ParentContext
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
        /// 社員番号7桁
        /// </summary>
        public string EmpId7 { get; set; } = string.Empty;

        /// <summary>
        /// Password(平文)
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// ゲストログイン判定用、ゲストの時「true」
        /// </summary>
        public Boolean GuestKbn { get; set; } = false;    }
}
