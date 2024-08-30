using HangTab.Models;

namespace HangTab.Tests.TestData;
public abstract class TestData
{
    public class BowlerTestData : TheoryData<Bowler, bool>
    {
        public BowlerTestData()
        {
            Add(new Bowler
            {
                FirstName = "Jason",
                LastName = "Dubar"
            }, true);
            Add(new Bowler
            {
                FirstName = "Donnie",
                LastName = "George"
            }, false);
        }
    }
}
