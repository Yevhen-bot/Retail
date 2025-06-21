using API.DTOs;
using API.Mappers;
using Core.Interfaces;
using Data_Access.Repos;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnersController : ControllerBase
    {
        private readonly OwnerService _ownerService;
        private readonly Mapper _mapper;

        public OwnersController(OwnerService service, Mapper mapper)
        {
            _ownerService = service;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel m)
        {
            try
            {
                await _ownerService.Register(_mapper.Registration(m));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Created();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel m)
        {
            try
            {
                await _ownerService.Login(m.Password, m.Email);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [Authorize(Policy = "Owner")]
        [HttpPost("simulate_day")]
        public async Task<IActionResult> SimulateDay()
        {
            try
            {
                await _ownerService.SimulateDay(int.Parse(HttpContext.User.FindFirst("Id")?.Value));
            } catch(Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}
