using System.Threading.Tasks;

namespace shopapp.webui.EmailServices
{
    public interface IEmailSender
    {
        //smpt => gmail, hotmail
        //api => sendgrid

        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}