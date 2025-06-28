using HangTab.Models;

namespace HangTab.Repositories;
public interface IPersonRepository
{
    Task<Person> GetPersonById(int id);
    Task<IEnumerable<Person>> GetAllPeople();
    Task<IEnumerable<Person>> GetRegulars();
    Task<IEnumerable<Person>> GetSubstitutes();
    Task<bool> AddPerson(Person person);
    Task<bool> DeletePerson(int id);
    Task<bool> UpdatePerson(Person person);
}
