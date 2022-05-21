using CartService.Settings.Interfaces;

namespace CartService.Settings
{
    public class EmailSettings : IEmailSettings
    {
        public string Stmp { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool SSL { get; set; }
    }
}