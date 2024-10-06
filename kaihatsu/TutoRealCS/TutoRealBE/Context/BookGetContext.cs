using System.Text.Json;

namespace TutoRealBE.Context
{
    /// <summary>
    /// 書籍情報取得コンテキスト
    /// </summary>
    public class BookGetContext : ParentContext
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
        /// 書籍登録ID
        /// </summary>
        public string Id { get; set; } = string.Empty;

      
    }

}