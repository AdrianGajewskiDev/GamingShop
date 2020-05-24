using GamingShop.Data.DbContext;
using GamingShop.Data.Models;
using GamingShop.Service.Services;
using GamingShop.Web.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GamingShop.Service.Implementation
{

    public class ImageService : IImage
    {
        private ApplicationDbContext _context;
        private readonly ApplicationOptions _options;

        public ImageService(ApplicationDbContext context, IOptions<ApplicationOptions> options)
        {
            _context =  context;
            _options = options.Value;
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

        public string GetImagePathForUser(string userID)
        {

                var images = _context.Images.Where(img => img.UserID == userID);

                if (!images.Any())
                    return "Image Not Found";

                var sortedImages = images.OrderByDescending(img => img.Posted);

                return sortedImages.First().UniqueName;
        }

        public async Task UploadImageAsync(int ID, IFormFile image, ImageType type)
        {

                var id = ID.ToString();

                await UploadImageAsync(id, image, type);
        }

        public async Task UploadImageAsync(string ID, IFormFile image, ImageType type)
        {

                if (image == null)
                    throw new ArgumentNullException();

                var path = _options.ImagesPath;

                var uniqueName = $"{ID}_{image.FileName}";

                var filePath = Path.Combine(path, uniqueName);

                if (Directory.Exists(path))
                {
                    using (var sr = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(sr);
                    }
                }

                Image img = null;

                switch (type)
                {
                    case ImageType.GameCover:
                        img = new Image
                        {
                            UniqueName = uniqueName,
                            Path = filePath,
                            GameID = int.Parse(ID),
                            Posted = DateTime.UtcNow
                        };
                        break;
                    case ImageType.UserProfile:
                        img = new Image
                        {
                            UniqueName = uniqueName,
                            Path = filePath,
                            UserID = ID,
                            Posted = DateTime.UtcNow
                        };
                        break;
                    default:
                        break;
                }

                _context.Images.Add(img);

                await _context.SaveChangesAsync();

        }
    }
}
