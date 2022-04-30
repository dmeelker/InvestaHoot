import * as Game from "../game.js";

class LoginPanel extends HTMLElement {
    constructor() {
        super();

        this.innerHTML = `
            <input type="text" id="nameField" placeholder="What's your name?" /><br />
            <button id="loginButton">Play!</button>
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