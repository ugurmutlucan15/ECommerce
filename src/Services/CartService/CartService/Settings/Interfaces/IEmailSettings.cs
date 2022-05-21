namespace CartService.Settings.Interfaces
{
    public interface IEmailSettings
    {
        string Stmp { get; set; }

        int Port { get; set; }

        string UserName { get; set; }

        string Password { get; set; }

        bool SSL { get; set; }
    }
}