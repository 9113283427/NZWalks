using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks2.API.CustomeActionFilter;
using NZWalks2.API.Data;
using NZWalks2.API.Models.Domain;
using NZWalks2.API.Models.DTO;
using NZWalks2.API.Repository;

namespace NZWalks2.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
             this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        // Get All Regions

        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            // Get Data From DataBase - Domain  models

            var regionsDomain = await regionRepository.GetAllAsync();

            //Return Dto
            return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
        }


        // Get Region By Id (Get Single Region)
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            // var regionDomain = dbContext.Regions.Find(id);

            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                 return NotFound();
            }
            // Return Dto back to client
            return Ok(mapper.Map<RegionDto>(regionDomain));

        }


        // Post to create a new region

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {


            // convert dto to domain model
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

            // Use Domain Model to Create Region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);


            // mAp domain model back to Dto

            var regionsDto = mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = regionsDto.Id }, regionsDto);
        }
        


        // Update region
        // Put: https://localhost:portnumber/api/regions/

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id , [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            
                // map Dto to domain model
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

                // check if region exists
                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }
                // convert domain model to Dto

                return Ok(mapper.Map<RegionDto>(regionDomainModel));
            
        }

        // Delete Region

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer,Reader")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }
    }
}



