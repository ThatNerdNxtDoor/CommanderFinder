using System.Threading.Tasks;
using CF_Console.Models;
using Microsoft.VisualBasic;
namespace CF_UnitTesting;

//Note to self: for adding library depedency, use 'dotnet add ./CF_UnitTesting/CF_UnitTesting.csproj reference ./{Target project}'

public class CardPickerAPI
{
    [Fact]
    public async Task CommanderPickerAPICallTest()
    {
        Card commander = await CommanderFinderConsole.RetrieveCard(null);
        Assert.True(commander != null, $"{commander}");
    }

    [Fact]
    public async Task CardPickerAPICallTest()
    {
        Card commander = await CommanderFinderConsole.RetrieveCard(null);
        Card new_card = await CommanderFinderConsole.RetrieveCard(commander);
        Assert.True(new_card != null, $"Commander: {commander} \nColorIdentity: {commander.ColorsToString()} \nCard: {new_card}");
    }
}
