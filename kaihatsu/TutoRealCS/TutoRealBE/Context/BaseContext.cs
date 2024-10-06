using System.Text.Json;
using TutoRealBE.Entity;
using TutoRealBE.Result;

namespace TutoRealBE.Context
{
    /// <summary>
    /// Parentコンテキストがabstractのため、Baseに実態を持たせる
    /// </summary>
    public class BaseContext : ParentContext
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
    }
}