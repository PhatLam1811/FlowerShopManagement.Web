namespace FlowerShopManagement.Core.Entities
{
    public class InforDelivery
    {
        public string? Name { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? District { get; set; } = string.Empty;
        public string? Commune { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public bool IsDefault { get; set; } = false;
    }
}
