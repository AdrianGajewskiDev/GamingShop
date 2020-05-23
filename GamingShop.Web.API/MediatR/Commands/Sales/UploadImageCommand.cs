using GamingShop.Service;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace GamingShop.Web.API.MediatR.Commands.Sales
{
    public class UploadImageCommand : IRequest<bool> 
    {
        public UploadImageCommand(IFormFile image, object iD, ImageType imageType)
        {
            Image = image;
            ID = iD;
            ImageType = imageType;
        }

        public IFormFile Image { get; private set; }
        public object ID { get; private set; }
        public ImageType ImageType { get; private set; }
    }
}
