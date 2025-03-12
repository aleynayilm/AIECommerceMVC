using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace AIECommerce.Web.Components
{
    public class UserSummaryViewComponent:ViewComponent
    {
        private readonly IServiceManager _manager;
        public UserSummaryViewComponent(IServiceManager manager)
        {
            _manager=manager;
        }
        public string Invoke(){
            return _manager.AuthService.GetAllUsers().Count().ToString();
        }
    }
}