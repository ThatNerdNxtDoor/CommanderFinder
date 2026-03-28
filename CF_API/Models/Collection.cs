namespace CF_API.Models;

public class CFCollection
{
    private int Id {get; set;}
    private Card Commander {get; set;}
    private List<Card> Cards {get; set;}

    public int id {get => Id; set => Id = value;}
    public Card commander {get => Commander; set => Commander = value;}
    public List<Card> cards {get => Cards; set => Cards = value;}
}