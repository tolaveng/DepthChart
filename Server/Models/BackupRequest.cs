namespace Server.Models
{
    public class BackupRequest
    {
        public string Position { get; set; }
        public PlayerRequest Player { get; set; }
        public int? Depth { get; set; }
    }
}
