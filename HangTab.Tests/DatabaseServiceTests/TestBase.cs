using HangTab.Data;
using HangTab.Data.Impl;
using HangTab.Services.Impl;

namespace HangTab.Tests.DatabaseServiceTests;
public class TestBase
{
    protected IDatabaseContext ContextFake { get; }
    protected DatabaseService DatabaseService { get; }

    protected TestBase()
    {
        ContextFake = A.Fake<IDatabaseContext>();
        DatabaseService = new DatabaseService(A.Fake<BowlerService>(), ContextFake);
    }
}
