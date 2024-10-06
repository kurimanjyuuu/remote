using System.Reflection;
using System.Text.Json;

namespace TutoRealBE.Result
{
    /// <summary>
    /// ログイン結果格納コンテキスト
    /// </summary>
    public class AuthorityResult : ParentContext
    {
        // シリアライズ処理を実装
        public override string Serialize()
        {
            return JsonSerializer.Serialize(this);
        }

        // デシリアライズ処理を実装
        public override void Deserialize(string serializedData)
        {
            // JSON文字列からDictionaryへデシリアライズする
            var deserializedData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(serializedData);
            if (deserializedData == null) return;

            // オブジェクトのプロパティを反復処理する
            foreach (PropertyInfo propertyInfo in this.GetType().GetProperties())
            {
                // プロパティにセット可能かチェック
                if (!propertyInfo.CanWrite) continue;

                // デシリアライズしたデータの中に、現在のプロパティ名と一致するキーがあるかチェック
                if (deserializedData.TryGetValue(propertyInfo.Name, out JsonElement value))
                {
                    // JSON要素をプロパティの型に変換し、プロパティに設定する
                    object typedValue = JsonSerializer.Deserialize(value.GetRawText(), propertyInfo.PropertyType);
                    propertyInfo.SetValue(this, typedValue);
                }
            }
        }
        /// <summary>
        /// 社員番号7桁
        /// </summary>
        public string EmpId7 { get; set; } = string.Empty;

        /// <summary>
        /// 姓漢字
        /// </summary>
        public string SeiKanji { get; set; } = string.Empty;

        /// <summary>
        /// 名漢字
        /// </summary>
        public string MeiKanji { get; set; } = string.Empty;

        /// <summary>
        /// 姓カナ
        /// </summary>
        public string SeiKana { get; set; } = string.Empty;

        /// <summary>
        /// 名カナ
        /// </summary>
        public string MeiKana { get; set; } = string.Empty;

        /// <summary>
        /// パスワード
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// メルアド
        /// </summary>
        public string MailAddress { get; set; } = string.Empty;

        /// <summary>
        /// 所属コード
        /// </summary>
        public string DeptCode { get; set; } = string.Empty;
    }
}
