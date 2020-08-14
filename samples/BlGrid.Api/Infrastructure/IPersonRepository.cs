using System.Collections.Generic;
using System.Threading.Tasks;
using BlGrid.Api.Infrastructure.QueryHelpers;

namespace BlGrid.Api.Infrastructure
{
    public interface IPersonRepository
    {
        Task<List<Person>> GetPersons();

        Task<List<Person>> SearchPersons(SearchModel searchModel);

        Task<int> CountPersons(SearchModel searchModel);

        Task AddPersons(List<Person> persons);
    }
}
