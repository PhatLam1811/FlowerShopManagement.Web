using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Models
{
    public class InforDeliveryModel
    {
        public string? Name { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public bool IsDefault { get; set; } = false;

        public InforDeliveryModel() { }
        public InforDeliveryModel(InforDelivery entity) {
            Name = entity.Name;
            Phone = entity.Phone;
            Address = entity.Address;
            IsDefault = entity.IsDefault;
        }
        public InforDelivery ToEntity() {
            return new InforDelivery() { Address= this.Address, Name = this.Name,
                IsDefault= this.IsDefault, Phone = this.Phone };
        }

    }
}
