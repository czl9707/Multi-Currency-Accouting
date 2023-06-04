import React from "react";
import { useDrag, useDrop } from "react-dnd";

import { DndTypes } from "./AccountingConstants";
import { floatValidator, lengthValidator } from "../Utils/Validator";

class Field
{
    width;
    title;
    validator;

    setValue;
    getValue;
    getDisplayName;
    gethint;

    constructor({width, title, valueField, displayField, hintField, validator = []})
    {
        this.width = width;
        this.title = title;
        this.validator = validator;
        
        this.setValue = (record, value) => record[valueField] = value; 
        this.getValue = (record) => record[valueField];


        if (typeof displayField === 'string'){
            this.getDisplayName = (record) => record[displayField];
        }else if (typeof displayField === 'function'){
            this.getDisplayName = displayField;
        }

        if (!hintField){
            this.getHint = (record) => this.getDisplayName(record);
        } else if (typeof hintField === 'string'){
            this.getHint = (record) => record[hintField];
        } else if (typeof hintField === 'function'){
            this.getHint = hintField;
        }
    }

    getDataElement(record)
    {
        return <DefaultElement 
            key={this.title}
            displayText={this.getDisplayName(record)}
            hintText={this.getHint(record)} 
            width={this.width}/>
    }

    getTitleElement(moveField)
    {
        return <TitleElement key={this.title} title={this.title} width={this.width} dragHandler={moveField}/>
    }
}


function TitleElement({title, width, dragHandler}) {
    const [_, drag] = useDrag({
        type: DndTypes.HEADER,
        item: {dragElementText:title}
    });
    const [{}, drop] = useDrop({
        accept: DndTypes.HEADER,
        drop: ({dragElementText}) => dragHandler(dragElementText, title),
    });

    return (
        <div ref={drop} className="accounting_record_header_cell" style={{width:width}}>
            <p ref={drag} style={{margin:"0", }}>{title}</p>
        </div>
    );
}


function DefaultElement({displayText, hintText, width}) {
    return (
        <div className="accounting_record_cell" style={{width:width}} hint={hintText}>
            {displayText}
        </div>
    );
}

const AccountingFields = {
    amount : new Field({
        width: "6em",
        title: "AMOUNT",
        displayField: "amount",
        valueField: "amount",
        validator: [floatValidator(2)]
    }),
    type: new Field({
        width: "6em",
        title: "TYPE",
        displayField: "typeName",
        valueField: "typeId",
    }),
    currency: new Field({
        width: "8em",
        title: "CURRENCY",
        displayField: "currIso",
        valueField: "currIso",
        hintField: "currName",
    }),
    date: new Field({
        width: "12em",
        title: "DATE",
        displayField: (record) => record._date.toLocaleString(
            'default', 
            {month: 'short', day: "2-digit", hour: "2-digit", minute: "2-digit"}
        ),
        valueField: "date",
        hintField: (record) => record._date.toLocaleString(
            'default', 
            {year: "numeric",month: 'short', day: "2-digit", weekday: 'short', hour: "2-digit", minute: "2-digit"}
        ),
    }),
    method: new Field({
        width: "6em",
        title: "PAYMENT",
        displayField: "methodName",
        valueField: "methodId",
    }),
    note: new Field({
        width: "24em",
        title: "NOTE",
        displayField: "note",
        valueField: "note",
        validator: [lengthValidator(120)]
    })
}

export { AccountingFields };
