using API.DTOs;
using API.Mappers;
using Core.ValueObj;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ClientService _service;
        private readonly Mapper _mapper;

        public ClientsController(ClientService service, Mapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] ClientRegistration m)
        {
            try
            {
                _service.Register(_mapper.RegisterClient(m));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Created();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel m)
        {
            try
            {
                _service.Login(m.Password, m.Email);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [Authorize(Policy = "Client")]
        [HttpPut("buy")]
        public IActionResult Buy([FromBody] SellModel m)
        {
            var l = _mapper.Buy(m);
            try
            {
                _service.MakeOrder((Product)l[0], (int)l[1], (int)l[2]);
            } catch(Exception e)
            {
                return BadRequest(e);
            }

            return Ok();
        }
    }
}
