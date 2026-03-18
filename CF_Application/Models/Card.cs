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
    public string RulesText{get;}
}