namespace TutoRealBE.Entity
{
    public class DepositoryEntity
    {
        /// <summary>
        /// 保管場所ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 拠点ID
        /// </summary>
        public int BaseId { get; set; }

        /// <summary>
        /// 拠点名
        /// </summary>
        public string BaseName { get; set; } = string.Empty;

        /// <summary>
        /// フロアID
        /// </summary>
        public int FloorId { get; set; }

        /// <summary>
        /// フロアNo
        /// </summary>
        public int FloorNo { get; set; }

        /// <summary>
        /// フロア名称
        /// </summary>
        public string FloorName { get; set; } = string.Empty;

        /// <summary>
        /// エリアID
        /// </summary>
        public int AreaId { get; set; }

        /// <summary>
        /// AreaNo
        /// </summary>
        public int AreaNo { get; set; }

        /// <summary>
        /// エリア名称
        /// </summary>
        public string AreaName { get; set; } = string.Empty;

        /// <summary>
        /// 詳細
        /// </summary>
        public int DetailId { get; set; }

        /// <summary>
        /// DetailNo
        /// </summary>
        public int DetailNo { get; set; }

        /// <summary>
        /// 詳細箇所名称
        /// </summary>
        public string DetailName { get; set; } = string.Empty;
    }
}
