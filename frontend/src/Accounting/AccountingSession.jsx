import React from "react";

import { Button } from "../Utils/Button";
import { Session } from "./AccountingConstants";

import "./Accounting.css";


function AccountingSessions({currentSession, setCurrentSession}){
    return (
        <div id="accounting_session_zone">
            <AccountingSessionTag name={"Expenditure"} sessionNum={Session.EXPENDITURE}
            currentSession={currentSession} setCurrentSession={setCurrentSession}/>
            <AccountingSessionTag name={"Income"} sessionNum={Session.INCOME}
            currentSession={currentSession} setCurrentSession={setCurrentSession}/>
        </div>
    );
}

function AccountingSessionTag({name, sessionNum, currentSession, setCurrentSession}){
    let grayed = sessionNum === currentSession;
    let className = grayed ? "session_tag_selected": "";
    let onClick = !grayed ? (() => setCurrentSession(sessionNum)) : null;
    return (
        <div className={className}>
            <Button name={name} grayed={grayed} onClickHandler={onClick}/>
        </div>
    );
}

export {AccountingSessions};