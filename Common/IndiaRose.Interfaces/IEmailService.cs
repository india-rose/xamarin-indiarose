namespace IndiaRose.Interfaces
{
    public interface IEmailService
    {
		/// <summary>
		/// Propose to the user to send an email to contact the dev team.
		/// </summary>
		/// <returns>True if the app can send email, false otherwise.</returns>
	    bool SendContactEmail();
    }
}
