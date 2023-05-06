import React from "react";

import "./Mask.css";

function LoadingMask(){
    return (
        <div id="mask_div">
            <div id="mask" className="mask_blink"></div> 
        </div>
    );
}

function PopupMask({children}){
    return (
        <div id="mask_div">
            <div id="mask"></div>
            <div id="on_mask">
                {children}
            </div>
        </div>
    )
}

export {LoadingMask, PopupMask};