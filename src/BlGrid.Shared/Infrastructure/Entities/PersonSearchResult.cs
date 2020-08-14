using System.Collections.Generic;

namespace BlGrid.Shared.Infrastructure.Entities
{
    public class PersonSearchResult
    {
        public List<Person> Persons { get; set; } = new List<Person>();

        public int TotalItems { get; set; }

    }
}
