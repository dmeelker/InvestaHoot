import * as Dom from '../dom.js';

class LobbyPanel extends HTMLElement {
    constructor() {
        super();

        this.innerHTML = `
            <h3>Waiting for the game to start!</h3>

            <div class="opponentsPanel">
                <strong>Your opponents:</strong>
                <div class="lobbyPlayerList"></div>
            </div>
            <div class="noopponentsPanel">You are the first one here!
            </div>
`;

        this.opponentsPanel = this.querySelector('.opponentsPanel');
        this.noopponentsPanel = this.querySelector('.noopponentsPanel');
        this.playerList = this.querySelector('.lobbyPlayerList');
    }

    set players(playerNames) {
        if (playerNames.length == 0) {
            Dom.show(this.noopponentsPanel);
            Dom.hide(this.opponentsPanel);
        } else {
            Dom.clear(this.playerList);
            for (let name of playerNames) {
                let element = document.createElement('div');
                element.innerText = name;
                this.playerList.appendChild(element);
            }

            Dom.hide(this.noopponentsPanel);
            Dom.show(this.opponentsPanel);
        }
    }
}

customElements.define('lobby-panel', LobbyPanel);