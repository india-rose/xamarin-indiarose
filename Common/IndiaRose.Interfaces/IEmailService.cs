namespace IndiaRose.Interfaces
{
    public interface IEmailService
    {
        void Send(string title, string address, string body);
    }
}
