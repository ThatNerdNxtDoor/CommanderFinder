import React, { useState } from 'react';

class Card extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            oracle_id: props.oracle_id,
            name: props.name,
            color_identity: props.color_identity,
            type_line: props.type_line,
            mana_cost: props.mana_cost,
            power: props.power,
            toughness: props.toughness,
            oracle_text: props.oracle_text,
            scryfall_uri: props.scryfall_uri,
            image_uris: props.image_uris
        };
    }
    

    render(props) { //Rendering for a single card. This essentially defines the non-static functions and variables look like.
        //console.log(this.props)
        return(
        <div>
            <a href={this.props.scryfall_uri}><img src={this.props.image_uris.small}></img></a>
            <h3>{this.props.name}</h3>
            <p>{this.props.mana_cost} - {this.props.type_line}</p>
            {this.props.type_line.includes("Creature") ? <p>({this.props.power}/{this.props.toughness})</p> : ""}
            <p className = "rules_text">{this.props.oracle_text}</p>
        </div>)
    }

    static ColorsToString(commander) {
        var colors;
        if (commander.length == 0) //If no colors are in the color identity, the card is colorless.
        {
            colors = "C";
        } else
        {
            colors = commander.color_identity.join("");
        }
        return colors;
    }

    static parseToCard(cardData) { //Custom parsing method to take excess data from Scryfall API's JSON response and return a digestible Card object.
        let card = JSON.parse(cardData);
        console.log(card);
        return new Card({
            oracle_id: card.oracle_id,
            name: card.name,
            color_identity: card.color_identity,
            type_line: card.type_line,
            mana_cost: card.mana_cost,
            power: card.power,
            toughness: card.toughness,
            oracle_text: card.oracle_text,
            scryfall_uri: card.scryfall_uri,
            image_uris: {
                small: card.image_uris.small,
                normal: card.image_uris.normal,
                large: card.image_uris.large,
                png: card.image_uris.png
            }})
    }
}

export default Card