import * as Dom from '../dom.js';

class ScoresPanel extends HTMLElement {
    constructor() {
        super();

        this.innerHTML = `
            <game-logo></game-logo>
            <h2>Scores</h2>
            <div>That's all folks!</div>
            <ol class="playerScoresPanel">
            </ol>
`;
        this.playerScoresPanel = this.querySelector('.playerScoresPanel');
    }

    update(scores) {
        Dom.clear(this.playerScoresPanel);

        for (let player of scores) {
            let element = document.createElement('li');
            element.innerText = `${player.Score} - ${player.Name}`;
            this.playerScoresPanel.appendChild(element);
        }
    }
}

customElements.define('scores-panel', ScoresPanel);