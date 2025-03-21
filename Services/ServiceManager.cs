﻿using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public class ServiceManager : IServiceManager
	{
		private readonly IProductService _productService;
		private readonly ICategoryService _categoryService;
		private readonly IOrderService _orderService;
		private readonly IAuthService _authService;
        private readonly IReplicateAIService _replicateAIService;

		public ServiceManager(IProductService productService, ICategoryService categoryService, IOrderService orderService, IAuthService authService, IReplicateAIService replicateAIService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _orderService = orderService;
            _authService = authService;
            _replicateAIService = replicateAIService;
        }
        public IProductService ProductService => _productService;

		public ICategoryService CategoryService => _categoryService;

        public IOrderService OrderService => _orderService;

        public IAuthService AuthService => _authService;

        public IReplicateAIService ReplicateAIService => _replicateAIService;
    }
}
