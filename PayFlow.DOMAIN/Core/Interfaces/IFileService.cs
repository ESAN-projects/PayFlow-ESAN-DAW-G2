using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface IFileService
    {
        Task<IActionResult> GetVoucherFileAsync(string voucherFileName);
        Task<string> SaveVoucherAsync(IFormFile file, string numeroOperacion);
    }
}