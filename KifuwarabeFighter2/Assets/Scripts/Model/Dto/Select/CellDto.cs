namespace Assets.Scripts.Model.Dto.Select
{
    public class CellDto
    {
        public CellDto(
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
