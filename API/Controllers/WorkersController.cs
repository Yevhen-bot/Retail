using API.DTOs;
using API.Mappers;
using Core.ValueObj;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkersController : ControllerBase
    {
        private readonly WorkerService _workerService;
        private readonly Mapper _mapper;

        public WorkersController(WorkerService workerService, Mapper mapper)
        {
            _workerService = workerService;
            _mapper = mapper;
        }

        [Authorize(Policy = "Owner")]
        [HttpPost("add_manager")]
        public async Task<IActionResult> AddManager([FromBody] CreateWorkerModel m)
        {
            var obj = _mapper.GetWorker(m);
            try
            {
                await _workerService.AddManager((Name)obj[0], (Age)obj[1], (Email)obj[2], (Adress)obj[3], (Salary)obj[4], (string)obj[5], (int)obj[6]);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Created();
        }

        [Authorize(Policy = "Manager")]
        [HttpPost("add_worker")]
        public async Task<IActionResult> AddWorker([FromBody] CreateWorkerModel m)
        {
            var obj = _mapper.GetWorker(m);
            try
            {
                await _workerService.AddWorker((Name)obj[0], (Age)obj[1], (Email)obj[2], (Adress)obj[3], (Salary)obj[4], (string)obj[5], (int)obj[6]);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Created();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel m)
        {
            try
            {
                await _workerService.Login(m.Password, m.Email);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [Authorize(Policy = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetWorkers()
        {
            try
            {
                return Ok(new { Workers = await _workerService.GetWorkers() });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
