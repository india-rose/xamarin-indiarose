using System;
using IndiaRose.Interfaces;
using Lotz.Xam.Messaging;
using Storm.Mvvm.Services;

namespace IndiaRose.Services
{
    public class EmailService : AbstractService, IEmailService
    {
	    private const string CONTACT_UID = "Contact";
	    private const string ADDRESS_PROPERTY = "EmailAddress";
	    private const string TITLE_PROPERTY = "Title";
	    private const string BODY_PROPERTY = "Body";

	    public bool SendContactEmail()
	    {
		    try
		    {
			    var emailTask = MessagingPlugin.EmailMessenger;
			    if (emailTask.CanSendEmail)
			    {
				    string address = LocalizationService.GetString(CONTACT_UID, ADDRESS_PROPERTY);
				    string title = LocalizationService.GetString(CONTACT_UID, TITLE_PROPERTY);
				    string body = LocalizationService.GetString(CONTACT_UID, BODY_PROPERTY);

				    var message = new EmailMessageBuilder().To(address).Subject(title).Body(body).Build();
				    emailTask.SendEmail(message);
				    return true;
			    }
		    }
			catch (Exception e)
			{
				LoggerService.Log(string.Format("IndiaRose.Services.EmailService.SendContactEmail() : Exception during email sending process => {0}", e.Message), MessageSeverity.Critical);
			}
		    return false;
	    }
    }
}
