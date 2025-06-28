using HangTab.Data;
using HangTab.Models;

namespace HangTab.Repositories.Impl;
public class PersonRepository(IDatabaseContext context) : IPersonRepository
{
    public Task<Person> GetPersonById(int id) => context.GetItemByIdAsync<Person>(id);
    public Task<IEnumerable<Person>> GetAllPeople() => context.GetAllAsync<Person>();
    public Task<IEnumerable<Person>> GetRegulars() => context.GetFilteredAsync<Person>(b => !b.IsSub);
    public Task<IEnumerable<Person>> GetSubstitutes() => context.GetFilteredAsync<Person>(b => b.IsSub);
    public Task<bool> AddPerson(Person person) => context.AddItemAsync(person);
    public Task<bool> DeletePerson(int id) => context.DeleteItemByIdAsync<Person>(id);
    public Task<bool> UpdatePerson(Person person) => context.UpdateItemAsync(person);
}
