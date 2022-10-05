using FlowerShopManagement.Infrustructure.Interfaces;

// ******************** THIS IMPLEMENTATION IS ONLY USED FOR REFERENCE FOR NOW *********************

namespace FlowerShopManagement.Infrustructure.DatabaseSettings
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;

    }
}
