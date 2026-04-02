# `CommanderFinder`
A WebAPI-based application meant give ideas for Magic: the Gathering Commander decks by digging through the card game's bottomless sea of cards to use as a commander, as well as finding cards that could possibly be used alongside said commanders in its color identity.<br>
From the optimal to the obscure, the meta to the mediocre, there is only really one way to find out what you get.<br>
## `Context For Those Unfamiliar`<br>
Magic: the Gathering is one of the most popular trading card games to date, and is played in multiple official and fan-made formats.  `Commander` is an official format where you create a 100 card deck with one of them being a Legendary Creature that is designated as the eponymous Commander. You will always have access to your Commander, and thus the other 99 cards are meant to all be built around this singular card in one way or another. Every card has some number of associated `colors`, whether it is in the cost to play them or in their rules text. This decides a card's `Color Identity`, and all cards in the 99 of the deck must be within the color identity of the Commander.<br>
This program not only finds potential cards to use as a Commander, but also finds cards that are within a Commander's Color Identity to generate ideas on what can be done with a deck.
## `Requirements For Running`<br>
CommanderFinder comes with an API console (```CF_API\bin\Debug\net9.0\CF_API.exe```) and an Application console (```CF_Application\bin\Debug\net9.0\CF_Application.exe```). The Application is intended for user consumption, but the API console must be launched beforehand so that the local port is opened and can be interacted with. <br>
Running the program requires Version 9.0 of the .NET SDK,  Version 9.0.6 of Swashbuckle, and Version 9.0.11 of OpenAPI.<br>
Inputs are mostly shortened down to one or two characters. Any commands that can be inputted are in parinthesis.<br>
For Example:<br>
```
(A)dd a new card to the collection.
(R)emove a card from the collection.
(D)elete the collection.
(E)xit to main menu.
Enter your letter command:
A
```
-------------------------------------------------------------------
## `Capstone Questions`<br>
This project has helped me better understand how API's function and interface with eachother, as well as how they pass their information.<br>
The course has given me a more in-depth understanding of c# than what I had learned at my university.<br>
With more time, I would implement a React client as well to better visualize the cards and make the user's navigation more intuitive.