using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TutoRealBE.Entity;

namespace TutoRealBE.Result
{
    /// <summary>
    /// 画面で使用するマスターデータを保持する
    /// </summary>
    public class MasterDataResult : ParentContext
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
        /// カテゴリマスター
        /// </summary>
        public List<CategoryItemsEntity> CategoryMaster { get; set; } = new List<CategoryItemsEntity>();

        /// <summary>
        /// 拠点マスター
        /// </summary>
        public List<DepositoryEntity> DepositoryMaster { get; set; } = new List<DepositoryEntity>();
    }
}
