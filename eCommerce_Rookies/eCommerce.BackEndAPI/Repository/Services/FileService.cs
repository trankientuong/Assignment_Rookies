using eCommerce.BackEndAPI.Repository.IServices;

namespace eCommerce.BackEndAPI.Repository.Services
{
    public class FileService : IFileService
    {
        public async Task DeleteFile(string fileName, string folder)
        {
            string folderPath = Path.Combine(WebHostEnvironmentHelper.GetWebRootPath(), folder);
            string path = Path.Combine(folderPath, fileName);
            if (File.Exists(path))
            {
                await Task.Run(() => File.Delete(path));
            }
        }

        public async Task<string> SaveFile(IFormFile file, string folder)
        {
            string fileName = $"{Guid.NewGuid()}.jpeg";
            string folderPath = Path.Combine(WebHostEnvironmentHelper.GetWebRootPath(), folder);
            string path = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }
    }
}
