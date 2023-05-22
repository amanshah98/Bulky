using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulky.DataAccess.Repository.IRepository;

namespace Bulky.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository _categoryRepository { get; private set; }

        public IProductRepository _productRepository { get; private set; }

        public ICompanyRepository _companyRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            _productRepository = new ProductRepository(_db);
            _categoryRepository = new CategoryRepository(_db);
            _companyRepository = new CompanyRepository(_db);
        }



        public void Save()
        {
            _db.SaveChanges();
        }
    }
}