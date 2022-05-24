using AdCourier.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ajkerdeal.Controllers.AdCourier
{
    [Produces("application/json")]
    [Route("api/ImageProcessing")]
    public class ImageProcessingController : ControllerBase
    {
        private readonly IImageProcessingService _imageProcessingService;

        public ImageProcessingController(IImageProcessingService imageProcessingService)
        {
            _imageProcessingService = imageProcessingService;
        }
    }
}
