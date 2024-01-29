using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks2.API.CustomeActionFilter;
using NZWalks2.API.Models.Domain;
using NZWalks2.API.Models.DTO;
using NZWalks2.API.Repository;

namespace NZWalks2.API.Controllers
{
    //api/walks
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        //Create Walk
        //Post: /api/walk
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            // Map to Domain Model
                var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

                await walkRepository.CreateAsync(walkDomainModel);
                // Map Domain Model to Dto

                return Ok(mapper.Map<WalkDto>(walkDomainModel));
           
        }                                                                                                                                                                                                                                                                           
        //Get Walks
        // Get: /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscendind=true
        [HttpGet]

        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
           var walksDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true,
               pageNumber, pageSize);

            // Map Domain Model to Dto
            return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
        }

        // Get Walk By Id
        // Get: /api/Walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);

            if(walkDomainModel == null)
            {
                return NotFound();
            }

            // Map Dpmain Model to Dto
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        // Update Walk
        // Update: /api/Walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {

            
                // Map Dto to Domain model
                var walkDomainDomain = mapper.Map<Walk>(updateWalkRequestDto);

                walkDomainDomain = await walkRepository.UpdateAsync(id, walkDomainDomain);

                if (walkDomainDomain == null)
                {
                    return NotFound();
                }

                // Map Domain Model to Dto
                return Ok(mapper.Map<WalkDto>(walkDomainDomain));
            
        }

        // Delete Walk
        // Delete: /api/Walk/{id}
        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleteWalkDomainModel = await walkRepository.DeleteAsync(id);

            if (deleteWalkDomainModel == null)
            {
                return NotFound();
            }

            //Map Domain model to Dto
            return Ok(mapper.Map<WalkDto>(deleteWalkDomainModel));

        }
    }
}
