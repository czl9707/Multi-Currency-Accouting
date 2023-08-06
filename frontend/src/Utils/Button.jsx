import React from "react";
import "./Button.css"

function Button({picture, name, onClickHandler, colored, border, reverse, grayed}) {
    let className = ("content_button " + 
        (colored ? "color_button" : "white_button") + " " +
        (grayed ? "button_grayed" : "") + " " + 
        (border ? "button_border" : "") + " " + 
        (reverse ? "button_reverse" : "")
    );

    return (
        <div className={className} onClick={onClickHandler}>
            {picture && <i className="material-symbols-rounded">{picture}</i>}
            {name && <p> {name} </p>}
        </div>
    );
}

export { Button };