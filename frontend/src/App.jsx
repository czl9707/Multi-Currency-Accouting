import React from 'react';
import { Routes, Route, BrowserRouter, Navigate} from "react-router-dom";

import './App.css';

import { Overview } from './Overview/Overview';
import { Accounting } from './Accounting/Accounting';
import { Menu } from './Menu';
import { NotFound } from './NotFound/NotFound';

function App() {
  return (
    <BrowserRouter>
      <div id="app_container">
        <Menu/>
        <Content/>
      </div>
    </BrowserRouter>
  );
}

function Content() {
  return (
    <div id='content_div'>
      <Routes>
        <Route path='/overview' element={<Overview/>}/>
        <Route path='/accounting' element={<Accounting/>}/>
        <Route path="/404" element={<NotFound/>}/>
        <Route path="/*" element={<Navigate to="/overview"/>} />
      </Routes>
    </div>
  );
}

export { App };
