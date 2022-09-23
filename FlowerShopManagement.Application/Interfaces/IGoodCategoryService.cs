using FlowerShopManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FlowerShopManagement.Core.Interfaces
{
    public interface IGoodCategoryService
    {
        public List<GoodCategory> Get();

        public GoodCategory? Get(string id);

        public void Create(GoodCategory customer);

        public void Remove(string id);

        public void Update(string id, GoodCategory customer);
    }
}
