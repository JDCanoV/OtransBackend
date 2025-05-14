using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;

namespace OtransBackend.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration config)
        {
            var cloudName = config["Cloudinary:CloudName"];
            var apiKey = config["Cloudinary:ApiKey"];
            var apiSecret = config["Cloudinary:ApiSecret"];

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadFileAsync(IFormFile file, string customName)
        {
            using var stream = file.OpenReadStream();
            var ext = Path.GetExtension(file.FileName).ToLower();

            // Determina si el archivo es una imagen o no
            if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif" || ext == ".bmp")
            {
                // Si es una imagen, usamos ImageUploadParams
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(customName, stream),
                    PublicId = customName
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult?.SecureUrl == null)
                {
                    throw new Exception("Error al subir la imagen a Cloudinary");
                }

                return uploadResult.SecureUrl.ToString();
            }
            else
            {
                // Si no es una imagen, usamos RawUploadParams (para PDFs, documentos, etc.)
                var uploadParams = new RawUploadParams()
                {
                    File = new FileDescription(customName, stream),
                    PublicId = customName
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult?.SecureUrl == null)
                {
                    throw new Exception("Error al subir el archivo a Cloudinary");
                }

                return uploadResult.SecureUrl.ToString();
            }
        }
    }
}
