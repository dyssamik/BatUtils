namespace BatUtils.Models
{
    public class Client
    {
        public bool Enabled { get; set; } = true;
        public string Code { get; set; }
        public string Name { get; set; }
        public string RKServerAddress { get; set; }
        public ushort RKServerPort { get; set; }
        public string SHServerAddress { get; set; }
        public ushort SHServerPort { get; set; }
    }
}