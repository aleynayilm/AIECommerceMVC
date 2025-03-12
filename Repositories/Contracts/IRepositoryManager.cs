﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IRepositoryManager
    {
        IProductRepository ProductR { get; }
        ICategoryRepository CategoryR { get; }
        IOrderRepository OrderR { get; }
        void Save();
    }
}
