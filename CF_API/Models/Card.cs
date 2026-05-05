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
        Images = new PictureURIs();
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
    private PictureURIs Images;

    public string oracle_id {get => ID; set => ID = value;} //Scrfall API's ID.
    public string name {get => Name; set => Name = value;} //Card Name.
    public List<string> color_identity {get => ColorIdentity; set => ColorIdentity = value;} //The colors found within its mana cost or rules text (All cards must be within a commander's color identity).
    public string type_line {get => TypeLine; set => TypeLine = value;} //The type line of the card, detailing super, normal, and sub types (ie "Legendary Creature - Dragon Avatar").
    public string mana_cost {get => ManaCost; set => ManaCost = value;} //The mana cost to cast the card.
    public string power {get => Power; set => Power = value;} //The attack of the card.
    public string toughness {get => Toughness; set => Toughness = value;} //The health of the card .
    public string oracle_text {get => OracleText; set => OracleText = value;} //The rules text of the card (what the card does).
    public string scryfall_uri {get => URI; set => URI = value;} //The URI for the card on Scryfall.
    public PictureURIs image_uris {get => Images; set => Images = value;} //The URIs for the card images
}

public record PictureURIs //Companion class for storing the picture URIs of the card
{
    public PictureURIs()
    {
        Small = "";
        Medium = "";
        Large = "";
        PNG = "";
    }

    private string Small;
    private string Medium;
    private string Large;
    private string PNG;

    public string small {get => Small; set => Small = value;} //146 × 204 JPG
    public string normal {get => Medium; set => Medium = value;} //488 × 680 JPG
    public string large {get => Large; set => Large = value;} //672 × 936 JPG
    public string png {get => PNG; set => PNG = value;} //745 × 1040 PNG
}