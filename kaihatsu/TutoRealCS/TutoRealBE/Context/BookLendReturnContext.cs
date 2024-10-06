using System.Text.Json;
using Microsoft.AspNetCore.Http;
using TutoRealBE.Result;

namespace TutoRealBE.Context
{
    /// <summary>
    /// 書籍登録コンテキスト
    /// </summary>
    public class BookLendReturnContext : ParentContext
    {
        // シリアライズ処理を実装
        public override string Serialize()
        {
            return JsonSerializer.Serialize(this);
        }

        // デシリアライズ処理を実装
        public override void Deserialize(string serializedData)
        {
            AuthorityResult obj = JsonSerializer.Deserialize<AuthorityResult>(serializedData);
            if (obj != null)
            {
            }
        }
        /// <summary>
        /// 書籍登録ID（連番）
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ログインユーザー
        /// </summary>
        public string EmpId7 { get; set; } = string.Empty;

        /// <summary>
        /// 貸出のときTrue
        /// </summary>
        public bool isLend { get; set; }
        
        /// <summary>
        /// 貸出のときTrue
        /// </summary>
        public bool isReturn { get; set; }
    }
}