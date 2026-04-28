import React from 'react';
import Card from './card.jsx'

class Collection extends React.Component {
    constructor() {
        super();
        this.state = {id: 0,
            commander: new Card(),
            cards: new Card[0],
        }
    }
    render() {
        
    }
}

export default Collection