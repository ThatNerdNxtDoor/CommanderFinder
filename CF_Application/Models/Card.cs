namespace CF_Console.Models;

public record Card
{
    public string Id {get;}
    public string Name {get;}
    public string[] Color {get;}
    public string[] Types {get;}
    public string ManaCost {get;}
    public string Power {get;}
    public string Toughness {get;}
    public string RulesText {get;}

    public string ColorsToString() //Returns the card's colors as a single uninterrupted string
    {
        string colors = Color.ToString();
        return colors.Replace(",", ""); //Removes "," from the stringified list to have an uninterrupted list of colors (eg "RW")
    }
}