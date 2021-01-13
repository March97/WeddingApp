using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using WeddingApp.API.Data;
using WeddingApp.API.Dtos;
using WeddingApp.API.Helpers;
using WeddingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WeddingApp.API.Services
{
    public class PhotosForPlaceService : IPhotosForPlaceService
    {
        private readonly IPlaceRepository _placeRepo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        public PhotosForPlaceService(IPlaceRepository placeRepo, IMapper mapper,
        IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _placeRepo = placeRepo;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }
        public async Task<PhotoForReturnDto> GetPhoto(int id) 
        {
            var photoFromRepo = await _placeRepo.GetPhoto(id);
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return photo;
        } 

        public async Task<PhotoForReturnDto> AddPhotoForPlace(int userId, int placeId, 
            PhotoForCreationDto photoForCreationDto) 
        {
            var placeFromRepo = await _placeRepo.GetPlace(placeId);

            var file = photoForCreationDto.File;
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation()
                            .Width(660).Height(495).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<PhotoForPlace>(photoForCreationDto);

            if (!placeFromRepo.Photos.Any(u => u.IsMain))
                photo.IsMain = true;

            placeFromRepo.Photos.Add(photo);

            if (await _placeRepo.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return photoToReturn;
            }

            throw new System.Exception("Could not add the photo");
        }

        public async Task<bool> SetMainPhoto(int userId, int placeId, int id)
        {
            var place = await _placeRepo.GetPlace(placeId);

            if (!place.Photos.Any(p => p.Id == id))
                throw new System.Exception("Unauthorized");

            var photoFromRepo = await _placeRepo.GetPhoto(id);

            if (photoFromRepo.IsMain)
                throw new System.Exception("This is already the mian photo");

            var currentMainPhoto = await _placeRepo.GetMainPhotoForPlace(placeId);
            currentMainPhoto.IsMain = false;

            photoFromRepo.IsMain = true;

            if (await _placeRepo.SaveAll())
                return true;

            throw new System.Exception("Could not set photo to main");
        }

        public async Task<bool> DeletePhoto(int userId, int placeId, int id) 
        {
            var place = await _placeRepo.GetPlace(placeId);

            if (!place.Photos.Any(p => p.Id == id))
                throw new System.Exception("Unauthorized");

            var photoFromRepo = await _placeRepo.GetPhoto(id);

            if (photoFromRepo.IsMain)
                throw new System.Exception("You cannot delete your main photo");

            if (photoFromRepo.PublicId != null)
            {

                var deleteParams = new DeletionParams(photoFromRepo.PublicId);

                var result = _cloudinary.Destroy(deleteParams);

                if (result.Result == "ok") {
                    _placeRepo.Delete(photoFromRepo);
                }
            }

            if (photoFromRepo.PublicId == null)
            {
                _placeRepo.Delete(photoFromRepo);
            }

            if (await _placeRepo.SaveAll())
                return true;

            throw new System.Exception("Failed to delete the photo");
        }
        
    }
}