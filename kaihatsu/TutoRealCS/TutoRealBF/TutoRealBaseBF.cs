using System.Reflection.Metadata;
using TutoRealBE;
using TutoRealBE.Context;
using TutoRealIF;
using static TutoRealBE.Entity.CommonEntity;

namespace TutoRealBF
{
    public interface ITutoRealBaseBF
    {
        Task<IEnumerable<ParentContext>> Invoke<TContext>(TContext context) where TContext : ParentContext;
        Task SelectAsync(EmpInfoGetContext context);
    }

    public class TutoRealBaseBF : ITutoRealBaseBF
    {
        private readonly ITutoRealBaseIF _tutoRealIF;

        public TutoRealBaseBF(ITutoRealBaseIF tutoRealIf)
        {
            _tutoRealIF = tutoRealIf;
        }

        public Task<IEnumerable<ParentContext>> Invoke<TContext>(TContext context) where TContext : ParentContext
        {
            // contextの型に基づいて処理を分岐
            switch (context.ProcessKbn)
            {
                case ProcessKbn.Select:
                    return _tutoRealIF.Select(context);
                case ProcessKbn.Insert:
                    return _tutoRealIF.Insert(context);
                case ProcessKbn.Update:
                    return _tutoRealIF.Update(context);
                case ProcessKbn.Delete:
                    return _tutoRealIF.Delete(context);
                default:
                    throw new ArgumentException("未対応のコンテキストが指定されました。", nameof(context));
            }
        }

        public Task SelectAsync(EmpInfoGetContext context)
        {
            throw new NotImplementedException();
        }
    }
}