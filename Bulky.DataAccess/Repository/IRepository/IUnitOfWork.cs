using Bulky.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository _categoryRepository { get; }
        IProductRepository _productRepository { get; }
        ICompanyRepository _companyRepository { get; }
        IShoppingCartRepository _shoppingCart { get; }
        IApplicationUserRepository _applicationUserRepository { get; }
        void Save();
    }
}
