using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlGrid.Api.Infrastructure;
using BlGrid.Api.Infrastructure.QueryHelpers;
using Microsoft.AspNetCore.Mvc;

namespace BlGrid.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]/[action]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonRepository _personRepository;

        public PersonController(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetPersons()
        {
            var persons = await _personRepository.GetPersons();

            return Ok(persons);
        }

        [HttpPost]
        public async Task<IActionResult> SearchPersons([FromBody] SearchModel searchModel)
        {
            var persons = await _personRepository.SearchPersons(searchModel);

            var count = await _personRepository.CountPersons(searchModel);

            return Ok(new PersonSearchResult { Persons = persons, TotalItems = count});
        }

        [HttpPost]
        public async Task<IActionResult> AddPersons([FromBody] List<Person> persons)
        {
            await _personRepository.AddPersons(persons);

            return Ok();
        }
    }
}
