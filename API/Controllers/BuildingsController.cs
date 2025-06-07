using API.DTOs;
using API.Mappers;
using Core.Interfaces;
using Core.ValueObj;
using Data_Access.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingsController : ControllerBase
    {
        private readonly BuildingService _service;
        private readonly Mapper _mapper;

        public BuildingsController(BuildingService r, Mapper m)
        {
            _service = r;
            _mapper = m;
        }

        [Authorize(Policy = "Owner")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var b = _service.GetBuildings(int.Parse(HttpContext.User.FindFirst("Id")?.Value));
            return Ok(new { Buildings = _mapper.GetBuildingModels(b)});
        }

        [Authorize(Policy = "Owner")]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Building b = null;
            try
            {
                b = _service.GetBuilding(int.Parse(HttpContext.User.FindFirst("Id")?.Value), id);
            } catch(Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(_mapper.GetBuildingModels([b])[0]);
        }

        [Authorize(Policy = "Owner")]
        [HttpPost]
        public IActionResult AddBuilding([FromBody] CreateBuildingModel m)
        {
            var objs = _mapper.CreateBuilding(m);
            _service.CreateBuilding((string)objs[0], (double)objs[1], (Adress)objs[2], (BuildingRole)objs[3]);

            return Created();
        }

        [Authorize(Policy = "Owner")]
        [HttpDelete("{id}")]
        public IActionResult DeleteBuilding(int id)
        {
            try
            {
                _service.DeleteBuilding(int.Parse(HttpContext.User.FindFirst("Id")?.Value), id);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return NoContent();
        }

        [Authorize(Policy = "Warehouse_Worker")]
        [HttpPatch("add_products")]
        public IActionResult BringGoods(BringGoodsModel m)
        {
            var l = _mapper.GetPairGoodQuant(m);
            try
            {
                _service.AddGoods((Product)l[0], (int)l[1], (int)l[2]);
            } catch(Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [Authorize(Policy = "Warehouse_Worker")]
        [HttpPut("export")]
        public IActionResult Export([FromBody] ExportImportModel m)
        {
            var l = _mapper.ExportImport(m);
            try
            {
                _service.Export((Product)l[0], (int)l[1], (int)l[2], (int)l[3]);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return Ok();
        }

        [Authorize(Policy = "Store_Worker")]
        [HttpPut("import")]
        public IActionResult Import([FromBody] ExportImportModel m)
        {
            var l = _mapper.ExportImport(m);
            try
            {
                _service.Import((Product)l[0], (int)l[1], (int)l[2], (int)l[3]);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return Ok();
        }
    }
}
