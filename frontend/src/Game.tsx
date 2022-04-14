import { List, Box, TextField, Button, ListItemButton, ListItemText } from "@mui/material";
import React, { useState } from "react";

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
        <Box sx={{ margin: '1rem' }} >
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

// const Round = ({
//     roundNumber: number,
//     answers: string[],
//     timeleft
//});

export default function Game() {

    const [playerName, setUser] = React.useState<string | null>(null);

    const enterFn = async (name: string) => {
        setUser(name);

        await axios.get(apiUrl.concat('/join?name='.concat(name)));
    };

    if (playerName !== null) {

    }

    return (
        <EnterRoom fn={enterFn} />
    );
    // return (
    //     <QuestionAnswers
    //         answers={["hallo 1", "hallo2", "hallo3", "hallo4"]} />

    // )
}