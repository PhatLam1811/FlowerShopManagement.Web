using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using FlowerShopManagement.Infrustructure.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShopManagement.Application.Interfaces
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
