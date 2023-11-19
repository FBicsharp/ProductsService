
namespace ProductsService.Model
{
    public interface IBolbModelRequest
    {
        string ContainerName { get; }
        string ResourceName { get; set; }
        Stream ResourceStream { get; set; }
    }
}