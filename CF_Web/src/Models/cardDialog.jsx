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
                    <Card name = {card.name} mana_cost = {card.mana_cost} color_identity = {card.color_identity} type_line = {card.type_line}
                            power = {card.power} toughness = {card.toughness} oracle_text = {card.oracle_text} oracle_id = {card.oracle_id}
                            scryfall_uri = {card.scryfall_uri} image_uris = {card.image_uris}/>
                    <button id = "confirmCardDialogAddition" onClick = {() => this.confirmAddition(card, deckID)}>Yes</button>
                    <button onClick = {() => {
                        document.getElementById("confirmCardDialog").close();
                    }}>No</button>
                </div>
                )
            });
        
    }

    confirmAddition(card, deckID) {//Posts or updates the API depending on if a deck has been assigned.
        console.log("This all looks good")
        try{
            if (deckID == -1) { //No deck assigned: Create new collection

            } else { //Deck id assigned, updated collection

            }
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