using Microsoft.AspNetCore.Mvc;
using System.Buffers.Text;
using System.Diagnostics;
using TutoRealBF;
using TutoRealCS.Models;
using static TutoRealCommon.CommonConst;

namespace TutoRealCS.Controllers
{
    public class MasterManageController : BaseController
    {
        private readonly ITutoRealBaseBF _baseBF;

        public MasterManageController(ITutoRealBaseBF baseBF) : base(baseBF)
        {
            _baseBF = baseBF;
        }

        public IActionResult Index()
        {
            return View(VIEWNAME.MASTERMANAGE);
        }
    }
}
