import React from "react";
import "./Button.css"

function Button({picture, name, onClickHandler, colored, border, reverse, grayed, transparent, style={}}) {
    let className = ("content_button " + 
        (colored ? "color_button" : "white_button") + " " +
        (grayed ? "button_grayed" : "")
    );
    if (! border) style["borderStyle"] = "none";
    if (reverse) style["flexDirection"] = "row-reverse";
    if (transparent) style["opacity"] = 0;

    return (
        <div className={className} style={style} onClick={onClickHandler}>
            {picture && <i className="material-symbols-rounded">{picture}</i>}
            {name && <p> {name} </p>}
        </div>
    );
}

export { Button };