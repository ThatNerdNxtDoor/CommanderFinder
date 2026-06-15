import React, { useState } from 'react';

class Card extends React.Component {
    constructor() {
        super();
        this.state = {oracle_id: "",
            name: "",
            color_identity: [],
            type_line: "",
            mana_cost: "",
            power: "",
            toughness: "",
            oracle_text: "",
            scryfall_uri: "",
            image_uris: {small: "", normal: "", large: "", png: ""}
        };
    }
    

    render() { //Rendering for a single card.
        return(<div>
                    <a href={this.props.scryfall_uri}><img src={this.props.image_uris.small}></img></a>
                    <h3>{this.props.name}</h3>
                    <p>{this.props.mana_cost} - {this.props.type_line}</p>
                    {this.props.type_line.includes("Creature") ? <p>({this.props.power}/{this.props.toughness})</p> : ""}
                    <p className = "rules_text">{this.props.oracle_text}</p>
                </div>)
    }

    ColorsToString() {
        var colors;
        if (color_identity.length == 0) //If no colors are in the color identity, the card is colorless.
        {
            colors = "C";
        } else
        {
            colors = colors.join("");
        }
        return colors;
    }
}

export default Card