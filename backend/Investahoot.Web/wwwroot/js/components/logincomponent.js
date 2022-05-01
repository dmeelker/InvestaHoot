import * as Game from "../game.js";

class LoginPanel extends HTMLElement {
    constructor() {
        super();

        this.innerHTML = `
            <game-logo></game-logo>
            <div class="bg-white rounded padding-m center-vertical shadow" style="max-width: 10em;">
                <input type="text" id="nameField" placeholder="What's your name?" /><br />
                <button id="loginButton" class="button-main margin-top-m">Play!</button>
            </div>
`;

        this.loginButton = this.querySelector('button');
        this.username = this.getAttribute('username');

        this.loginButton.addEventListener('click', (e) => {
            this.loginButton.disabled = true;
            Game.joinGame(this.username);
        });
    }

    set username(value) {
        this.querySelector('input').value = value;
    }

    get username() {
        return this.querySelector('input').value;
    }

    reset() {
        this.loginButton.disabled = false;
    }
}

customElements.define('login-panel', LoginPanel);