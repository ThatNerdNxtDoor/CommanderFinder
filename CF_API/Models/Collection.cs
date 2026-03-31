namespace CF_API.Models;

public class CFCollection
{
    private int Id {get; set;}
    private Card Commander {get; set;}
    private List<Card> Cards {get; set;}

    public int id {get => Id; set => Id = value;} //ID of the Collection
    public Card commander {get => Commander; set => Commander = value;} //The commander of the collection, all cards are picked based on their color identity
    public List<Card> cards {get => Cards; set => Cards = value;} //The cards saved to the collection.
}