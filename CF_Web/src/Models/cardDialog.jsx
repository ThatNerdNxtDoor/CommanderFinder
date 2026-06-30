import React from 'react';
import Card from './card.jsx'

class CardDialog extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            windowContent: "Placeholder Content"
        }
    }

    setDialogContent(card, message, deckID) { //Set the window's content using the message, card, and deck id (new deck is -1)
        this.setState({windowContent: (<div>
                    <div id = "confirmCardDialogMessage">
                        {message}
                    </div>
                    <Card name = {card.state.name} mana_cost = {card.state.mana_cost} color_identity = {card.state.color_identity} type_line = {card.state.type_line}
                            power = {card.state.power} toughness = {card.state.toughness} oracle_text = {card.state.oracle_text} oracle_id = {card.state.oracle_id}
                            scryfall_uri = {card.state.scryfall_uri} image_uris = {card.state.image_uris}/>
                    <button id = "confirmCardDialogAddition" onClick = {() => this.confirmAddition(card.state, deckID)}>Yes</button>
                    <button onClick = {() => {
                        document.getElementById("confirmCardDialog").close();
                    }}>No</button>
                </div>
                )
            });
        
    }

    async confirmAddition(card, deck) {//Posts or updates the API depending on if a deck has been assigned.
        console.log("This all looks good");
        try{
            if (deck == null) { //No deck assigned: Create new collection
                let cardJSON = JSON.stringify(card);
                const requestOptions = {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: cardJSON,
                };
                const response = await fetch("https://localhost:7277/api/CFCollection", requestOptions);
                            
                if (!response.ok) {
                    throw new Error(`HTTP Error: Response Code ${response.status}`);
                }
                alert("Collection Created Successfully");
            } else { //Deck assigned, updated collection
                deck.cards.push(card);
                //Create and fetch PUT request
                let deckJSON = JSON.stringify(deck)
                const requestOptions = {
                    method: 'PUT',
                    headers: { 'Content-Type': 'application/json' },
                    body: deckJSON,
                };
                const response = await fetch("https://localhost:7277/api/CFCollection/" + deck.id, requestOptions);
                            
                if (!response.ok) {
                    throw new Error(`HTTP Error: Response Code ${response.status}`);
                }
                alert("Card Added Successfully");
            }
            window.location.reload(); //Reload page to show changes
        } catch(error) {
            alert(error);
        }
        document.getElementById("confirmCardDialog").close();
    }

    render() { //Render function for the card dialog confirmation.
        return(
            <dialog id = "confirmCardDialog" className = "cardAddition" open = {false}>
                {this.state.windowContent}
            </dialog>
        );
    }
}

export default CardDialog