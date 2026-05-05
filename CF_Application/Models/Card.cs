using System.ComponentModel;
using System.Drawing;

namespace CF_Console.Models;

public record Card
{
    public Card() //Constructor that allows cards to be extracted from the Scryfall API
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
    public string toughness {get => Toughness; set => Toughness = value;} //The health of the card.
    public string oracle_text {get => OracleText; set => OracleText = value;} //The rules text of the card (what the card does).
    public string scryfall_uri {get => URI; set => URI = value;} //The URI for the card on Scryfall.
    public PictureURIs image_uris {get => Images; set => Images = value;} //The URIs for the card images

    public string ColorsToString() //Returns the card's colors as a single uninterrupted string
    {
        string colors;
        if (color_identity.Capacity == 0) //If no colors are in the color identity, the card is colorless.
        {
            colors = "C";
        } else
        {
            colors = string.Join(",", ColorIdentity.ToArray());
        }
        return colors;
    }

    public void PrintCard() //Returns an ascii-fied print of the card
    {
        int cardWidth = 60;
        Console.WriteLine($"+{new string('-', cardWidth)}+"); //Top of card + nameplate
        
        //Card name + Mana Cost
        int spaceLength = cardWidth - 2 - mana_cost.Length;
        Console.WriteLine($"| {name.PadRight(spaceLength)}{mana_cost} |");

        Console.WriteLine($"+{new string('-', cardWidth)}+"); //Bottom of nameplate
        
        //Scryfall URL where the card art is normally
        spaceLength = cardWidth - 2;
        Console.WriteLine($"| {"https://scryfall.com/card/".PadRight(spaceLength)} |");
        Console.WriteLine($"| {scryfall_uri.Substring(26).PadRight(spaceLength)} |");

        Console.WriteLine($"+{new string('-', cardWidth)}+"); //Top of typeplate
        
        //Card type
        spaceLength = cardWidth - 2;
        Console.WriteLine($"| {type_line.PadRight(spaceLength)} |");

        Console.WriteLine($"+{new string('-', cardWidth)}+"); //Bottom of typeplate + top of body
        
        //Body of card

        //Rules Text
        spaceLength = cardWidth - 2;
        string[] words = oracle_text.Replace("\n", " \n ").Split(" ");
        string textWrap = "";
        foreach (string word in words)
        {
            if (word != "\n") {
                if ((textWrap + word).Length < spaceLength) //Add word to temp string
                {
                    textWrap += word + " ";
                } else //Print temp string as a line and start a new temp strng
                {
                    Console.WriteLine($"| {textWrap.PadRight(spaceLength)} |");
                    textWrap = word + " ";
                }
            } else
            {
                Console.WriteLine($"| {textWrap.PadRight(spaceLength)} |");
                Console.WriteLine($"| {"".PadRight(spaceLength)} |");
                textWrap = "";
            }
        }
        Console.WriteLine($"| {textWrap.PadRight(spaceLength)} |"); //Print rest of temp string

        //Statblock
        if (type_line.Contains("Creature"))
        {
            string statBlock = $"[{power}/{toughness}]";
            spaceLength = cardWidth;
            Console.WriteLine($"|{statBlock.PadLeft(spaceLength)}|");
        }

        Console.WriteLine($"+{new string('-', cardWidth)}+"); //Bottom of card
    }
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