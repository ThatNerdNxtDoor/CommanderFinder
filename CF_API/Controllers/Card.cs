using Microsoft.AspNetCore.SignalR;

namespace CF_API.Controllers;

public record Card
{
    public int Id {get;}
    public string Name {get;}
    public string[] Color {get;}
    public string ManaCost {get;}
    public string Power {get;}
    public string Toughness {get;}
    public string RulesText{get;}
}