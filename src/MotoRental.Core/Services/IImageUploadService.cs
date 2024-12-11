using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoRental.Core.Services
{
    public interface IImageUploadService
    {
        Task<string> UploadBase64Image(string base64Image, string container);
    }
}