using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayFlow.DOMAIN.Core.Interfaces;

namespace PayFlow.DOMAIN.Core.Servicies
{
    public class FileService : IFileService
    {
        private readonly string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf" };
        private readonly long maxFileSize = 5 * 1024 * 1024; // 5MB
        private readonly string fileDirectory = @"Imagenes";

        public FileService()
        {
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);  // Crea el directorio si no existe
            }
        }

        // Guardar el voucher (archivo) en el servidor
        public async Task<string> SaveVoucherAsync(IFormFile file, string numeroOperacion)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                throw new Exception("El archivo debe ser JPEG, PNG o PDF.");
            }

            if (file.Length > maxFileSize)
            {
                throw new Exception("El archivo no debe exceder los 5 MB.");
            }

            // Crear una ruta personalizada para el archivo usando el número de operación
            var fileName = $"{numeroOperacion}{extension}";
            var filePath = Path.Combine(fileDirectory, fileName);

            // Guardar el archivo en el servidor
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath; // Devolver la ruta del archivo guardado
        }

        // Recuperar el archivo para visualización o descarga
        public async Task<IActionResult> GetVoucherFileAsync(string voucherFileName)
        {
            var filePath = Path.Combine(fileDirectory, voucherFileName);

            if (!File.Exists(filePath))
            {
                throw new Exception("El archivo no existe.");
            }

            var fileBytes = await File.ReadAllBytesAsync(filePath);
            var fileExtension = Path.GetExtension(filePath).ToLower();

            // Servir el archivo dependiendo de su tipo
            switch (fileExtension)
            {
                case ".jpg":
                case ".jpeg":
                case ".png":
                    return new FileContentResult(fileBytes, "image/jpeg"); // Devolver imagen
                case ".pdf":
                    return new FileContentResult(fileBytes, "application/pdf"); // Devolver PDF
                default:
                    throw new Exception("Formato de archivo no soportado.");
            }
        }
    }
}
