import React from 'react';
import Card from './card.jsx'

class CardDialog extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            card: new Card(),
            message: "",
        }
    }

    setDialogContent(card, message) {
        document.getElementById("confirmCardDialogMessage").innerHTML = message;
        
    }

    render() {
        return(
            <dialog id = "confirmCardDialog" className = "cardAddition" open = {false}>
                <div id = "confirmCardDialogMessage">
                    {this.message}
                </div>
                <Card name = {this.card.name} mana_cost = {this.card.mana_cost} color_identity = {this.card.color_identity} type_line = {this.card.type_line}
                        power = {this.card.power} toughness = {this.card.toughness} oracle_text = {this.card.oracle_text} oracle_id = {this.card.oracle_id}
                        scryfall_uri = {this.card.scryfall_uri} image_uris = {this.card.image_uris}/>
                <button id = "confirmCardDialogAddition">Yes</button>
                <button onClick = {() => {
                    document.getElementById("confirmCard").close();
                }}>No</button>
            </dialog>
        );
    }
}

export default CardDialog