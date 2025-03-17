using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Entities.RequestParameters;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public class ProductManager : IProductService
	{
		private readonly IRepositoryManager _manager;
		private readonly IMapper _mapper;

		public ProductManager(IRepositoryManager manager, IMapper mapper)
		{
			_manager = manager;
			_mapper = mapper;
		}

        public void CreateProduct(ProductDtoForInsertion productDto)
        {
			Product product = _mapper.Map<Product>(productDto);
			if (!string.IsNullOrWhiteSpace(productDto.ImageUrl))
    {
        product.ImageUrl = productDto.ImageUrl; // Veritabanına kaydedilecek URL
    }
			_manager.ProductR.Create(product);
			_manager.Save();
        }

        public void DeleteOneProduct(int id)
        {
			Product product = GetOneProduct(id, false);

			if (product is not null) { 
			_manager.ProductR.DeleteOneProduct(product);
				_manager.Save();
			}
        }

        public IEnumerable<Product> GetAllProducts(bool trcakChanges)
		{
			return _manager.ProductR.GetAllProducts(trcakChanges);
		}

        public IEnumerable<Product> GetAllProductsWithDetails(ProductRequestParameters p)
        {
            return _manager.ProductR.GetAllProductsWithDetails(p);
        }

        public IEnumerable<Product> GetLastestProducts(int n, bool trackChanges)
        {
            return _manager.ProductR.FindAll(trackChanges).OrderByDescending(prd=>prd.ProductId).Take(n);
        }

        public Product? GetOneProduct(int id, bool trackChanges)
		{
			var product= _manager.ProductR.GetOneProduct(id, trackChanges);
			if (product is null)
				throw new Exception("Product not found!");
			return product;
		}

        public ProductDtoForUpdate GetOneProductForUpdate(int id, bool trackChanges)
        {
            var product= GetOneProduct(id, trackChanges);
			var productDto = _mapper.Map<ProductDtoForUpdate>(product);
			return productDto;
        }

        public IEnumerable<Product> GetShowcaseProducts(bool trackChanges)
        {
            var products= _manager.ProductR.GetShowcaseProducts(trackChanges);
			return products;
        }

        public void UpdateOneProduct(ProductDtoForUpdate productDto)
        {
			//var entity = _manager.ProductR.GetOneProduct(productDto.ProductId, true);
			//entity.ProductName= productDto.ProductName;
			//entity.Price= productDto.Price;
			var entity = _mapper.Map<Product>(productDto);
			_manager.ProductR.UpdateOneProduct(entity);
			_manager.Save();
        }
    }
}
