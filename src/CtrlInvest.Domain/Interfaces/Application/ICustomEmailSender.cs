using System.Threading.Tasks;

namespace CtrlInvest.Domain.Interfaces.Application
{
    public interface ICustomEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
