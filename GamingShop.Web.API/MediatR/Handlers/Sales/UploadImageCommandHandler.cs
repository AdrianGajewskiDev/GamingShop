using GamingShop.Service.Services;
using GamingShop.Web.API.MediatR.Commands.Sales;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GamingShop.Web.API.MediatR.Handlers.Sales
{
    public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, bool>
    {
        private readonly IImage _imageService;

        public UploadImageCommandHandler(IImage imageService)
        {
            _imageService = imageService;
        }

        public async Task<bool> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.ID is string)
                {
                    await _imageService.UploadImageAsync(request.ID as string, request.Image, request.ImageType);
                    return true;
                }

                await _imageService.UploadImageAsync((int)request.ID, request.Image, request.ImageType);

                return true;
            }
            catch(Exception)
            {
                return false;
            }
          
        }
    }
}
