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
        public IActionResult AddManager([FromBody] CreateWorkerModel m)
        {
            var obj = _mapper.GetWorker(m);
            try
            {
                _workerService.AddManager((Name)obj[0], (Age)obj[1], (Email)obj[2], (Adress)obj[3], (Salary)obj[4], (string)obj[5], (int)obj[6]);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Created();
        }

        [Authorize(Policy = "Manager")]
        [HttpPost("add_worker")]
        public IActionResult AddWorker([FromBody] CreateWorkerModel m)
        {
            var obj = _mapper.GetWorker(m);
            try
            {
                _workerService.AddWorker((Name)obj[0], (Age)obj[1], (Email)obj[2], (Adress)obj[3], (Salary)obj[4], (string)obj[5], (int)obj[6]);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Created();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel m)
        {
            try
            {
                _workerService.Login(m.Password, m.Email);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}
