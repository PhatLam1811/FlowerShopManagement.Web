using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Models
{
    public class InforDeliveryModel
    {
        public string? FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public bool IsDefault { get; set; } = false;

        public InforDeliveryModel() { }
        public InforDeliveryModel(InforDelivery entity) {
            FullName = entity.Name;
            PhoneNumber = entity.Phone;
            Address = entity.Address;
            IsDefault = entity.IsDefault;
        }
        public InforDelivery ToEntity() {
            return new InforDelivery() { Address= this.Address, Name = this.FullName,
                IsDefault= this.IsDefault, Phone = this.PhoneNumber
            };
        }

    }
}
