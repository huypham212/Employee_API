using AppS3.Data;
using AppS3.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppS3.Services.PositionService
{
    public class PositionService : IPositionService
    {
        private readonly AppS3DbContext _context; 

        public PositionService(AppS3DbContext context)
        {
            _context = context;
        }

        public async Task CreatePosition(Position position)
        {
            await _context.Positions.AddAsync(position);
        }

        public async Task DeletePosition(int id)
        {
            var del_position = await _context.Positions.FindAsync(id);

            _context.Remove(del_position);
        }

        public async Task<IEnumerable<Position>> GetAllPosition()
        {
            var position = await _context.Positions.ToListAsync();

            return position;
        }

        public async Task<Position> GetPositionById(int id)
        {
            var position = await _context.Positions.FirstOrDefaultAsync(key => key.Id == id);

            return position;
        }

        public async Task<bool> SaveChange()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task UpdatePosition(Position position)
        {
            var exist_position = await _context.Positions.FindAsync(position.Id);

            if(exist_position != null)
            {
                exist_position.PositionName = position.PositionName;
                exist_position.Salary = position.Salary;
            }
        }
    }
}
