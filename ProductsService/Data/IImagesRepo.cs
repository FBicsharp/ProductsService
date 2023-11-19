using ProductsService.Model;

namespace ProductsService.Data
{
    public interface IImagesRepo
    {
        /// <summary>
        /// Retrieves all images in the system.
        /// </summary>
        /// <returns>An IEnumerable of Image objects representing all images.</returns>
        IEnumerable<Image> GetAllImages(Guid company);

        /// <summary>
        /// Retrieves a image by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the image.</param>
        /// <returns>A Image object representing the image with the specified ID.</returns>
        Image GetImageById(Guid company, Guid id);

        /// <summary>
        /// Creates a new image in the system.
        /// </summary>
        /// <param name="image">The Image object to be created.</param>
        void CreateImage(Image image);

        /// <summary>
        /// Deletes a image from the system based on its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the image to be deleted.</param>
        void DeleteImage(Guid company, Guid id);

        void UpdateImage(Image updateImage);

        /// <summary>
        /// Saves changes made to the images in the system.
        /// </summary>
        /// <returns>True if changes were successfully saved; otherwise, false.</returns>
        bool SaveChanges();

        /// <summary>
        /// Asynchronously retrieves all images in the system.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for cancelling the asynchronous operation.</param>
        /// <returns>An asynchronous task that represents the operation. The task result is an IEnumerable of Image objects.</returns>
        Task<IEnumerable<Image>> GetAllImagesAsync(Guid company,CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously retrieves a image by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the image.</param>
        /// <param name="cancellationToken">Cancellation token for cancelling the asynchronous operation.</param>
        /// <returns>An asynchronous task that represents the operation. The task result is a Image object.</returns>
        Task<Image> GetImageByIdAsync(Guid company, Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously creates a new image in the system.
        /// </summary>
        /// <param name="image">The Image object to be created.</param>
        /// <param name="cancellationToken">Cancellation token for cancelling the asynchronous operation.</param>
        /// <returns>An asynchronous task that represents the operation.</returns>
        Task CreateImageAsync(Image image, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously deletes a image from the system based on its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the image to be deleted.</param>
        /// <param name="cancellationToken">Cancellation token for cancelling the asynchronous operation.</param>
        /// <returns>An asynchronous task that represents the operation.</returns>
        Task DeleteImageAsync(Guid company, Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously saves changes made to the images in the system.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for cancelling the asynchronous operation.</param>
        /// <returns>An asynchronous task that represents the operation. The task result is true if changes were successfully saved; otherwise, false.</returns>
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken);


        Task UpdateImageAsync(Image updateImage, CancellationToken cancellationToken);

    }
}
