namespace Assets.Scripts.Model.Dto.Select
{
    /// <summary>
    /// プレイヤーの状況
    /// </summary>
    public class PlayerStatus
    {
        public PlayerStatus(float locationY)
        {
            this.LocationY = locationY;
        }

        public float LocationY { get; set; }
    }
}
