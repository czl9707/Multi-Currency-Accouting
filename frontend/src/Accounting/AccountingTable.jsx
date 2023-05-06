import React, { useState } from "react";
import { DndProvider } from 'react-dnd'
import { HTML5Backend } from 'react-dnd-html5-backend'

import { HorizontalDivider } from "../Utils/Layout";
import AccountingFields from "./AccountingFields";
import { Button } from "../Utils/Button";

function AccountingTable ({records}) {
    const [fields, setFields] = useState(AccountingFields);

    return (
        <>
            <AccountingHead fields={fields} setFields={setFields}/>
            <HorizontalDivider/>
            <AccountingBody fields={fields} records={records}/>
        </>
    );
}

function moveField(setFields){
    return (from, to) => {
        setFields((fields) => {
            let fromIndex = fields.indexOf(fields.find((f) => f.displayName === from));
            let toIndex = fields.indexOf(fields.find((f) => f.displayName === to));
            if (fromIndex === toIndex ||
                fromIndex < 0 || fromIndex >= fields.length ||
                toIndex < 0 || toIndex >= fields.length
                ) return fields;
                
            let f = fields[fromIndex];
            fields.splice(fromIndex, 1);
            fields.splice(toIndex, 0, f);
            return [...fields];
        });
    };
}

function AccountingHead ({fields, setFields}){
    return (
        <>
            <div style={{height:"1em"}}></div>
            <div className="accounting_record">
                <DndProvider backend={HTML5Backend}>
                    {fields.map(
                        (field, _) => field.getTitleElement(moveField(setFields))
                    )}
                </DndProvider>
                <div style={{flex:"1 1"}}></div>
                <Button picture={"edit"} grayed transparent/>
                <Button picture={"delete"} grayed transparent/>
            </div>
        </>
    );
}


function AccountingRecord ({fields, record}) {
    return (
        <> 
            <div className="accounting_record">
                {fields.map(
                    (field, _) => field.getDataElement(record)
                )}
                <div style={{flex:"1 1"}}></div>
                <Button picture={"edit"}/>
                <Button picture={"delete"}/>
            </div>
            <HorizontalDivider/>
        </>
    );
}

function AccountingBody ({fields, records = []}){
    return (
        <div>
            {records.map((record, i) => <AccountingRecord fields={fields} record={record} key={i}/>)}
        </div>
    );
}



export { AccountingTable };