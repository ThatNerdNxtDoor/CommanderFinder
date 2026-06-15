import React from 'react';
import Card from './card.jsx'

class Collection extends React.Component {
    constructor() {
        super();
        this.state = {id: 0,
            commander: new Card(),
            cards: new Array(new Card()),
        }
    }

    render() { //NOTE: Functions that are connected to a button need to be within the same scope as the where the button is rendered.
        //Function to remove a card from a collection
        const deleteCard = async (card, deck) => {
            try {
                console.log(card);
                console.log(deck);
                console.log(Object.values(deck.cards));
                const new_cards = deck.cards.filter(item => item.oracle_id != card.oracle_id);
                console.log(new_cards);
                deck.cards = new_cards;
                console.log(deck);
                // Note: Request options allow you to do different HTTP requests instead of using different functions
                const requestOptions = {
                    method: 'PUT',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(deck)
                };
                console.log("https://localhost:7277/api/CFCollection/" + deck.id);
                const response = await fetch("https://localhost:7277/api/CFCollection/" + deck.id, requestOptions);
                
                if (!response.ok) {
                    throw new Error(`HTTP Error: Response Code ${response.status}`);
                }
                    alert("Card Removed Successfully");
            } catch (error) {
                alert(error.message);
            }
        }

        return(<div>
            <h2>{this.props.commander.name}</h2>
            <p>ID: {this.props.id}</p>
            <a href={this.props.commander.scryfall_uri}><img src={this.props.commander.image_uris.small} alt={this.props.commander.name}></img></a>
            <div>({this.props.commander.power}/{this.props.commander.toughness}) - {this.props.commander.type_line}</div>
            <ul className='card_list'>
                {this.props.cards.map((card, i) => (
                <li className="card_item" key = {i}>
                    <div className = "container">
                        <Card name = {card.name} mana_cost = {card.mana_cost} color_identity = {card.color_identity} type_line = {card.type_line}
                        power = {card.power} toughness = {card.toughness} oracle_text = {card.oracle_text} oracle_id = {card.oracle_id}
                        scryfall_uri = {card.scryfall_uri} image_uris = {card.image_uris}/>;
                    <button className="removeCard" onClick = {() => deleteCard(card, {id: this.props.id, commander: this.props.commander, cards: this.props.cards})}>Remove From Collection</button>
                    </div>
                </li>
            ))}</ul>
        </div>)
    }

}

export default Collection