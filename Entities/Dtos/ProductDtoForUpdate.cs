﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public record ProductDtoForUpdate : ProductDto
    {
        public bool Showcase {get; set;}
    }
}
