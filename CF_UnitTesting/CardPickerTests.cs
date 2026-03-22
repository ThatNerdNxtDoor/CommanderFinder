using System.Threading.Tasks;
using CF_Console.Models;
using Microsoft.VisualBasic;
namespace CF_UnitTesting;

//Note to self: for adding library depedency, use 'dotnet add ./CF_UnitTesting/CF_UnitTesting.csproj reference ./{Target project}'

public class UnitTest1
{
    [Theory]
    [InlineData(null)]
    public async Task CardPickerTest(Card arg)
    {
        Card commander = await CommanderFinderConsole.RetrieveCard(arg);

        Assert.True(commander != null);
    }
}
