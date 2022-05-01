import * as Game from "../game.js";
import * as Dom from "../dom.js";

class QuestionPanel extends HTMLElement {
    constructor() {
        super();

        this.innerHTML = `
            <h2>What's your answer?</h2>
            <div class="timePanel"><span class="timeLabel"></span> seconds left</div>
            <div class="answerButtonPanel"></div>
`;

        this.answerPanel = this.querySelector('.answerButtonPanel');
        this.timePanel = this.querySelector('.timePanel');
        this.timeLabel = this.querySelector('.timeLabel');
        this.secondsLeft = 0;
    }

    set answers(answers) {
        Dom.clear(this.answerPanel);
        let index = 0;

        for (let answer of answers) {
            let element = document.createElement('button');
            element.innerText = answer;
            element.value = index;
            element.addEventListener('click', (e) => this.#onAnswerButtonClicked(e.srcElement))
            this.answerPanel.appendChild(element);

            index++;
        }
    }

    connectedCallback() {
        this.intervalId = window.setInterval(() => this.#onTimerUpdate(), 1000);
    }

    disconnectedCallback() {
        window.clearInterval(this.intervalId);
    }

    #onTimerUpdate() {
        if (this.secondsLeft > 0) {
            this.secondsLeft--;
            this.updateTimeLeft();
        }
    }

    #onAnswerButtonClicked(button) {
        let answerIndex = button.value;
        this.disableQuestionButtons();
        Game.answerQuestion(answerIndex);
    }

    update(state) {
        if (state.Answered) {
            Dom.hide(this.answerPanel);
            Dom.hide(this.timePanel);
        } else {
            this.answers = state.Answers;
            this.secondsLeft = state.TimeLeft;
            this.updateTimeLeft();

            Dom.show(this.answerPanel);
            Dom.show(this.timePanel);
            this.enableQuestionButtons();
        }
    }

    updateTimeLeft() {
        if (this.timeLabel.innerText == this.secondsLeft)
            return;

        this.timeLabel.innerText = this.secondsLeft;
        this.pulseTimeLeft();
    }

    pulseTimeLeft() {
        this.timeLabel.classList.add('pulse');
        window.setTimeout(() => this.timeLabel.classList.remove('pulse'), 900);
    }

    enableQuestionButtons() {
        this.querySelectorAll('button')
            .forEach(button => button.disabled = false);
    }

    disableQuestionButtons() {
        this.querySelectorAll('button')
            .forEach(button => button.disabled = true);
    }
}

customElements.define('question-panel', QuestionPanel);