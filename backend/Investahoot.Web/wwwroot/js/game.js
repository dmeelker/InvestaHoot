"use strict";

import * as Dom from './dom.js';
import { } from './components/logincomponent.js';
import { } from './components/lobbycomponent.js';
import { } from './components/questioncomponent.js';
import { } from './components/roundresultcomponent.js';
import { } from './components/scorescomponent.js';

const LoginState = 'login';
const LobbyState = 'lobby';
const QuestionState = 'question';
const RoundFinishedState = 'roundfinished';
const ScoreState = 'score';

let gameId = '';
let playerId = '';
let state = null;
let eventStream = null;

const loginPanel = document.querySelector("login-panel");
const lobbyPanel = document.querySelector("lobby-panel");
const questionPanel = document.querySelector("question-panel");
const roundResultPanel = document.querySelector("round-result-panel");
const scoresPanel = document.querySelector("scores-panel");

const panelForState = new Map([
    [LoginState, loginPanel],
    [LobbyState, lobbyPanel],
    [QuestionState, questionPanel],
    [RoundFinishedState, roundResultPanel],
    [ScoreState, scoresPanel]
]);

export function joinGame(name) {
    fetch(`/api/join?name=${name}`)
        .then(response => response.json())
        .then(data => {
            gameId = data.gameId;
            playerId = data.playerId;

            updateState(LobbyState);
            listenForEvents();
        })
}

function updateState(newState) {
    if (state == newState)
        return;

    state = newState;
    ensureStatePanelVisible(state);
}

function ensureStatePanelVisible(state) {
    let statePanel = panelForState.get(state);

    Dom.hideAll(panelForState.values());
    Dom.show(statePanel);
}

export function answerQuestion(answerIndex) {
    fetch(`/api/answer?gameId=${gameId}&playerId=${playerId}&answer=${answerIndex}`, {
        method: 'POST'
    });
}

function listenForEvents() {
    stopListeningForEvents();
    eventStream = new EventSource(`/api/events?gameId=${gameId}&playerId=${playerId}`);

    eventStream.addEventListener("open", function (event) {
        console.log("Connected");
    }, false);

    eventStream.addEventListener("error", function (event) {
        console.log(event);
    }, false);

    eventStream.addEventListener("message", function (event) {
        console.log(event.data);

        const gameState = JSON.parse(event.data);
        processGameStateUpdate(gameState);
    }, false);
}

function processGameStateUpdate(event) {
    if (event.State == 'Lobby') {
        lobbyPanel.players = event.Players;
        updateState(LobbyState);
    } else if (event.State == 'Question') {
        questionPanel.update(event);
        updateState(QuestionState);
    } else if (event.State == 'RoundFinished') {
        roundResultPanel.update(event);
        updateState(RoundFinishedState);
    } else if (event.State == 'Score') {
        scoresPanel.update(event.Players);
        updateState(ScoreState);
    } else if (event.State == 'Closed') {
        stopListeningForEvents();
        loginPanel.reset();
        updateState(LoginState);
    }
}

function stopListeningForEvents() {
    if (eventStream) {
        eventStream.close();
        eventStream = null;
    }
}

updateState(LoginState);