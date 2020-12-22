using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeddingApp.API.Models;

namespace WeddingApp.API.Data
{
    public class PlaceRepository : IPlaceRepository
    {
        private readonly DataContext _context;

        public PlaceRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<PhotoForPlace> GetMainPhotoForPlace(int placeId)
        {
            return await _context.PhotosForPlaces.Where(u => u.PlaceId == placeId)
                .FirstOrDefaultAsync(p => p.IsMain);
        }

        public async Task<PhotoForPlace> GetPhoto(int id)
        {
            var photo = await _context.PhotosForPlaces.FirstOrDefaultAsync(p => p.Id == id);
            return photo;
        }

        public async Task<Place> GetPlace(int id)
        {
            var place = await _context.Places.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);

            return place;
        }

        public async Task<IEnumerable<Place>> GetPlaces()
        {
            var places = await _context.Places.Include(p => p.Photos).ToListAsync();

            return places;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}