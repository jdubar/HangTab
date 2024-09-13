using HangTab.Data;
using HangTab.Services.Impl;

namespace HangTab.Tests.DatabaseServiceTests;
public class TestBase
{
    protected IDatabaseContext ContextFake { get; }
    protected DatabaseService DatabaseService { get; }

    protected TestBase()
    {
        ContextFake = A.Fake<IDatabaseContext>();
        DatabaseService = new DatabaseService(ContextFake);
    }
}
