import * as Dom from "../dom.js";

class RoundResultPanel extends HTMLElement {
    constructor() {
        super();

        this.innerHTML = `
            <h2 class="correctAnswerPanel">
                Correct
            </h2>
            <h2 class="incorrectAnswerPanel">
                Incorrect<br/>
                <svg xmlns="http://www.w3.org/2000/svg" width="80" height="80" viewBox="0 0 80 80"><g fill="none" fill-rule="evenodd"><g><g><g><g transform="translate(-257 -1827) translate(90 1581) translate(47 190) translate(120 56)"><circle cx="40" cy="40" r="37.895" fill="#F35" stroke="#FFF" stroke-width="4.211"></circle><g fill="#FFF" fill-rule="nonzero" stroke="#000" stroke-opacity="0.15" stroke-width="2.105"><path d="M39.99 12.621v14.736l14.736.001V39.99H39.99v14.736H27.359V39.99H12.62V27.359h14.736l.001-14.737H39.99z" transform="translate(6.316 6.316) rotate(-135 33.674 33.674)"></path></g></g></g></g></g></g></svg>
            </h2>
            <small>You got <span class="pointsLabel"></span> points. You are now in spot <span class="rankingLabel"></span></small>
`;

        this.correctAnswerPanel = this.querySelector('.correctAnswerPanel');
        this.incorrectAnswerPanel = this.querySelector('.incorrectAnswerPanel');
        this.pointsLabel = this.querySelector('.pointsLabel');
        this.rankingLabel = this.querySelector('.rankingLabel');
    }

    update(state) {
        if (state.CorrectAnswer) {
            Dom.show(this.correctAnswerPanel);
            Dom.hide(this.incorrectAnswerPanel);
        } else {
            Dom.show(this.incorrectAnswerPanel);
            Dom.hide(this.correctAnswerPanel);
        }

        this.pointsLabel.innerText = state.Points;
        this.rankingLabel.innerText = state.CurrentRanking;
    }
}

customElements.define('round-result-panel', RoundResultPanel);