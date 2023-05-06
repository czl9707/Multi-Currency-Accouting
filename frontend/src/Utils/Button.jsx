import React from "react";
import "./Button.css"

function Button({picture, name, onClickHandler, colored, border, reverse, grayed, transparent}) {
    let className = ("content_button " + 
        (colored ? "color_button" : "white_button") + " " +
        (grayed ? "button_grayed" : "")
    );
    let overideStyle = {}
    if (! border) overideStyle["borderStyle"] = "none";
    if (reverse) overideStyle["flexDirection"] = "row-reverse";
    if (transparent) overideStyle["opacity"] = 0;

    return (
        <div className={className} style={overideStyle} onClick={onClickHandler}>
            {picture && <i className="material-symbols-rounded">{picture}</i>}
            {name && <p> {name} </p>}
        </div>
    );
}

export { Button };