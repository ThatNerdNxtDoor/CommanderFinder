import React from 'react';

class Card extends React.Component {
    constructor() {
        super();
        this.state = {id: "",
            name: "",
            color_identity: [""],
            type_line: "",
            mana_cost: "",
            power: "",
            toughness: "",
            oracle_text: "",
            uri: "",
            image_uris: ""
        };
    }
}

export default Card