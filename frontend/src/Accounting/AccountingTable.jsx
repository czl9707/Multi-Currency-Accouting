import React, { useState } from "react";
import { DndProvider } from 'react-dnd';
import { HTML5Backend } from 'react-dnd-html5-backend';

import "./Accounting.css";

import { HorizontalDivider } from "../Utils/Layout";
import { AccountingFields } from "./AccountingFields";
import { Button } from "../Utils/Button";

function AccountingTable ({records, popupDataProvider, editRecordClickHandler, removeRecordClickHandler}) {
    const [fields, setFields] = useState(Object.values(AccountingFields));

    return (
        <>
            <AccountingHead fields={fields} setFields={setFields}/>
            <HorizontalDivider/>
            <AccountingBody fields={fields} records={records} popupDataProvider={popupDataProvider}
                editRecordClickHandler={editRecordClickHandler} removeRecordClickHandler={removeRecordClickHandler}/>
        </>
    );
}

function moveFieldWrapper(setFields){
    return (from, to) => {
        setFields((fields) => {
            let fromIndex = fields.indexOf(fields.find((f) => f.title === from));
            let toIndex = fields.indexOf(fields.find((f) => f.title === to));
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
                        (field, _) => field.getTitleElement(moveFieldWrapper(setFields))
                    )}
                </DndProvider>
                <div style={{flex:"1 1"}}></div>
                <Button picture="edit" grayed transparent/>
                <Button picture="delete" grayed transparent/>
            </div>
        </>
    );
}


function AccountingRecord ({fields, record, editRecordClickHandler, removeRecordClickHandler}) {
    return (
        <> 
            <div className="accounting_record">
                {fields.map(
                    (field, _) => field.getDataElement(record)
                )}
                <div style={{flex:"1 1"}}></div>
                <Button picture="edit" onClickHandler={editRecordClickHandler}/>
                <Button picture="delete" onClickHandler={removeRecordClickHandler}/>
            </div>
            <HorizontalDivider/>
        </>
    );
}

function AccountingBody ({fields, records = [], popupDataProvider, editRecordClickHandler, removeRecordClickHandler}){
    return (
        <div id="accounting_body" className="scroll_shadow">
            {records.map((record, i) => 
                <AccountingRecord fields={fields} record={record} key={i}
                    editRecordClickHandler={
                        () => {
                            popupDataProvider(record);
                            editRecordClickHandler();
                        }
                    } 
                    removeRecordClickHandler={
                        () => {
                            popupDataProvider(record);
                            removeRecordClickHandler();
                        }
                    }/>
            )}
        </div>
    );
}



export { AccountingTable };