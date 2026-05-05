import { useState, useEffect } from 'react'
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
      results.push(<li key={index}>
        <h2>{deck.commander.name}</h2>
        <p>ID: {deck.id}</p>
        <a href={deck.commander.scryfall_uri}><img src={deck.commander.image_uris.small} alt={deck.commander.name}></img></a>
        <div>({deck.commander.power}/{deck.commander.toughness}) - {deck.commander.type_line}</div>
        <ul className='card_list'>
          {deck.cards.map((card, i) => (
            <div className = "container">
            <a href={card.scryfall_uri}><img src={card.image_uris.small}></img></a>
            <h3>{card.name}</h3>
            <p>{card.mana_cost} - {card.type_line}</p>
            {card.type_line.includes("Creature") ? <p>({card.power}/{card.toughness})</p> : ""}
            <p className = "rules_text">{card.oracle_text}</p>
            </div>
        ))}</ul>
        <hr></hr>
        </li>
      );})
    return(
      <div className = "container">
        <h1 className = "plain_text">Deck Collection:</h1>
        <hr></hr>
        <ul>
          {results}
        </ul>
      </div>
    );
  }
}

export default App
