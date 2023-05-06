import React from "react";

import { PopupMask } from "./Mask";
import { Button } from "./Button"

import "./Popup.css"

function Popup({children, name, closeHandler}){
    return (
        <PopupMask>
            <div id="popup_div">
                <div id="popup_header">
                    <p>{name}</p>
                    <Button picture={"close"} onClickHandler={closeHandler} colored/>
                </div>
                <div id="popup_body">
                    {children}
                </div>
            </div>
        </PopupMask>
    );
}

export { Popup };