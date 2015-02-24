namespace IndiaRose.Interfaces
{
    public interface IEmailService
    {
        void send(string title, string address, string body);
    }
}
