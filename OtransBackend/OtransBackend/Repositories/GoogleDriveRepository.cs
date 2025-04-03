using Google.Apis.Drive.v3.Data; // Para la clase File de Google Drive
using Microsoft.AspNetCore.Http;
using System.IO; // Para la clase File de System.IO
using System.Threading.Tasks;

public class GoogleDriveRepository
{
    private readonly GoogleDriveService _googleDriveService;

    public GoogleDriveRepository()
    {
        // Especifica la ruta correcta al archivo JSON
        var credentialPath = Path.Combine(Directory.GetCurrentDirectory(), "Utilities", "client_secret_741082527421-1e8jasp95vkv6b20sqo8i2pfh7kio26v.apps.googleusercontent.com.json");
        _googleDriveService = new GoogleDriveService(credentialPath); // Inicializamos el servicio con el archivo JSON
    }

    public async Task<string> SubirArchivoAsync(IFormFile archivo)
    {
        // Guardamos el archivo temporalmente en el servidor
        var filePath = Path.Combine(Path.GetTempPath(), archivo.FileName);

        using (var stream = new System.IO.FileStream(filePath, FileMode.Create)) // "FileStream" de System.IO
        {
            await archivo.CopyToAsync(stream); // Copiamos el archivo al sistema temporal
        }

        // Ahora subimos el archivo a Google Drive
        string fileId = await _googleDriveService.UploadFileAsync(filePath, archivo.ContentType);

        // Eliminamos el archivo temporal después de subirlo
        System.IO.File.Delete(filePath); // "File" de System.IO para eliminar el archivo temporal

        return fileId; // Retornamos el ID del archivo subido
    }
}
