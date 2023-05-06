import React from "react";
import { useDrag, useDrop } from "react-dnd";

import { DndTypes } from "./AccountingConstants";

class Field
{
    width;
    displayName;
    elementConstructor;

    constructor(width, displayName, elementConstructor)
    {
        this.width = width;
        this.displayName = displayName;
        this.elementConstructor = elementConstructor;
    };
    
    setWidth(width) 
    {
        this.width = width;
    }

    getDataElement(record)
    {
        return this.elementConstructor({width: this.width, ...record})
    }

    getTitleElement(moveField)
    {
        return <TitleElement key={this.displayName} displayText={this.displayName} width={this.width} dragHandler={moveField}/>
    }
}


function TitleElement({displayText, width, dragHandler}) {
    const [_, drag] = useDrag({
        type: DndTypes.HEADER,
        item: {dragElementText:displayText}
    });
    const [{}, drop] = useDrop({
        accept: DndTypes.HEADER,
        drop: ({dragElementText}) => dragHandler(dragElementText, displayText),
    });

    return (
        <div ref={drop} className={"accounting_record_header_cell"} style={{width:width}}>
            <p ref={drag} style={{margin:"0", }}>{displayText}</p>
        </div>
    );
}


function DefaultElement({displayText, hintText, width}) {
    hintText = hintText ? hintText : displayText;
    return (
        <div className={"accounting_record_cell"} style={{width:width}} hint={hintText}>
            {displayText}
        </div>
    );
}

function AmountElement({amount, width}) {
    return <DefaultElement displayText={amount} width={width}/>;
}

function TypeElement({type, width}) {
    return <DefaultElement displayText={type.typeName} width={width}/>;
}

function CurrencyElement({curr, width}) {
    return <DefaultElement displayText={curr.currIso} hintText={curr.currName} width={width}/>;
}

function DateElement({date, width}) {
    let date_hint_str = date.toLocaleString('default', 
        {year: "numeric",month: 'short', day: "2-digit", weekday: 'short', hour: "2-digit", minute: "2-digit"}
    );

    let date_str = date.toLocaleString('default', 
        {month: 'short', day: "2-digit", hour: "2-digit", minute: "2-digit"}
    );
    return <DefaultElement displayText={date_str} hintText={date_hint_str} width={width}/>;
}

function MethodElement({method, width}) {
    return <DefaultElement displayText={method.methodName} width={width}/>;
}

function NoteElement({note, width}) {
    return <DefaultElement displayText={note} hintText={note} width={width}/>;
}

const AccountingFields = [
    new Field("6em", "AMOUNT", AmountElement),
    new Field("6em", "TYPE", TypeElement),
    new Field("8em", "CURRENCY", CurrencyElement),
    new Field("12em", "DATE", DateElement),
    new Field("6em", "PAYMENT", MethodElement),
    new Field("12em", "NOTE", NoteElement),
]

export default AccountingFields;