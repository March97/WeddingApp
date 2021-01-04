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

namespace WeddingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/places/{placeId}/photos")]
    [ApiController]
    public class PhotosForPlaceController : ControllerBase
    {
        private readonly IPlaceRepository _placeRepo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        public PhotosForPlaceController(IPlaceRepository placeRepo, IMapper mapper,
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

        [HttpGet("{id}", Name = "GetPhotoForPlace")]
        public async Task<IActionResult> GetPhoto(int id) 
        {
            var photoFromRepo = await _placeRepo.GetPhoto(id);
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        } 

        [HttpPost]
        public async Task<IActionResult> AddPhotoForPlace(int userId, int placeId,
            [FromForm]PhotoForCreationDto photoForCreationDto) 
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

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
                        // Transformation = new Transformation()
                        //     .Width(500).Height(500).Crop("fill").Gravity("face")
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
                return CreatedAtRoute("GetPhotoForPlace", new {userId = userId, placeId = placeId, id = photo.Id}, photo);
            }

            return BadRequest("Could not add the photo");
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int placeId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var place = await _placeRepo.GetPlace(placeId);

            if (!place.Photos.Any(p => p.Id == id))
                return Unauthorized();

            var photoFromRepo = await _placeRepo.GetPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("This is already the mian photo");

            var currentMainPhoto = await _placeRepo.GetMainPhotoForPlace(placeId);
            currentMainPhoto.IsMain = false;

            photoFromRepo.IsMain = true;

            if (await _placeRepo.SaveAll())
                return NoContent();

            return BadRequest("Could not set photo to main");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int placeId, int id) 
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var place = await _placeRepo.GetPlace(placeId);

            if (!place.Photos.Any(p => p.Id == id))
                return Unauthorized();

            var photoFromRepo = await _placeRepo.GetPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("You cannot delete your main photo");

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
                return Ok();

            return BadRequest("Failed to delete the photo");
        }
    }
}