import * as Dom from '../dom.js';

class LobbyPanel extends HTMLElement {
    constructor() {
        super();

        this.innerHTML = `
            <h3>Waiting for the game to start!</h3>

            <h4>Your opponents</h4>
            <div class="lobbyPlayerList"></div>
`;

        this.playerList = this.querySelector('.lobbyPlayerList');
    }

    set players(playerNames) {
        Dom.clear(this.playerList);

        for (let name of playerNames) {
            let element = document.createElement('div');
            element.innerText = name;
            this.playerList.appendChild(element);
        }
    }
}

customElements.define('lobby-panel', LobbyPanel);