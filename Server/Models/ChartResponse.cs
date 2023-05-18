using Application.Models;

namespace Server.Models
{
    public class ChartResponse
    {
        public string Position { get; set; }
        public int PlayerNumber { get; set; }
        public string PlayerName { get; set; }

        public static ChartResponse FromData(ChartDto chart)
        {
            return new ChartResponse
            {
                Position = chart.PositionId.ToUpper(),
                PlayerNumber = chart.PlayerNumber,
                PlayerName = chart.Player != null ? chart.Player.Name : "",
            };
        }
    }
}
