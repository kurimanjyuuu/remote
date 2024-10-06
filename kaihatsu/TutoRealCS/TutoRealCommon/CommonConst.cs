namespace TutoRealCommon;

public class CommonConst
{
    #region "定数"
    /// <summary>
    /// DBContext
    /// </summary>
    public const string DBCONTEXT = "TutoRealDbContext";

    /// <summary>
    /// DBContext
    /// </summary>
    public const string DBNAME = "TutoRealDB";

    /// <summary>
    /// yyyy/MM/dd
    /// </summary>
    public const string FMT_YMD = "{0:yyyy/MM/dd}";

    /// <summary>
    /// テーブル指定フォーマット
    /// </summary>
    public const string FMT_TBL = "{0}.dbo.{1}";

    /// <summary>
    /// フォルダパス
    /// </summary>
    public struct FOLDERPATH
    {
        public const string FOLDER_FMT = "{0}\\{1}\\{2}";
        public const string WWWROOT= "wwwroot";
        public const string IMG = "img";
        public const string BOOK = "Book";
        public const string LOG = "log";
    }

    /// <summary>
    /// スキルチェック時のレベル判定
    /// </summary>
    public struct SKILLLEVEL
    {
        public const string BASIC = "Basic";
        public const string STANDARD = "Standard";
        public const string ADVANCED = "Advanced";
    }
    public struct SESSIONKEY
    {
        /// <summary>
        /// 拠点マスター
        /// </summary>
        public const string MASTERDATAS = "MasterDatas";

        /// <summary>
        /// ログインユーザー
        /// </summary>
        public const string LOGINUSER = "LoginUser";
    }

    /// <summary>
    /// DBテーブル一覧
    /// </summary>
    public struct TBLNAME
    {
        /// <summary>
        /// ログインユーザー一覧テーブル
        /// </summary>
        public const string T_LOGINUSER = "T_LoginUser";

        /// <summary>
        /// 戻り待機者一覧テーブル
        /// </summary>
        public const string T_STANDBYLIST = "T_StandByList";

        /// <summary>
        /// 書籍貸出状況一覧テーブル
        /// </summary>
        public const string T_LENDSTATUS = "T_LendStatus";

        /// <summary>
        /// 書籍一覧テーブル
        /// </summary>
        public const string T_BOOKLIST = "T_BookList";

        /// <summary>
        /// 部署マスター
        /// 
        /// </summary>
        public const string M_DEPTMASTER = "M_DeptMaster";

        /// <summary>
        /// 社員マスター
        /// </summary>
        public const string M_EMPMASTER = "M_EmpMaster";

        /// <summary>
        /// 拠点マスター
        /// </summary>
        public const string M_BASEMASTER = "M_BaseMaster";

        /// <summary>
        /// フロアマスター
        /// </summary>
        public const string M_FLOORMASTER = "M_FloorMaster";

        /// <summary>
        /// エリアマスター
        /// </summary>
        public const string M_AREAMASTER = "M_AreaMaster";

        /// <summary>
        /// 詳細保管箇所マスター
        /// </summary>
        public const string M_DETAILMASTER = "M_DetailMaster";

        /// <summary>
        /// 保管場所マスター
        /// </summary>
        public const string M_DEPOSITORYMASTER = "M_DepositoryMaster";

        /// <summary>
        /// カテゴリマスター
        /// </summary>
        public const string M_CATEGORYMASTER = "M_CategoryMaster";
    }

    /// <summary>
    /// View一覧
    /// </summary>
    public struct VIEWNAME
    {
        /// <summary>
        /// ログイン
        /// </summary>
        public const string LOGIN = "Login";

        /// <summary>
        /// メインページ
        /// </summary>
        public const string MAIN = "Main";

        /// <summary>
        /// 戻り待機者一覧
        /// </summary>
        public const string STANDBYLIST = "StandByList";

        /// <summary>
        /// 書籍登録・詳細
        /// </summary>
        public const string BOOKREGIST = "BookRegist";

        /// <summary>
        /// 書籍検索
        /// </summary>
        public const string BOOKLIST = "BookList";

        /// <summary>
        /// マスター管理
        /// </summary>
        public const string MASTERMANAGE = "MasterM" +
            "anage";
    }

    public struct ERRORMSG
    {
        public const string NONE = "";
        public const string RECORD_ZERO = "対象の{0}は登録されていません。";
        public const string PASSWORD_ERR = "パスワードが正しくありません。";
    }

    public struct FLG
    {
        public const string ON = "1";
        public const string OFF = "0";
    }

    public struct ERRORCODE
    {
        public const string SUCCESS = "0";
        public const string QUERY_TIMEOUT = "1";
    }
    #endregion
}
