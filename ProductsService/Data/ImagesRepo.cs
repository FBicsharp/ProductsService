using Microsoft.EntityFrameworkCore;
using ProductsService.Data;
using ProductsService.Model;



namespace ImagesService.Data
{
    public class ImagesRepo : IImagesRepo
    {
        private readonly ProductDbContext _context;

        public ImagesRepo(ProductDbContext context)
        {
            _context = context;
        }
        #region ASYNC


        public async Task CreateImageAsync(Image image, CancellationToken cancellationToken)
            => await _context.Images.AddAsync(image, cancellationToken);


        public async Task DeleteImageAsync(Guid company, Guid id, CancellationToken cancellationToken)
        {
            var image = await _context.Images.FirstOrDefaultAsync(p => p.id == id && p.company == company);
            if (image != null)
                _context.Images.Remove(image);            
                //image.state = ImageState.Inactive;
        }

        public async Task UpdateImageAsync(Image updateImage, CancellationToken cancellationToken)
        {
            var image = await _context.Images.FirstOrDefaultAsync(p => p.id == updateImage.id && p.company == updateImage.company);
            if (image == null)
                return;
            image.description = updateImage.description;            
            image.last_update = DateTime.UtcNow;
            image.external_storege_id = updateImage.external_storege_id;
            _context.Images.Update(image);            
        }


        public async Task<IEnumerable<Image>> GetAllImagesAsync(Guid company,CancellationToken cancellationToken)
            => await _context.Images.Where(c=>c.company==company).ToListAsync();


        public async Task<Image> GetImageByIdAsync(Guid company,Guid id, CancellationToken cancellationToken)
        {
            var image = await _context.Images.FirstOrDefaultAsync(p => p.id == id && p.company==company, cancellationToken);
            if (image != null)
                return image;

            return new Image();
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
            => await _context.SaveChangesAsync(cancellationToken) > 0;
        #endregion

        #region SYNCRONOUS


        public void CreateImage(Image image)
            => CreateImageAsync(image, CancellationToken.None).GetAwaiter().GetResult();

        public void DeleteImage(Guid company, Guid id)
            => DeleteImageAsync(company,id, CancellationToken.None).GetAwaiter().GetResult();

        public IEnumerable<Image> GetAllImages(Guid company)
            => GetAllImagesAsync(company,CancellationToken.None).GetAwaiter().GetResult();


        public Image GetImageById(Guid company, Guid id)
            => GetImageByIdAsync(company,id, CancellationToken.None).GetAwaiter().GetResult();

        public bool SaveChanges()
            => SaveChangesAsync(CancellationToken.None).GetAwaiter().GetResult();
        public void UpdateImage(Image updateImage)
            => UpdateImageAsync(updateImage, CancellationToken.None).GetAwaiter().GetResult();

        #endregion


    }

}
