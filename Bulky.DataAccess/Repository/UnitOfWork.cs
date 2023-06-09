﻿using Bulky.DataAccess.Data;
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

        public IShoppingCartRepository _shoppingCart { get; private set; }

        public IApplicationUserRepository _applicationUserRepository { get; private set; }

        public IOrderHeaderRepository _orderHeaderRepository { get; private set; }

        public IOrderDetailRepository _orderDetailRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            _categoryRepository = new CategoryRepository(_db);
            _productRepository = new ProductRepository(db);
            _productRepository = new ProductRepository(_db);
            _categoryRepository = new CategoryRepository(_db);
            _companyRepository = new CompanyRepository(_db);
            _shoppingCart = new ShoppingCartRepository(_db);
            _applicationUserRepository = new ApplicationUserRepository(_db);
            _orderHeaderRepository= new OrderHeaderRepository(_db);
            _orderDetailRepository= new OrderDetailRepository(_db);
        }



        public void Save()
        {
            _db.SaveChanges();
        }
    }
}