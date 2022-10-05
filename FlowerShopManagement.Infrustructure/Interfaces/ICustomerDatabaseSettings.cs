// ******************** THIS INTERFACE IS ONLY USED FOR REFERENCE FOR NOW *********************

namespace FlowerShopManagement.Infrustructure.Interfaces
{
    public interface IDatabaseSettings
    {
        string ConnectionString { get; set; }

        string DatabaseName { get; set; }

    }
}
