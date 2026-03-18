namespace CF_API.Controllers;

public class Collection
{
    public int Id {get; set;}
    public Card Commander {get; set;}
    public List<Card> Cards {get; set;}
}