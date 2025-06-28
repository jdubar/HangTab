using HangTab.Models;
using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class PersonService(IPersonRepository personRepository) : IPersonService
{
    public Task<Person> GetPersonById(int id) => personRepository.GetPersonById(id);
    public Task<IEnumerable<Person>> GetAllPeople() => personRepository.GetAllPeople();
    public Task<IEnumerable<Person>> GetRegulars() => personRepository.GetRegulars();
    public Task<IEnumerable<Person>> GetSubstitutes() => personRepository.GetSubstitutes();
    public Task<bool> AddPerson(Person person) => personRepository.AddPerson(person);
    public Task<bool> DeletePerson(int id) => personRepository.DeletePerson(id);
    public Task<bool> UpdatePerson(Person person) => personRepository.UpdatePerson(person);
}
