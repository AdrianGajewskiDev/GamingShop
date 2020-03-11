using GamingShop.Data.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GamingShop.Service.Services
{
    public interface IImage
    {
        Task UploadImageAsync(Image image);
        string GetImagePathForGame(int id);
        string GetImageNameForGame(int id);

    }
}
