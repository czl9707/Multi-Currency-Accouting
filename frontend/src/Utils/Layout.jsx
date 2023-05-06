import React from "react";
import "./Layout.css"

function ContentHeader(props){
    return (
        <div id="content_header">
            {props.children}
        </div>
    );
}

function ContentBody(props){
    return (
        <div id="content_body">
            {props.children}
        </div>
    )
}

function ContentFooter(props){
    return (
        <div id="content_footer">
            {props.children}
        </div>
    )
}

function ContentTitle({title})
{
    return (
        <div id="content_title">
            <p style={{fontSize:"2em", fontWeight:"600", margin: 0}}>{title}</p>
        </div>
    );
}

function HorizontalDivider(){
    return <div className="content_divider_horizontal"></div>;
}

function VerticalDivider(){
    return <div className="content_divider_vertical"></div>;
}


export { ContentHeader, ContentBody, ContentFooter, ContentTitle, HorizontalDivider, VerticalDivider};