import React, { useState } from "react";

import { Popup } from "../Utils/Popup";
import { Button } from "../Utils/Button";
import { 
    GetAllCashflowType, 
    GetAllCurrencies, 
    GetAllPaymentMethod, 
    AddRecord, 
    EditRecord,
    RemoveRecord,
} from "./AccountantDA";
import { Record } from "./AccountantRecord";
import { AccountingFields } from "./AccountingFields";
import { Session } from "./AccountingConstants";
import { AccountingFilter } from "./AccountingFilter";


function AddRecordPopup({closeHandler}){
    const [cashflowType, setCashflowType] = useState(Session.EXPENDITURE);
    const [record, setRecord] = useState(new Record());

    let validToAdd = record.amount !== "" && parseFloat(record.amount) > 0;

    const AddRecordHandler = () => AddRecord(cashflowType, record, closeHandler);

    function UpdateRecord(e){
        setRecord((currRecord) => {
            let value = e.target.value;
            for (const validator of AccountingFields[e.target.id].validator)
                value = validator(value, record[e.target.id]);
            
            AccountingFields[e.target.id].setValue(currRecord, value);
            return new Record(currRecord);
        });
    }

    return (
        <Popup closeHandler={closeHandler} name="Add Record">
            <form>
                <div className="popup_item">
                    <div style={{width:"100%", display:"flex"}}>
                        <Button name="Expense" colored={cashflowType === Session.EXPENDITURE} grayed={cashflowType === Session.EXPENDITURE}
                        onClickHandler={() => setCashflowType(Session.EXPENDITURE)} style={{flex:"1 1", marginLeft:"0", textAlign:"center"}}/>
                        <Button name="Income" colored={cashflowType === Session.INCOME} grayed={cashflowType === Session.INCOME}
                        onClickHandler={() => setCashflowType(Session.INCOME)} style={{flex:"1 1", marginRight:"0", textAlign:"center"}}/>
                    </div>
                </div>

                <div className="popup_item">
                    <label className="popup_left" htmlFor="amount">Amount</label>
                    <textarea className="popup_input popup_right" id="amount" type="text" value={record.amount} onChange={UpdateRecord}/>
                </div>
                <div className="popup_item">
                    <label className="popup_left" htmlFor="type">Type</label>
                    <select className="popup_input popup_right" id="type" value={record.type} onChange={UpdateRecord}>
                        <TypeOptions cashflowType={cashflowType} initValue={record.typeId}/>
                    </select>
                </div>
                <div className="popup_item">
                    <label className="popup_left" htmlFor="currency">Currency</label>
                    <select className="popup_input popup_right" id="currency" value={record.currency} onChange={UpdateRecord}>
                        <CurrencyOptions initValue={record.currIso}/>
                    </select>
                </div>
                <div className="popup_item">
                    <label className="popup_left" htmlFor="date">Date</label>
                    <input className="popup_input popup_right" id="date" type="datetime-local" value={record.date} onChange={UpdateRecord}/>
                </div>
                <div className="popup_item">
                    <label className="popup_left" htmlFor="method">Method</label>
                    <select className="popup_input popup_right" id="method" value={record.method} onChange={UpdateRecord}>
                        <MethodOptions initValue={record.methodId}/>
                    </select>
                </div>
                <div className="popup_item">
                    <label className="popup_left" htmlFor="note">Note</label>
                    <textarea className="popup_input popup_right" id="note" type="text" value={record.note} onChange={UpdateRecord}
                    style={{height:"5em"}}/>
                </div>

                <br/>
                <div className="popup_item">
                    <div style={{flex:"1 1"}}/>
                    <Button name="Submit" colored={validToAdd} grayed={!validToAdd}
                    onClickHandler={validToAdd? AddRecordHandler : null} style={{marginRight:"0"}}/>
                </div>
            </form>
        </Popup>
    );
}

function FilterPopup({closeHandler, cashflowType, filter, setFilter}){
    const [filterHolder, setfilterHolder] = useState(new AccountingFilter(filter));
    console.log(filterHolder);
    function UpdateFilter(e){
        setfilterHolder((currFilter) => {
            currFilter[e.target.id] = e.target.value;
            return new AccountingFilter(currFilter);
        });
    }

    function FinishHandler()
    {
        setFilter(filterHolder);
        closeHandler();
    }

    return (
        <Popup closeHandler={closeHandler} name="Filter">
            <div className="popup_item">
                <label className="popup_left" htmlFor="date">Start Date</label>
                <input className="popup_input popup_right" id="startDate" type="date" value={filterHolder.startDate} onChange={UpdateFilter}/>
            </div>
            <div className="popup_item">
                <label className="popup_left" htmlFor="date">End Date</label>
                <input className="popup_input popup_right" id="endDate" type="date" value={filterHolder.endDate} onChange={UpdateFilter}/>
            </div>
            <div className="popup_item">
                <label className="popup_left" htmlFor="type">Type</label>
                <select className="popup_input popup_right" id="type" value={filterHolder.type} onChange={UpdateFilter}>
                    <TypeOptions cashflowType={cashflowType} includeAll initValue={filterHolder.type}/>
                </select>
            </div>
            <div className="popup_item">
                <label className="popup_left" htmlFor="method">Method</label>
                <select className="popup_input popup_right" id="method" value={filterHolder.method} onChange={UpdateFilter}>
                    <MethodOptions includeAll initValue={filterHolder.method}/>
                </select>
            </div>
            <div className="popup_item">
                <label className="popup_left" htmlFor="currency">Currency</label>
                <select className="popup_input popup_right" id="currency" value={filterHolder.currency} onChange={UpdateFilter}>
                    <CurrencyOptions includeAll initValue={filterHolder.currency}/>
                </select>
            </div>
            <div className="popup_item">
                <div style={{flex:"1 1"}}/>
                <Button name="Confirm" colored
                onClickHandler={FinishHandler} style={{marginRight:"0"}}/>
            </div>
        </Popup>
    );
}

