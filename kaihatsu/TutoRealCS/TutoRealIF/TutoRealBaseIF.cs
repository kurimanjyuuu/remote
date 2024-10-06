using TutoRealBE;
using Microsoft.AspNetCore.Http;
namespace TutoRealIF
{
    public interface ITutoRealBaseIF
    {
        Task<IEnumerable<ParentContext>> Select(ParentContext context);
        Task<IEnumerable<ParentContext>> Insert(ParentContext context);
        Task<IEnumerable<ParentContext>> Update(ParentContext context);
        Task<IEnumerable<ParentContext>> Delete(ParentContext context);
    }
    // TutoRealBaseBFはTutoRealIFインターフェースを実装します。
    public abstract class TutoRealBaseIF : ITutoRealBaseIF
    {
        public abstract Task<IEnumerable<ParentContext>> Select(ParentContext context);
        public abstract Task<IEnumerable<ParentContext>> Insert(ParentContext context);
        public abstract Task<IEnumerable<ParentContext>> Update(ParentContext context);
        public abstract Task<IEnumerable<ParentContext>>  Delete(ParentContext context);

    }
}