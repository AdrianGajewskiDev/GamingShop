using GamingShop.Data.Models;
using GamingShop.Service.Services;
using GamingShop.Web.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GamingShop.Service.Implementation
{

    public class ImageService : IImage
    {
        private readonly ApplicationDbContext _context;

        public ImageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetImageNameForGame(int id)
        {
            var image = _context.Images.Where(x => x.GameID == id).FirstOrDefault();

            if (image == null)
                return "Image not found!!";

            return image.UniqueName;
        }

        public string GetImagePathForGame(int id)
        {
            var image = _context.Images.Where(x => x.GameID == id).FirstOrDefault();

            if (image == null)
                return "Image not found!!";

            return image.Path;
        }

        public async Task UploadImageAsync(Image image)
        {
            _context.Images.Add(image);

            await _context.SaveChangesAsync();
        }


    }
}
