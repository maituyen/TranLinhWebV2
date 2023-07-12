using System.Net;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using File = Google.Apis.Drive.v3.Data.File;

namespace MyProject.Models
{
	public class GoogleDrive
    {
        private Drive googleDrive;
        public GoogleDrive()
		{
		}
        public string jsonCredentials()
        {
            return @"{
                    'type': 'service_account',
                    'project_id': 'your-project-id',
                    'private_key_id': 'your-private-key-id',
                    'private_key': 'your-private-key',
                    'client_email': 'your-client-email',
                    'client_id': 'your-client-id',
                    'auth_uri': 'https://accounts.google.com/o/oauth2/auth',
                    'token_uri': 'https://accounts.google.com/o/oauth2/token',
                    'auth_provider_x509_cert_url': 'https://www.googleapis.com/oauth2/v1/certs',
                    'client_x509_cert_url': 'https://www.googleapis.com/robot/v1/metadata/x509/your-client-email'
                }";
        }
        public virtual void CreateFolder()
        { 
             
            GoogleCredential credential = GoogleCredential.FromJson(jsonCredentials())
                .CreateScoped(DriveService.ScopeConstants.Drive);
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Tên ứng dụng của bạn",
            });

            // Tạo đối tượng thư mục mới
            var folderMetadata = new File()
            {
                Name = "Tên thư mục",
                MimeType = "application/vnd.google-apps.folder"
            };

            // Tạo thư mục trên Google Drive
            var request = service.Files.Create(folderMetadata);
            request.Fields = "id";
            var folder = request.Execute();

            // Lấy ID của thư mục mới tạo
            string folderId = folder.Id;

            Console.WriteLine("Thư mục đã được tạo thành công. ID của thư mục: " + folderId);
        }
        public virtual void FolderExsist()
        {
            GoogleCredential credential = GoogleCredential.FromJson(jsonCredentials())
                .CreateScoped(DriveService.ScopeConstants.Drive);
            // Khởi tạo dịch vụ Google Drive
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Tên ứng dụng của bạn",
            });

            // Tìm kiếm thư mục theo tên
            string folderName = "Tên thư mục cần kiểm tra";
            string folderId = null;

            // Gửi yêu cầu truy vấn danh sách các thư mục
            var request = service.Files.List();
            request.Q = $"mimeType='application/vnd.google-apps.folder' and name='{folderName}' and trashed=false";
            request.Fields = "files(id)";
            var result = request.Execute();

            // Kiểm tra kết quả trả về
            if (result.Files != null && result.Files.Count > 0)
            {
                // Thư mục tồn tại
                folderId = result.Files[0].Id;
                Console.WriteLine("Thư mục tồn tại. ID của thư mục: " + folderId);
            }
            else
            {
                // Thư mục không tồn tại
                Console.WriteLine("Thư mục không tồn tại.");
            }
        }
        public virtual void UploadFile(string filePath)
        {
            GoogleCredential credential = GoogleCredential.FromJson(jsonCredentials())
                .CreateScoped(DriveService.ScopeConstants.Drive);
            // Khởi tạo dịch vụ Google Drive
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Tên ứng dụng của bạn",
            });

            // Tạo đối tượng tệp tin mới
            var fileMetadata = new File()
            {
                Name = Path.GetFileName(filePath)
            };

            // Đường dẫn đến tệp tin cần upload
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                // Upload tệp tin lên Google Drive
                var request = service.Files.Create(fileMetadata, stream, "mimeType của tệp tin");
                request.Fields = "id";
                var file = request.Upload();

                // Lấy ID của tệp tin đã upload
                //string fileId = file.Id;

                Console.WriteLine("Tệp tin đã được upload thành công. ID của tệp tin: " );
            }
        }
        
}
} 

