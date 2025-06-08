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
        private readonly CacheService _cache;
        private static string _cacheKey = "Buildings";

        public BuildingsController(BuildingService r, Mapper m, CacheService cache)
        {
            _service = r;
            _mapper = m;
            _cache = cache;
        }

        [Authorize(Policy = "Owner")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var ownerid = int.Parse(HttpContext.User.FindFirst("Id")?.Value);

            if (!_cache.GetValue(_cacheKey + ownerid, out List<Building> col))
            {
                col = _service.GetBuildings(ownerid);
                _cache.SetValue(_cacheKey + ownerid, col, 2);
            }

            return Ok(new { Buildings = _mapper.GetBuildingModels(col)});
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
            _cache.RemoveValue(_cacheKey + int.Parse(HttpContext.User.FindFirst("Id")?.Value));
            
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
                _cache.RemoveValue(_cacheKey + int.Parse(HttpContext.User.FindFirst("Id")?.Value));
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
                _cache.RemoveValue(_cacheKey + int.Parse(HttpContext.User.FindFirst("Id")?.Value));
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
                _cache.RemoveValue(_cacheKey + int.Parse(HttpContext.User.FindFirst("Id")?.Value));
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
                _cache.RemoveValue(_cacheKey + int.Parse(HttpContext.User.FindFirst("Id")?.Value));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return Ok();
        }
    }
}
