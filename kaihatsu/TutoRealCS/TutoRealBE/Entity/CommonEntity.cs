namespace TutoRealBE.Entity
{
    /// <summary>
    /// 共通エンティティクラス
    /// </summary>
    public static class CommonEntity
    {
        /// <summary>
        /// 処理区分
        /// </summary>
        public enum ProcessKbn
        {
            /// <summary>
            /// DBアクセス(取得)
            /// </summary>
            Select, 
            /// <summary>
            /// DBアクセス(更新)
            /// </summary>
            Update,
            /// <summary>
            /// DBアクセス(新規登録)
            /// </summary>
            Insert,
            /// <summary>
            /// DBアクセス(削除)
            /// </summary>
            Delete,
        }
    }
}
