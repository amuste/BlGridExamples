using System.Collections.Generic;

namespace BlGrid.Api.Infrastructure
{
    public class PersonSearchResult
    {
        public List<Person> Persons { get; set; } = new List<Person>();

        public int TotalItems { get; set; }

    }
}
