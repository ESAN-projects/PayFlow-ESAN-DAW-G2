using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body, byte[]? attachment = null, string? attachmentName = null);
    }
}
