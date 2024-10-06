namespace TutoRealCS.Models
{
    public class BaseViewModel
    {
        /// <summary>
        /// 画面タイトル
        /// </summary>
        public string title { get; set; } = string.Empty;

        /// <summary>
        /// ログインユーザー名
        /// </summary>
        public string user { get; set; } = string.Empty;
    }
}
