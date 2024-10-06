namespace TutoRealBE.Entity
{
    public class CategoryItemsEntity
    {
        /// <summary>
        /// カテゴリID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// カテゴリ名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// ビット値
        /// </summary>
        public int Bitvalue { get; set; }

        /// <summary>
        /// チェック状態
        /// </summary>
        public bool CheckStatus { get; set; } = false;
    }
}
