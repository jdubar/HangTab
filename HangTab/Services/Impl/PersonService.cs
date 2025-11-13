using HangTab.Models;
using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class PersonService(IBaseRepository<Person> repo) : IPersonService
{
    public async Task<Result> AddAsync(Person person)
    {
        if (person is null)
        {
            return Result.Fail("Person cannot be null.");
        }

        return await repo.AddAsync(person)
            ? Result.Ok()
            : Result.Fail("Failed to add the person.");
    }

    public async Task<Result> DeleteAsync(int id)
    {
        if (id < 1)
        {
            return Result.Fail("Invalid person ID.");
        }

        var person = await repo.GetByIdAsync(id);
        if (person is null)
        {
            return Result.Fail($"Person with ID {id} was not found.");
        }

        person.IsDeleted = true;

        return await repo.UpdateAsync(person)
            ? Result.Ok()
            : Result.Fail("Failed to delete the person.");
    }

    public async Task<Result<IEnumerable<Person>>> GetAllAsync()
    {
        var people = await repo.GetAllAsync();
        return people is null || !people.Any()
            ? Result.Fail("No people found.")
            : Result.Ok(people);
    }

    public async Task<Result<Person>> GetByIdAsync(int id)
    {
        if (id < 1)
        {
            return Result.Fail("Invalid person ID.");
        }

        var person = await repo.GetByIdAsync(id);
        return person is null
            ? Result.Fail($"Person with ID {id} was not found.")
            : Result.Ok(person);
    }

    public async Task<Result<IEnumerable<Person>>> GetRegularsAsync()
    {
        var people = await repo.GetFilteredAsync(b => !b.IsSub);
        return people is null || !people.Any()
            ? Result.Fail("No regulars found.")
            : Result.Ok(people);
    }

    public async Task<Result<IEnumerable<Person>>> GetSubstitutesAsync()
    {
        var people = await repo.GetFilteredAsync(b => b.IsSub);
        return people is null || !people.Any()
            ? Result.Fail("No substitutes found.")
            : Result.Ok(people);
    }

    public async Task<Result> UpdateAsync(Person person)
    {
        if (person is null)
        {
            return Result.Fail("Person cannot be null.");
        }

        return await repo.UpdateAsync(person)
            ? Result.Ok()
            : Result.Fail("Failed to update the person.");
    }
}
