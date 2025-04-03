using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data; // Para la clase File de Google Drive
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO; // Para la clase File de System.IO

public class GoogleDriveService
{
    private readonly DriveService _driveService;

    public GoogleDriveService(string credentialPath)
    {
        var credential = GetGoogleCredentials(credentialPath).Result;
        _driveService = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "TuAplicacion", // Nombre de tu app
        });
    }

    private async Task<UserCredential> GetGoogleCredentials(string credentialPath)
    {
        UserCredential credential;
        using (var stream = new FileStream(credentialPath, FileMode.Open, FileAccess.Read)) // "FileStream" de System.IO
        {
            string credPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".credentials/drive-dotnet-quickstart.json");

            // Usar LocalServerCodeReceiver en lugar de un string
            var codeReceiver = new LocalServerCodeReceiver(); // Crea un receptor de código local para manejar la redirección

            // Usar el receptor de código con la URL de redirección local predeterminada
            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                new[] { DriveService.Scope.DriveFile },
                "user", CancellationToken.None, new FileDataStore(credPath, true), codeReceiver);
        }

        return credential;
    }

    public async Task<string> UploadFileAsync(string filePath, string mimeType)
    {
        var fileMetadata = new Google.Apis.Drive.v3.Data.File() // Aquí usamos Google.Apis.Drive.v3.Data.File
        {
            Name = Path.GetFileName(filePath)
        };

        FilesResource.CreateMediaUpload request;
        using (var fileStream = new FileStream(filePath, FileMode.Open)) // "FileStream" de System.IO
        {
            request = _driveService.Files.Create(fileMetadata, fileStream, mimeType);
            request.Fields = "id";
            await request.UploadAsync(CancellationToken.None);
        }

        var file = request.ResponseBody;
        return file.Id;
    }
}
