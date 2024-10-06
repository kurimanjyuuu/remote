using TutoRealBE.Context;

namespace TutoRealCS.Models
{
    public class LoginUserViewModel
    {
        #region ログイン情報

        /// <summary>
        /// ログインユーザー
        /// </summary>
        public LoginContext LoginUser { get; set; } = new LoginContext();

        #endregion
    }
}