using FlowerShopManagement.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace FlowerShopManagement.Application.Models
{
    public class InforDeliveryModel
    {
        [Required]
        [RegularExpression(@"([a-zA-Z])")]
        public string? FullName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^([\+]?84[-]?|[0])?[1-9][0-9]{8}$")]
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
