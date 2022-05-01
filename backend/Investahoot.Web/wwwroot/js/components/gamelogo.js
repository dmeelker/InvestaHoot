class GameLogo extends HTMLElement {
    constructor() {
        super();

        this.innerHTML = `
            <h1>InvestaHOOT</h1>
`;
    }
}

customElements.define('game-logo', GameLogo);