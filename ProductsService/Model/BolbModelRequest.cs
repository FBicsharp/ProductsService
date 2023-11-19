namespace ProductsService.Model
{
    public class BolbModelRequest : IBolbModelRequest
    {
        public string ContainerName { get => string.Concat(companySettings.CompanyId, string.IsNullOrEmpty(companySettings.Container) ? "" : "-", companySettings.Container).ToLower(); }

        public string ResourceName { get; set; }
        public Stream ResourceStream { get; set; }

        private readonly CompanySettings companySettings;

        public BolbModelRequest(CompanySettings companySettings)
        {
            this.companySettings = companySettings;
        }
    }



}