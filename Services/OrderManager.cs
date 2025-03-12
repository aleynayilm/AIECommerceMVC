using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderManager : IOrderService
    {
        private readonly IRepositoryManager _manager;

        public OrderManager(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public IQueryable<Order> Orders => _manager.OrderR.Orders;

        public int NumberOfInProcess => _manager.OrderR.NumberOfInProcess;

        public void Complete(int id)
        {
            _manager.OrderR.Complete(id);
            _manager.Save();
        }

        public Order? GetOneOrder(int id)
        {
            return _manager.OrderR.GetOneOrder(id);
        }

        public void SaveOrder(Order order)
        {
            _manager.OrderR.SaveOrder(order);
            _manager.Save();
        }
    }
}
