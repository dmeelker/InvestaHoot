import React from 'react';
import {
  Outlet,
  Link
} from 'react-router-dom';

import './App.css';

export default function App() {
  return (
    <div>
      <h1>investahoot!</h1>
      <nav
        style={{
          borderBottom: "solid 1px",
          paddingBottom: "1rem",
        }}
      >
        <Link to="/game">Game</Link>
        <Link to="/about">About</Link> |{" "}
      </nav>
      <Outlet />
    </div >
  );
}
