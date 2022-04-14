import { AppBar, Box, Button, IconButton, Menu, MenuItem, Toolbar, Typography } from '@mui/material';
import MenuIcon from '@mui/icons-material/Menu';
import React from 'react';
import {
  Outlet,
  Link
} from 'react-router-dom';

import './App.css';
import ResponsiveAppBar from './AppBar';

export default function App() {
  return (
    <div>

      <ResponsiveAppBar />
      <Outlet />
    </div>
  );
}
