namespace Assets.Scripts.Model.Dto.Select
{
    /// <summary>
    /// キャラクターの欄
    /// </summary>
    public class CharacterCell
    {
        public CharacterCell(
            string name,
            CharacterIndex characterIndex,
            float x)
        {
            this.Name = name;
            this.CharacterIndex = characterIndex;
            this.X = x;
        }

        public string Name { get; set; }
        public CharacterIndex CharacterIndex { get; set; }
        public float X { get; set; }
    }
}
