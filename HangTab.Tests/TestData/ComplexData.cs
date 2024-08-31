using HangTab.Models;

namespace HangTab.Tests.TestData;
public abstract class ComplexData
{
    public class BowlerTheoryData : TheoryData<Bowler, bool>
    {
        public BowlerTheoryData()
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
