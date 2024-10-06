using Microsoft.AspNetCore.Mvc;
using NLog;
using TutoRealBF;

namespace TutoRealCS.Controllers
{
    public class SkillCheckController : BaseController
    {
        private readonly ITutoRealBaseBF _baseBF;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public SkillCheckController(ITutoRealBaseBF baseBF) : base(baseBF)
        {
            _baseBF = baseBF;
        }

        [HttpGet]
        [HttpPost]
        public IActionResult Basic()
        {
            #region 
            /*******
             * Ｑ１*
             *******/
            logger.Info("==========Q1　Start==========");
            OutputData("Q1の回答を記載");
            logger.Info("==========Q1　End==========");

            /*******
             * Ｑ２*
             *******/
            logger.Info("==========Q2　Start==========");
            OutputData("Q2の回答を記載");
            logger.Info("==========Q2　End==========");

            /*******
             * Ｑ３*
             *******/
            logger.Info("==========Q3　Start==========");
            OutputData("Q3の回答を記載");
            logger.Info("==========Q3　End==========");
            #endregion

            return RedirectToAction("Logout", "Login");
        }

        private void OutputData(string data)
        {
            string logFormat = "wwwroot\\log\\log_{0}.log";
            string filename = string.Format(logFormat, DateTime.Today.ToString("yyyyMMdd"));
            using (StreamWriter writer = new StreamWriter(filename))
            {
                   writer.WriteLine(data);  // 各行のデータをファイルに書き込む
            }
        }
    }
}