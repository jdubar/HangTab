using HangTab.Models;

namespace HangTab.Services;
public interface IPersonService
{
    Task<Result> AddAsync(Person person);
    Task<Result> DeleteAsync(int id);
    Task<Result<IEnumerable<Person>>> GetAllAsync();
    Task<Result<Person>> GetByIdAsync(int id);
    Task<Result<IEnumerable<Person>>> GetRegularsAsync();
    Task<Result<IEnumerable<Person>>> GetSubstitutesAsync();
    Task<Result> UpdateAsync(Person person);
}
