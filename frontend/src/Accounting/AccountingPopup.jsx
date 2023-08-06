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
} from "../Utils/AccountantDA";
import { Record } from "./AccountantRecord";
import { AccountingFields } from "./AccountingFields";
import { Session } from "./AccountingConstants";
import { AccountingFilter } from "./AccountingFilter";


function AddRecordPopup({closeHandler}){
    const [cashflowType, setCashflowType] = useState(Session.EXPENDITURE);
    const [record, setRecord] = useState(new Record());

    let validToAdd = record.amount !== "" && parseFloat(record.amount) > 0;

    const AddRecordHandler = () => AddRecord(cashflowType, record, () => closeHandler(true));

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
                    <TypeSelect cashflowType={cashflowType} setType={UpdateRecord} defaultValue={record.typeId}/>
                </div>
                <div className="popup_item">
                    <label className="popup_left" htmlFor="currency">Currency</label>
                    <CurrencySelect setCurrency={UpdateRecord} defaultValue={record.currIso}/>
                </div>
                <div className="popup_item">
                    <label className="popup_left" htmlFor="date">Date</label>
                    <input className="popup_input popup_right" id="date" type="datetime-local" value={record.date} onChange={UpdateRecord}/>
                </div>
                <div className="popup_item">
                    <label className="popup_left" htmlFor="method">Method</label>
                    <MethodOptions setMethod={UpdateRecord} defaultValue={record.methodId}/>
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
                <TypeSelect cashflowType={cashflowType} includeAll setType={UpdateFilter} defaultValue={filterHolder.type}/>
            </div>
            <div className="popup_item">
                <label className="popup_left" htmlFor="method">Method</label>
                <MethodOptions includeAll setMethod={UpdateFilter} defaultValue={filterHolder.method}/>
            </div>
            <div className="popup_item">
                <label className="popup_left" htmlFor="currency">Currency</label>
                <CurrencySelect includeAll setCurrency={UpdateFilter} defaultValue={filterHolder.currency}/>
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

    const editRecordHanlder = () => EditRecord(cashflowType, record, () => closeHandler(true));

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
                <TypeSelect cashflowType={cashflowType} setType={UpdateRecord} defaultValue={record.typeId}/>
            </div>
            <div className="popup_item">
                <label className="popup_left" htmlFor="currency">Currency</label>
                <CurrencySelect setCurrency={UpdateRecord} defaultValue={record.currIso}/>
            </div>
            <div className="popup_item">
                <label className="popup_left" htmlFor="date">Date</label>
                <input className="popup_input popup_right" id="date" type="datetime-local" value={record.date} onChange={UpdateRecord}/>
            </div>
            <div className="popup_item">
                <label className="popup_left" htmlFor="method">Method</label>
                <MethodOptions setMethod={UpdateRecord} defaultValue={record.methodId}/>
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
    const removeRecordHandler = () => RemoveRecord(cashflowType, record, () => closeHandler(true));

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


function TypeSelect({cashflowType, includeAll, defaultValue, setType}){
    const {data: types, loading} = GetAllCashflowType(cashflowType);

    if (loading)
    {
        return <textarea className="popup_input popup_right" id="type" type="text"/>;
    }
    else
    {
        return (
            <select className="popup_input popup_right" id="type" onChange={setType} defaultValue={defaultValue}>
                {includeAll && <option value={-1} key={-1}> -- ALL -- </option>}
                {types.map((type, _) =>
                    <option value={type.typeId} key={type.typeId}>
                        {type.typeName}
                    </option>
                )}
            </select>
        );
    }
}

function MethodOptions({includeAll, defaultValue, setMethod}){
    const {data: methods, loading} = GetAllPaymentMethod();

    if (loading)
    {
        return <textarea className="popup_input popup_right" id="method" type="text"/>;
    }
    else
    {
        return (
            <select className="popup_input popup_right" id="method" onChange={setMethod} defaultValue={defaultValue}>
            {includeAll && <option value={-1} key={-1}> -- ALL -- </option>}
            {methods.map((method, _) =>
                <option value={method.methodId} key={method.methodId}>
                    {method.methodName}
                </option>
            )}
        </select>   
        );
    }   
}

function CurrencySelect({includeAll, defaultValue, setCurrency}){
    const {data: currencies, loading} = GetAllCurrencies();

    if (loading || currencies.length <= 0)
    {
        return <textarea className="popup_input popup_right" id="currency" type="text"/>
    }
    else
    {
        return (
            <select className="popup_input popup_right" id="currency" onChange={setCurrency} defaultValue={defaultValue}>
                {includeAll && <option value={"ALL"} key={"ALL"}> -- ALL -- </option>}
                {currencies.map((curr, _) =>
                    <option value={curr.currIso} key={curr.currIso}>
                        {`${curr.currIso} (${curr.currName})`}
                    </option>
                )}
            </select>        
        );
    }
}

export { AddRecordPopup, FilterPopup, EditPopup, RemovePopup} ;