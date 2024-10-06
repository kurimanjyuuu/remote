using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutoRealBE;

namespace TutoRealBL
{
    public interface ITutoRealBaseBL
    {
        ParentContext Select(ParentContext context);
        ParentContext Update(ParentContext context);
        ParentContext Insert(ParentContext context);
        ParentContext Delete(ParentContext context);
    }
}
