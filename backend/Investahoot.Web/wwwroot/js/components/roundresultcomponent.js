import * as Dom from "../dom.js";

class RoundResultPanel extends HTMLElement {
    constructor() {
        super();

        this.innerHTML = `
            <h2>Round over!</h2>

            <div>Your answer was</div>
            <div class="correctAnswerPanel" style="animation-name: pulse; animation-duration: 3s; animation-iteration-count: infinite; animation-direction: alternate;">
                CORRECT!
            </div>
            <div class="incorrectAnswerPanel">
                INCORRECT!
            </div>
            You got <span class="pointsLabel"></span> points. You are now in spot <span class="rankingLabel"></span>
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