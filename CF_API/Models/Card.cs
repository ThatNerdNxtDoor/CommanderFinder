namespace CF_API.Models;

public record Card
{
    public Card()
    {
        ID = "";
        Name = "";
        ColorIdentity = new List<string>();
        TypeLine = "";
        ManaCost = "";
        Power = "";
        Toughness = "";
        OracleText = "";
        URI = "";
    }

    private string ID;
    private string Name;
    private List<string> ColorIdentity;
    private string TypeLine;
    private string ManaCost;
    private string Power;
    private string Toughness;
    private string OracleText;
    private string URI;

    public string oracle_id {get => ID; set => ID = value;} //Scrfall API's ID.
    public string name {get => Name; set => Name = value;} //Card Name.
    public List<string> color_identity {get => ColorIdentity; set => ColorIdentity = value;} //The colors found within its mana cost or rules text (All cards must be within a commander's color identity).
    public string type_line {get => TypeLine; set => TypeLine = value;} //The type line of the card, detailing super, normal, and sub types (ie "Legendary Creature - Dragon Avatar").
    public string mana_cost {get => ManaCost; set => ManaCost = value;} //The mana cost to cast the card.
    public string power {get => Power; set => Power = value;} //The attack of the card.
    public string toughness {get => Toughness; set => Toughness = value;} //The health of the card .
    public string oracle_text {get => OracleText; set => OracleText = value;} //The rules text of the card (what the card does).
    public string scryfall_uri {get => URI; set => URI = value;} //The URI for the card on Scryfall.
}