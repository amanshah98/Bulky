﻿
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Data;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
       
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {

            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork._productRepository.GetAll(includeProperties:"Category").ToList();
            //IEnumerable<SelectListItem> CategoryList = _unitOfWork._categoryRepository.GetAll().Select(u => new SelectListItem
            //{
            //    Text = u.Name,
            //    Value=u.Id.ToString(),

            //});
            return View(objProductList);
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork._categoryRepository
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()

                }),
                Product = new Product()
            };
            if(id==null || id == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork._productRepository.Get(u=>u.Id==id);
                return View(productVM);
            }
            
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM,IFormFile? file)
        {
            //if (obj.Name == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
            //}
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file!=null)
                {
                    string fileName=Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);
                    string productpath =Path.Combine(wwwRootPath, @"images\product");

                    if(!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //delete the old image
                        var oldImagePath =
                            Path.Combine(wwwRootPath,productVM.Product.ImageUrl.TrimStart('\\'));

                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using(var fileStream=new FileStream(Path.Combine(productpath, fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }
                if(productVM.Product.Id == 0)
                {
                    _unitOfWork._productRepository.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork._productRepository.Update(productVM.Product);
                }
                //_unitOfWork._productRepository.Add(productVM.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product Created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork._categoryRepository
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()

                });
                return View(productVM);
            }
            

        }

        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product? productFromDb = _unitOfWork._productRepository.Get(u => u.Id == id);
        //    //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id == id);
        //    //Category? categoryFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();
        //    if (productFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(productFromDb);
        //}
        //[HttpPost]
        //public IActionResult Edit(Product obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork._productRepository.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product Updated successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View();

        //}

        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product? productFromDb =_unitOfWork._productRepository.Get(u => u.Id == id);
        //    //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id == id);
        //    //Category? categoryFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();
        //    if (productFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(productFromDb);
        //}
        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePOST(int? id)
        //{
        //    Product? obj = _unitOfWork._productRepository.Get(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork._productRepository.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Product Deleted successfully";
        //    return RedirectToAction("Index");
        //}

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll() 
        {
            List<Product> objProductList = _unitOfWork._productRepository.GetAll(includeProperties: "Category").ToList();
            return Json(new {data=objProductList});
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork._productRepository.Get(u=>u.Id == id);
            if(productToBeDeleted == null)
            {
                return Json(new {success=false,message ="Error while deleting"});
            }
            var oldImagePath =
                            Path.Combine(_webHostEnvironment.WebRootPath,
                            productToBeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork._productRepository.Remove(productToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleting Successful" });
        }
        #endregion
    }
}
