import * as Dom from './dom.js';

const LoginState = 'login';
const LobbyState = 'lobby';
const QuestionState = 'question';
const RoundFinishedState = 'roundfinished';
const ScoreState = 'score';

let lobbyPlayerListElement = document.getElementById('lobbyPlayerList');
let answerButtonPanel = document.getElementById('answerButtonPanel');
let playerScoresPanel = document.getElementById('playerScoresPanel');
let timeLabel = document.getElementById('timeLabel');

let gameId = '';
let playerId = '';
let state = null;
let gameState = null;
let eventStream = null;

document.getElementById("loginButton").addEventListener("click", () => {
    let name = document.getElementById("nameField").value;

    fetch(`/api/join?name=${name}`)
        .then(response => response.json())
        .then(data => {
            gameId = data.gameId;
            playerId = data.playerId;

            updateState(LobbyState);
            listenForEvents();
        })
});

function updateState(newState) {
    if (state == newState)
        return;

    state = newState;
    ensureStatePanelVisible(state);
}

function ensureStatePanelVisible(state) {
    let allPanelIds = ['loginPanel', 'lobbyPanel', 'questionPanel', 'roundFinishedPanel', 'scorePanel'];
    let statePanelId = getPanelIdForState(state);

    for (let id of allPanelIds) {
        let element = document.getElementById(id);
        let visible = id == statePanelId;

        Dom.setVisible(element, visible);
    }
}

function getPanelIdForState(state) {
    switch (state) {
        case LoginState:
            return 'loginPanel';
        case LobbyState:
            return 'lobbyPanel';
        case QuestionState:
            return 'questionPanel';
        case RoundFinishedState:
            return 'roundFinishedPanel'
        case ScoreState:
            return 'scorePanel';
        default:
            throw Error('Unknown state: ' + state);
    }
}

function processGameStateUpdate(gameState) {
    if (gameState.State == 'Lobby') {
        updateState(LobbyState);
        showLobbyPlayers(gameState.Players);
    } else if (gameState.State == 'Question') {
        if (gameState.Answered) {
            answerButtonPanel.innerHTML = '';
        } else {
            showAnswerButtons(gameState.Answers);
        }

        timeLabel.innerText = gameState.TimeLeft;
        updateState(QuestionState);
    } else if (gameState.State == 'RoundFinished') {
        showRoundResults(gameState);
        updateState(RoundFinishedState);
    } else if (gameState.State == 'Score') {
        showScores(gameState.Players);
        updateState(ScoreState);
    } else if (gameState.State == 'Closed') {
        stopListeningForEvents();
        updateState(LoginState);
    }
}

function showLobbyPlayers(playerNames) {
    lobbyPlayerListElement.innerHTML = '';

    for (let name of playerNames) {
        let element = document.createElement('div');
        element.innerText = name;
        lobbyPlayerListElement.appendChild(element);
    }
}

function showAnswerButtons(answers) {
    answerButtonPanel.innerHTML = '';
    let index = 0;

    for (let answer of answers) {
        let element = document.createElement('button');
        element.innerText = answer;
        element.value = index;
        element.addEventListener('click', onAnswerButtonClicked)
        answerButtonPanel.appendChild(element);

        index++;
    }
}

function onAnswerButtonClicked(e) {
    let answerIndex = e.srcElement.value;
    fetch(`/api/answer?gameId=${gameId}&playerId=${playerId}&answer=${answerIndex}`, {
        method: 'POST'
    });
}

function showRoundResults(state) {
    const correctAnswerPanel = document.getElementById('correctAnswerPanel');
    const incorrectAnswerPanel = document.getElementById('incorrectAnswerPanel');
    const pointsLabel = document.getElementById('pointsLabel');
    const rankingLabel = document.getElementById('rankingLabel');

    if (state.CorrectAnswer) {
        Dom.show(correctAnswerPanel);
        Dom.hide(incorrectAnswerPanel);
    } else {
        Dom.show(incorrectAnswerPanel);
        Dom.hide(correctAnswerPanel);
    }

    pointsLabel.innerText = state.Points;
    rankingLabel.innerText = state.CurrentRanking;
}

function showScores(scores) {
    playerScoresPanel.innerHTML = '';

    for (let player of scores) {
        let element = document.createElement('li');
        element.innerText = `${player.Score} - ${player.Name}`;
        playerScoresPanel.appendChild(element);
    }
}

function listenForEvents() {
    stopListeningForEvents();
    eventStream = new EventSource(`/api/events?gameId=${gameId}&playerId=${playerId}`);

    eventStream.addEventListener("open", function (event) {
        console.log(event);
        console.log("Connected");
    }, false);

    eventStream.addEventListener("error", function (event) {
        console.log(event);
        console.log("Error");
    }, false);

    eventStream.addEventListener("message", function (event) {
        console.log(event);
        console.log(event.data);

        gameState = JSON.parse(event.data);
        processGameStateUpdate(gameState);
    }, false);
}

function stopListeningForEvents() {
    if (eventStream) {
        eventStream.close();
        eventStream = null;
    }
}

updateState(LoginState);