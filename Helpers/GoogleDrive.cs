using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyProject.Helpers
{
  public  class  GoogleDrive
    {
        static string[] Scopes = { DriveService.Scope.Drive };
        static string ApplicationName = "TrầnLinhMobile";
         
    public static void CreateFolder(string folderName)
        {
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = GetCredentials(),
                ApplicationName = ApplicationName,
            });
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = folderName,
                MimeType = "application/vnd.google-apps.folder",
                Description="Xin chào nè",
            };
            var request = service.Files.Create(fileMetadata);
            request.Fields = "id";
            var file = request.Execute();
            Console.WriteLine("Folder ID: " + file.Id);

        }
        private static void DeleteFileFolder(string id, DriveService service)
        {
            var request = service.Files.Delete(id);
            request.Execute();

        }
        private static void UploadBasicImage(string path, DriveService service)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File();
            fileMetadata.Name = Path.GetFileName(path);
            fileMetadata.MimeType = "image/jpeg";
            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
            {
                request = service.Files.Create(fileMetadata, stream, "image/jpeg");
                request.Fields = "id";
                request.Upload();
            }

            var file = request.ResponseBody;

            Console.WriteLine("File ID: " + file.Id);

        }

        private static void ListFiles(DriveService service, ref string pageToken)
        {
            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 10;
            //listRequest.Fields = "nextPageToken, files(id, name)";
            listRequest.Fields = "nextPageToken, files(name)";
            listRequest.PageToken = pageToken;
            listRequest.Q = "mimeType='image/jpeg'";

            // List files.
            var request = listRequest.Execute();


            if (request.Files != null && request.Files.Count > 0)
            {


                foreach (var file in request.Files)
                {
                    Console.WriteLine("{0}", file.Name);
                }

                pageToken = request.NextPageToken;

                if (request.NextPageToken != null)
                {
                    Console.WriteLine("Press any key to conti...");
                    Console.ReadLine();



                }


            }
            else
            {
                Console.WriteLine("No files found.");

            }


        }

        private static UserCredential GetCredentials()
        {
            
            UserCredential credential;
            var folderName = "";
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "client_secret.json");
            using (var stream = new FileStream(pathToSave, FileMode.Open, FileAccess.Read))
            { 
                string filePath = ".credentials/drive-dotnet-quickstart.json";
                  folderName = Path.Combine("wwwroot", filePath);
                 pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(pathToSave, true)).Result;
                // Console.WriteLine("Credential file saved to: " + credPath);
            }

            return credential;
        }
    }
}