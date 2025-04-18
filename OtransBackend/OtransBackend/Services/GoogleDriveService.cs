using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace OtransBackend.Services
{
    public class GoogleDriveService
    {
        private readonly DriveService _driveService;

        public GoogleDriveService()
        {
            var credentialPath = Path.Combine(Directory.GetCurrentDirectory(), "GoogleDrive", "credentials.json");

            using var stream = new FileStream(credentialPath, FileMode.Open, FileAccess.Read);
            var credPath = Path.Combine(Directory.GetCurrentDirectory(), "GoogleDrive", "token.json");

            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                new[] { DriveService.Scope.DriveFile },
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;

            _driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "OtransDrive"
            });
        }

        public async Task<string> UploadFileAsync(IFormFile file, string customName)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = customName
            };

            using var stream = file.OpenReadStream();

            var request = _driveService.Files.Create(fileMetadata, stream, file.ContentType);
            request.Fields = "id";
            await request.UploadAsync();

            var uploadedFile = request.ResponseBody;

            if (uploadedFile?.Id == null)
                throw new Exception("Falló la subida a Google Drive");

            // 🔐 Permiso público de lectura
            var permission = new Google.Apis.Drive.v3.Data.Permission()
            {
                Role = "reader",
                Type = "anyone"
            };

            await _driveService.Permissions.Create(permission, uploadedFile.Id).ExecuteAsync();

            // 🌐 Retornar la URL de visualización
            return $"https://drive.google.com/file/d/{uploadedFile.Id}/view";
        }

    }
}
