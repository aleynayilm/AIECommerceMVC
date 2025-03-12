using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace AIECommerce.Web.Components
{
    public class OrderInProgressViewComponent:ViewComponent
    {
        private readonly IServiceManager _manager;
        public OrderInProgressViewComponent(IServiceManager manager)
        {
            _manager=manager;
        }
        public string Invoke(){
            return _manager.OrderService.NumberOfInProcess.ToString();
        }
    }
}