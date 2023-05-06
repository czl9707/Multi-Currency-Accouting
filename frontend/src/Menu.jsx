import React from "react";
import { useLocation, useNavigate } from "react-router-dom";

import "./Menu.css"

function Menu() {
    return (
      <div id='menu_div'>
        <p id="co_lab">CO.LAB</p>
        <Tag name="Overview" picture="home" url="/overview"/>
        <Tag name="Accounting" picture="payments" url="/accounting"/>
      </div>
    );
}
  
function Tag({picture, name, url}) {
    const navigate = useNavigate();
    let location = useLocation();
    let selected = location.pathname.startsWith(url);
    let className = "menu_tag " + (selected ? "menu_tag_selected" : "menu_tag_not_selected");
    let onClickHandler = selected ? null : () => navigate(url);

    return (
        <div className={className} onClick={onClickHandler}>
        <i className="material-symbols-rounded">{picture}</i>
        <p> {name} </p>
        </div>
    );
}

export { Menu };