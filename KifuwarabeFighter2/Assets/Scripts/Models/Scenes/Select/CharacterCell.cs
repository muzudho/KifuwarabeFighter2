namespace Assets.Scripts.Models.Scene.Select
{
    /// <summary>
    /// キャラクターの欄
    /// </summary>
    public class CharacterCell
    {
        public CharacterCell(
            string name,
            CharacterKey characterIndex,
            float x)
        {
            this.Name = name;
            this.CharacterIndex = characterIndex;
            this.X = x;
        }

        public string Name { get; set; }
        public CharacterKey CharacterIndex { get; set; }
        public float X { get; set; }
    }
}
