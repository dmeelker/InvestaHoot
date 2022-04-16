import { List, Box, TextField, Button, ListItemButton, ListItemText } from "@mui/material";
import React, { useEffect, useState } from "react";

// const apiUrl: string = 'https://investahootbackend.azurewebsites.net/api';
const apiUrl: string = 'https://localhost:7280/api';
const axios = require('axios').default;

type EnterRoomProps = {
    fn: (name: string) => void
};

const EnterRoom = ({ fn }: EnterRoomProps) => {
    const [nameFieldValue, setNameFieldValue] = React.useState<string>("");
    return (
        <Box>
            <TextField value={nameFieldValue} id="outlined-basic" label="Player name" variant="outlined" onChange={(e) => setNameFieldValue(e.currentTarget.value)} />
            <Button variant='contained' onClick={(_) => fn(nameFieldValue)}>Submit!</Button>
        </Box >
    );
}



type QuestionAnswerProps = {
    answers: string[]
};
const QuestionAnswers = ({ answers }: QuestionAnswerProps) => {
    return (
        <Box sx={{
            margin: '1rem'
        }} >
            <List>
                {answers.map((answer) => (
                    <ListItemButton>
                        <ListItemText primary={answer} />
                    </ListItemButton>
                ))}
            </List>
        </Box >
    )
}

type RoundProps = {
    roundNumber: number,
    answers: string[],
    timeLeft: number
};

const Round = ({
    roundNumber,
    answers,
    timeLeft
}: RoundProps) => {
    return (
        <Box>
            <h1>Round: {roundNumber}</h1>
            <QuestionAnswers answers={answers} />
        </Box>
    );
}


export default function Game() {
    const [playerName, setUser] = React.useState<string | null>(null);
    const [gameId, setGameId] = React.useState<string | null>(null);
    const [playerId, setPlayerId] = React.useState<string | null>(null);
    const [roundNumber, setRoundNumber] = React.useState<number>(-1);
    const [gameState, setGameState] = React.useState<string>("");
    const [players, setPlayers] = React.useState<string[]>([]);
    const [answers, setAnswers] = React.useState<string[]>([]);
    const [timeLeft, setTimeLeft] = React.useState<number>(0.0);

    const enterFn = async (name: string) => {
        setUser(name);

        let response = await axios.get(apiUrl.concat('/join?name='.concat(name)));

        setGameId(response.data["gameId"]);
        setPlayerId(response.data["playerId"]);

        console.log(response.data["gameId"]);
    };


    useEffect(() => {
        const pollState = async () => {
            if (!gameId)
                return;
            let response = await axios.get(apiUrl.concat('/state?playerId='.concat(playerId!).concat('&gameId=').concat(gameId!)));

            let responseState = response.data.state;

            if (gameState !== responseState) {
                setGameState(responseState);
            }

            if (responseState === 'Lobby') {
                setPlayers(response.data.players);
            }

            if (responseState === 'Question') {
                setAnswers(response.data.answers);
                setTimeLeft(response.data.timeLeft);
            }
        }
        setTimeout(pollState, 1000);
    }, [playerId, gameId, gameState, answers, timeLeft]);


    if (gameId === null) {
        return (
            <EnterRoom fn={enterFn} />
        );
    }

    if (gameState === 'Lobby') {
        return (
            <Box>
                <h1>Wachten op admin, {players.length} wachtende spelers.</h1>
                {players.map((p) => (
                    <h2 key={p}>{p}</h2>
                ))}

            </Box>
        )
    }

    if (gameState === 'Question') {
        return <h1>question!</h1>
    }
    return <div></div>


    // return (
    //     <QuestionAnswers
    //         answers={["hallo 1", "hallo2", "hallo3", "hallo4"]} />

    // );
}