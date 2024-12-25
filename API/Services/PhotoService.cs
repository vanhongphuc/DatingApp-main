using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            // Tạo một đối tượng ImageUploadParams để đặt các thông số tải lên
            var uploadResult = new ImageUploadResult();
            if(file.Length > 0){
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams{
                    File = new FileDescription(file.FileName,stream),// Thông tin về file hình ảnh và dữ liệu stream
                    // Thực hiện các biến đổi trên hình ảnh trước khi tải lên
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };
                 // Gọi phương thức UploadAsync của đối tượng Cloudinary để tải lên hình ảnh
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult; // Trả về kết quả tải lên hình ảnh từ Cloudinary
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result;
        }
    }
}