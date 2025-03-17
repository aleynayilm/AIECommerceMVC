using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Dtos;

namespace Services.Contracts
{
    public interface IReplicateAIService
    {
        Task<string> GenerateProductImageAsync(string productName, string userImageUrl); 
    }
}