using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppS3.Models;
using AppS3.Services.PositionService;
using Microsoft.AspNetCore.Authorization;
using AppS3.DTOs;
using Microsoft.Extensions.Logging;

namespace AppS3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PositionsController : ControllerBase
    {
        private readonly IPositionService _service;
        private readonly ILogger<PositionsController> _logger;

        public PositionsController(IPositionService service, ILogger<PositionsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: api/Positions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Position>>> GetPositions()
        {
            var position = await _service.GetAllPosition();

            if(position == null)
            {
                return NotFound();
            }

            return Ok(position);
        }

        // GET: api/Positions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Position>> GetPosition(int id)
        {
            var position = await _service.GetPositionById(id);

            if (position == null)
            {
                return NotFound();
            }

            return Ok(position);
        }

        // PUT: api/Positions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePosition(int id, Position position)
        {
            if(position == null)
            {
                return NoContent();
            }

            await _service.UpdatePosition(position);
            var result = await _service.SaveChange();

            if(result == false)
            {
                return BadRequest(new { msg = "Upadate Failed!" });
            }

            return Ok(new { position, msg = "Upadate Successful!" });
        }

        // POST: api/Positions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Position>> PostPosition(Position position)
        {
            var check_position = await _service.GetPositionById(position.Id);

            if(check_position == null)
            {
                await _service.CreatePosition(position);
                var result = await _service.SaveChange();

                if (result == false)
                {
                    return BadRequest(new { msg = "Create Failed!" });
                }

                return Ok(new { position, msg = "Create Successful!" });
            }

            return Conflict();
        }

        // DELETE: api/Positions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePosition(int id)
        {
            var del_position = await _service.GetPositionById(id);

            if(del_position == null)
            {
                return NotFound();
            }

            await _service.DeletePosition(id);
            var result = await _service.SaveChange();

            if (result == false)
            {
                return BadRequest(new { msg = "Delete Failed!" });
            }

            return Ok(new { del_position, msg = "Delete Successful!" });
        }
    }
}
