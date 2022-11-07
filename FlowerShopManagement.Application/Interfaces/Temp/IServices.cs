// ******************** THIS INTERFACE IS ONLY USED FOR REFERENCE FOR NOW (DONT PAY ATTENTION TO IT) *********************

namespace FlowerShopManagement.Application.Interfaces.Temp
{
    public interface IServices<Placeholder> where Placeholder : new()
    {

        public List<Placeholder> Get();

        public Placeholder Get(string id);

        public void Create(Placeholder customer);

        public void Remove(string id);

        public void Update(string id, Placeholder customer);
    }

}