function EditPopup({closeHandler, record: rawRecord, cashflowType}){
    const [record, setRecord] = useState(new Record(rawRecord));

    let validToAdd = record.amount !== "" && parseFloat(record.amount) > 0;

    const editRecordHanlder = () => EditRecord(cashflowType, record, closeHandler);

    function UpdateRecord(e){
        setRecord((currRecord) => {
            let value = e.target.value;
            for (const validator of AccountingFields[e.target.id].validator)
                value = validator(value, record[e.target.id]);
            
            AccountingFields[e.target.id].setValue(currRecord, value);
            return new Record(currRecord);
        });
    }

    return (
        <Popup closeHandler={closeHandler} name="Edit">
            <div className="popup_item">
                <label className="popup_left" htmlFor="amount">Amount</label>
                <textarea className="popup_input popup_right" id="amount" type="text" value={record.amount} onChange={UpdateRecord}/>
            </div>
            <div className="popup_item">
                <label className="popup_left" htmlFor="type">Type</label>
                <select className="popup_input popup_right" id="type" value={record.type} onChange={UpdateRecord}>
                    <TypeOptions cashflowType={cashflowType} initValue={record.typeId}/>
                </select>
            </div>
            <div className="popup_item">
                <label className="popup_left" htmlFor="currency">Currency</label>
                <select className="popup_input popup_right" id="currency" value={record.currency} onChange={UpdateRecord}>
                    <CurrencyOptions initValue={record.currIso}/>
                </select>
            </div>
            <div className="popup_item">
                <label className="popup_left" htmlFor="date">Date</label>
                <input className="popup_input popup_right" id="date" type="datetime-local" value={record.date} onChange={UpdateRecord}/>
            </div>
            <div className="popup_item">
                <label className="popup_left" htmlFor="method">Method</label>
                <select className="popup_input popup_right" id="method" value={record.method} onChange={UpdateRecord}>
                    <MethodOptions initValue={record.methodId}/>
                </select>
            </div>
            <div className="popup_item">
                <label className="popup_left" htmlFor="note">Note</label>
                <textarea className="popup_input popup_right" id="note" type="text" value={record.note} onChange={UpdateRecord}
                style={{height:"5em"}}/>
            </div>

            <br/>
            <div className="popup_item">
                <div style={{flex:"1 1"}}/>
                <Button name="Submit" colored={validToAdd} grayed={!validToAdd}
                onClickHandler={validToAdd? editRecordHanlder : null} style={{marginRight:"0"}}/>
            </div>
        </Popup>
    );
}

function RemovePopup({closeHandler, record, cashflowType}){
    const removeRecordHandler = () => RemoveRecord(cashflowType, record, closeHandler);

    return (
        <Popup closeHandler={closeHandler} name="Remove">
            <div className="popup_item">
                <label>Are you Sure? Delete CAN NOT UNDO !!!</label>
            </div>
            <div className="popup_item">
                <div style={{flex:"1 1"}}/>
                <Button name="Confirm" colored
                onClickHandler={removeRecordHandler} style={{marginRight:"0"}}/>
            </div>
        </Popup>
    );
}


function TypeOptions({cashflowType, includeAll, initValue}){
    const {data: types} = GetAllCashflowType(cashflowType);
    return (
        <>
            {includeAll && <option value={-1} key={-1} selected={initValue == -1}> -- ALL -- </option>}
            {types.map((type, _) =>
                <option value={type.typeId} key={type.typeId} selected={initValue == type.typeId}>
                    {type.typeName}
                </option>
            )}
        </>
    )
}

function MethodOptions({includeAll, initValue}){
    const {data: methods} = GetAllPaymentMethod();
    return (
        <>
            {includeAll && <option value={-1} key={-1} selected={initValue == -1}> -- ALL -- </option>}
            {methods.map((method, _) =>
                <option value={method.methodId} key={method.methodId} selected={initValue == method.methodId}>
                    {method.methodName}
                </option>
            )}
        </>
    ) 
}

function CurrencyOptions({includeAll, initValue}){
    const {data: currencies} = GetAllCurrencies();
    return (
        <>
            {includeAll && <option value={"ALL"} key={"ALL"} selected={initValue == "ALL"}> -- ALL -- </option>}
            {currencies.map((curr, _) =>
                <option value={curr.currIso} key={curr.currIso} selected={initValue == curr.currIso}>
                    {`${curr.currIso} (${curr.currName})`}
                </option>
            )}
        </>
    ) 
}

export { AddRecordPopup, FilterPopup, EditPopup, RemovePopup} ;