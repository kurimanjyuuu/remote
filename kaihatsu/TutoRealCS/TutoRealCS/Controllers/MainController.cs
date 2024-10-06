using Microsoft.AspNetCore.Mvc;
using System.Buffers.Text;
using System.Diagnostics;
using TutoRealBF;
using TutoRealCS.Models;
using CC = TutoRealCommon.CommonConst;

namespace TutoRealCS.Controllers
{
    public class MainController : BaseController
    {
        private readonly ITutoRealBaseBF _baseBF;

        public MainController(ITutoRealBaseBF baseBF) : base(baseBF)
        {
            _baseBF = baseBF;
        }

        public IActionResult Index()
        {
            return View(CC.VIEWNAME.MAIN);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
