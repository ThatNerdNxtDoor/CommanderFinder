import { useState, useEffect, use } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from './assets/vite.svg'
import heroImg from './assets/hero.png'
import './App.css'
import Collection from './Models/collection.jsx'
import Card from './Models/card.jsx'

function App() {
  const [data, setData] = useState(null);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(true);
  
  //Create a new collection to add to the database
  const createCollection = async () => {
    try {

    } catch(error) {
      alert(error.message)
    }
  }

  //Add card to collection.
  const addCard = async (deck) => {
    try {
      let card = await generateCard(deck == null ? null : deck.commander);
      
    } catch(error) {
      alert(error.message)
    }
  }

  //Function for generating a card from Scryfall's API
  const generateCard = async (commander) => {
    try {
      var argument = (commander == null) ? "is%3Acommander" : "identity<%3D" + commander.ColorsToString(); 
      const response = await fetch("https://api.scryfall.com/cards/random?q=" + argument);
      let card = JSON.parse(JSON.stringify(response.json(), null, 2));

      return card;
    } catch(error) {
        alert(error.message);
    }
  }

  //-------------------------Web Render; Only used once in the jsx-------------------------
  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await fetch("https://localhost:7277/api/CFCollection");
        
        if (!response.ok) {
          throw new Error(`HTTP Error: Response Code ${response.status}`);
        }

        const result = await response.json();
        setData(result);
      } catch (error) {
        setError(error.message);
      } finally {
        setLoading(false);
      }
    }
    fetchData();
  }, []);

  if (loading) {
    return(<div>Loading...</div>)
  } else if (error) {
    return(<div>Error: {error}</div>)
  } else if (data) {
    let decks = JSON.parse(JSON.stringify(data, null, 2));
    var results = [];
    decks.forEach((deck, index) => {
      results.push(
        <li key={index}>
          <Collection id = {deck.id} commander = {deck.commander} cards = {deck.cards} />
          <hr></hr>
        </li>
      );})
    return(
      <div className = "container">
        <h1 className = "plain_text">Deck Collection:</h1>
        <hr></hr>
        <div className='new_card'></div>
        <hr></hr>
        <ul>
          {results}
        </ul>
      </div>
    );
  }
}

/*<h2>{deck.commander.name}</h2>
          <p>ID: {deck.id}</p>
          <a href={deck.commander.scryfall_uri}><img src={deck.commander.image_uris.small} alt={deck.commander.name}></img></a>
          <div>({deck.commander.power}/{deck.commander.toughness}) - {deck.commander.type_line}</div>
          <ul className='card_list'>
            {deck.cards.map((card, i) => (
              <li className="card_item" key = {i}>
                <div className = "container">
                  <Card name = {card.name} mana_cost = {card.mana_cost} color_identity = {card.color_identity} type_line = {card.type_line}
                  power = {card.power} toughness = {card.toughness} oracle_text = {card.oracle_text} oracle_id = {card.oracle_id}
                  scryfall_uri = {card.scryfall_uri} image_uris = {card.image_uris}/>;
                  <button className="remove" onClick = {() => deleteCard(card, deck)}>Remove From Collection</button>
                </div>
              </li>
          ))}</ul>*/

/*<Card name = {card.name} mana_cost = {card.mana_cost} color_identity = {card.color_identity} type_line = {card.type_line}
                  power = {card.power} toughness = {card.toughness} oracle_text = {card.oracle_text} oracle_id = {card.oracle_id}
                  scryfall_uri = {card.scryfall_uri} image_uris = {card.image_uris}/>*/

/*function deleteCard(card, deck) {
  const [error, setError] = useState(null);
  const [message, setMessage] = useState(null);

  useEffect(() => {
    const deleteData = async () => {
      try {
        const new_deck = deck.filter(item => {
          item.id != card.id
        });
        // Note: Request options allow you to do different HTTP requests instead of using different functions
        const requestOptions = {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(new_deck)
        };
        const response = await fetch("https://localhost:7277/api/CFCollection/" + deck.id);
            
        if (!response.ok) {
            throw new Error(`HTTP Error: Response Code ${response.status}`);
        }
        setMessage("Card Successfully Removed")
      } catch (error) {
        setError(error.message);
      }
    }
    deleteData();
  }, []);
  alert(message);
}*/

export default App
