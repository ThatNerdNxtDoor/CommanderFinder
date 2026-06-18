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
  
  //Function for generating a card from Scryfall's API
  const generateCard = async (commander) => {
    try {
      var argument = (commander == null) ? "is%3Acommander" : "identity<%3D" + Card.ColorsToString(commander); 
      const response = await fetch("https://api.scryfall.com/cards/random?q=" + argument);
      let card = JSON.parse(JSON.stringify(response.json(), null, 2));

      return card;
    } catch(error) {
        alert(error.message);
    }
  }

  //Create a new collection to add to the database
  const createCollection = async () => {
    try {
      let card = await generateCard(null);
      //Create dialog box that shows the card and asks if they want to make it a commander
    } catch(error) {
      alert(error.message)
    }
  }

  //Add card to collection.
  const addCard = async (deck) => {
    try {
      let card = await generateCard(deck.commander);
      //Create popup that shows the card and asks if they want to make it a commander
    } catch(error) {
      alert(error.message)
    }
  }

  //Function for deleting a collection from the API
  const removeCollection = async (deckToRemove, decks) => {
    try {
      console.log(deckToRemove);
      const new_list = decks.filter(item => item.id != deckToRemove.id);
      console.log(new_list);
      // Note: Request options allow you to do different HTTP requests instead of using different functions
      const requestOptions = {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(new_list)
      };
      console.log("https://localhost:7277/api/CFCollection/" + deckToRemove.id);
      const response = await fetch("https://localhost:7277/api/CFCollection/" + deckToRemove.id, requestOptions);
                
      if (!response.ok) {
        throw new Error(`HTTP Error: Response Code ${response.status}`);
      }
      alert("Collection Removed Successfully");
    } catch (error) {
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
          <button className = "removeCard" onClick = {() => removeCollection(deck, decks)}>Delete Collection</button>
          <hr></hr>
        </li>
      );})
    return(
      <div>
        <div className = "container">
          <h1 className = "plain_text">Deck Collection:</h1>
          <hr></hr>
          <div className='new_card'></div>
          <hr></hr>
          <ul>
            {results}
          </ul>
        </div>
        <div>
          <button onClick = {createCollection}>Create Collection (Generate Commander)</button>
        </div>
        <dialog className = "cardAddition" open = {false}>
          <button>Yes</button>
          <button>No</button>
        </dialog>
      </div>
    );
  }
}

export default App
