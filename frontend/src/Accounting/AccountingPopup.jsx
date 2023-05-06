import React, { useState } from "react";

import { Popup } from "../Utils/Popup";
import { Button } from "../Utils/Button";
import { lengthValidator, floatValidator } from "../Utils/Validator";
import { GetAllCashflowType, GetAllCurrencies, GetAllPaymentMethod } from "./AccountantDA";
import { Record } from "./AccountantRecord";

function AddRecordPopup({closeHandler, cashflowType}){
    const [record, setRecord] = useState(
        new Record()
    );
    let validToAdd = record.amount !== "" && parseFloat(record.amount) > 0;

    function UpdateRecord(e){
        setRecord((currRecord) => {
            let value = e.target.value;
            for (const validator of FIELD_VALIDATORS[e.target.id])
                value = validator(value, record[e.target.id]);

            return {
                ...currRecord,
                [e.target.id]: value
            }
        });
    }

    function AddRecordHandler()
    {

    }

    return (
        <Popup closeHandler={closeHandler} name={"Add Record"}>
            <form onSubmit={null}>
                <div className="popup_item">
                    <label htmlFor="amount">Amount</label>
                    <textarea className="popup_input" id="amount" type="text" value={record.amount} onChange={UpdateRecord}/>
                </div>
                <div className="popup_item">
                    <label htmlFor="type">Type</label>
                    <select className="popup_input" id="type" value={record.type} onChange={UpdateRecord}>
                        <TypeOptions cashflowType={cashflowType}/>
                    </select>
                </div>
                <div className="popup_item">
                    <label htmlFor="currency">Currency</label>
                    <select className="popup_input" id="currency" value={record.currency} onChange={UpdateRecord}>
                        <CurrencyOptions/>
                    </select>
                </div>
                <div className="popup_item">
                    <label htmlFor="date">Date</label>
                    <input className="popup_input" id="date" type="datetime-local" value={record.date} onChange={UpdateRecord}/>
                </div>
                <div className="popup_item">
                    <label htmlFor="method">Method</label>
                    <select className="popup_input" id="method" value={record.method} onChange={UpdateRecord}>
                        <MethodOptions/>
                    </select>
                </div>
                <div className="popup_item">
                    <label htmlFor="note">Note</label>
                    <textarea className="popup_input" id="note" type="text" value={record.note} onChange={UpdateRecord}
                    style={{height:"5em"}}/>
                </div>
                <div className="popup_submit"></div>
                <Button name={"Submit"} colored={validToAdd} grayed={!validToAdd}
                onClickHandler={validToAdd? AddRecordHandler : null}/>
            </form>
        </Popup>
    );
}

function FilterPopup({closeHandler}){
    return (
        <Popup closeHandler={closeHandler} name={"Filter"}>
            
        </Popup>
    );
}

function TypeOptions({cashflowType}){
    const {data: types} = GetAllCashflowType(cashflowType);
    return (
        <>
            {types.map((type, _) =>
                <option value={type.typeId} key={type.typeId}>{type.typeName}</option>
            )}
        </>
    )
}

function CurrencyOptions(){
    const {data: currencies} = GetAllCurrencies();
    return (
        <>
            {currencies.map((curr, _) =>
                <option value={curr.currIso} key={curr.currI}>{curr.currName}</option>
            )}
        </>
    ) 
}

function MethodOptions(){
    const {data: methods} = GetAllPaymentMethod();
    return (
        <>
            {methods.map((method, _) =>
                <option value={method.methodId} key={method.methodId}>{method.methodName}</option>
            )}
        </>
    ) 
}

const FIELD_VALIDATORS = {
    "amount" : [floatValidator(2)],
    "type" : [],
    "currency" : [],
    "date" : [],
    "method" : [],
    "note" : [lengthValidator(60)]
}


export { AddRecordPopup, FilterPopup } ;