using AppS3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppS3.Services.PositionService
{
    public interface IPositionService
    {
        Task<bool> SaveChange();

        Task<IEnumerable<Position>> GetAllPosition();

        Task<Position> GetPositionById(int id);

        public Task CreatePosition(Position position);

        public Task UpdatePosition(Position position);

        public Task DeletePosition(int id);
    }
}
