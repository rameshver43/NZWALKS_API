using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    // https://localhost:1234/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _context;

        public RegionsController(NZWalksDbContext dbContext)
        {
            _context = dbContext;
        }
        // GET ALL REGION
        //GET: https://localhost:1234/api/regions
        [HttpGet]
        public IActionResult GetAll()
        {
            // Get Data from Database - Domain models

            var regionsDomain = _context.Regions.ToList();

            var regionsDto = new List<RegionDto>();

            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl,
                });
            }
            // Map Domain models to DTOs
            // return DTO
            return Ok(regionsDto);
        }

        // GET SINGLE REGION(REGION BY ID)
        // GET: https://localhost:1234/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            //var region = _context.Regions.Find(id);

            // GET Region Domain Model From Databases
            var region = _context.Regions.FirstOrDefault(x => x.Id == id);
            if(region == null)
            {
                return NotFound();
            }

            // map region domain model to region dto

            var regionDto = new RegionDto()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl,
            };
            return Ok(regionDto);
        }

        // POST To create new Region
        // POST: https://localhost:1234/api/regions/

        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Map or Covert DTO to Domain model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl,

            };
            // use Domain Model to create region
            _context.Regions.Add(regionDomainModel);
            _context.SaveChanges();

            // map domain model back to dto;
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };
            return CreatedAtAction(nameof(GetById),new {id = regionDto.Id}, regionDto);
        }

        // Update a Region
        // PUT: https://localhost:1234/api/regions/{id}

        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // check if region exists
            var regionDomainModel  = _context.Regions.FirstOrDefault(x => x.Id == id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            // Region Found

            // map dto to domain model

            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            _context.SaveChanges();

            // convert domain model to dto

            var regionDto = new RegionDto { Id = regionDomainModel.Id, Code = regionDomainModel.Code, Name = regionDomainModel.Name, RegionImageUrl = regionDomainModel.RegionImageUrl };

            return Ok(regionDto);


        }


        // Delete a Region
        // DELETE: https://localhost:1234/api/regions/{id}

        [HttpDelete]
        [Route("{id:Guid}")]

        public IActionResult Delete([FromRoute] Guid id) {
        
            // check if this region exists

            var regionDomainModel = _context.Regions.FirstOrDefault(x=>x.Id == id);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            _context.Regions.Remove(regionDomainModel);
            _context.SaveChanges();
            // return deleted Region back
            // map domain model to dto first

            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return Ok(regionDto);
        }


    }
}
