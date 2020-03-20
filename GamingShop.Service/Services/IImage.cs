using GamingShop.Data.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GamingShop.Service.Services
{
    public interface IImage
    {
        Task UploadImageAsync(int ID,IFormFile image, ImageType type);
        Task UploadImageAsync(string ID, IFormFile image, ImageType type);
        string GetImagePathForGame(int id);
        string GetImageNameForGame(int id);
        string GetImagePathForUser(string userID);

    }
}
