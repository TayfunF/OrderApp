using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Data.Repositories.IRepositories
{
    public interface IUnitOfWork
    {
        IAppUserRepository AppUser { get; }
        ICartRepository Cart { get; }
        ICategoryRepository Category { get; }
        IOrderDetailsRepository OrderDetails { get; }
        IOrderProductRepository OrderProduct { get; }
        IProductRepository Product { get; }

        void Save();
    }
}
