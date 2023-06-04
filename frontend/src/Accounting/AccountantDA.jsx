import React from "react";
import { redirect  } from "react-router-dom";
import { useFetchData, simpleFetch, HTTPMETHOD} from "../Utils/useFetch"

import config from  "../config.json"

function UseFetchForGet(url, httpMethod, paramString, emptyAsList)
{
    const {data, error, loading} = useFetchData(url, httpMethod, paramString);

    if ( error ) redirect("/404");  
    if ( _isEmtpy(data) && emptyAsList ) 
    {
        var _data = []
        return {data: _data, loading, error};
    }
    return {data, loading, error};
}

async function UseFetchForNoneGet(url, httpMethod, paramString, finishHandler)
{
    try
    {
        const response = await simpleFetch(url, httpMethod, paramString);
        if (! response.ok) throw response;
    }
    catch
    {
        redirect("/404"); 
    }
    finishHandler();
}


function GetRecordsByTimeSpan(cashflowType, filter){
    const requestUrl = `${config.server_url}/record/${cashflowType.toLowerCase()}/byTime?` + filter.ToQueryString();

    return UseFetchForGet(requestUrl, HTTPMETHOD.GET, undefined, true);
}

function GetAllCashflowType(cashflowType){
    const requestUrl = `${config.server_url}/type/${cashflowType.toLowerCase()}`;

    return UseFetchForGet(requestUrl, HTTPMETHOD.GET, undefined, true);
}

function GetAllPaymentMethod(){
    const requestUrl = `${config.server_url}/paymentmethod`;

    return UseFetchForGet(requestUrl, HTTPMETHOD.GET, undefined, true);
}

function GetAllCurrencies(){
    const requestUrl = `${config.server_url}/currency/all`;

    return UseFetchForGet(requestUrl, HTTPMETHOD.GET, undefined, true);
}

function AddRecord(cashflowType, record, finishHandler)
{
    const requestUrl = `${config.server_url}/record/${cashflowType.toLowerCase()}`
    const paramString = JSON.stringify(record.ToExternal());

    UseFetchForNoneGet(requestUrl, HTTPMETHOD.POST, paramString, finishHandler);
}

function EditRecord(cashflowType, record, finishHandler)
{
    const requestUrl = `${config.server_url}/record/${cashflowType.toLowerCase()}`
    const paramString = JSON.stringify(record.ToExternal());

    UseFetchForNoneGet(requestUrl, HTTPMETHOD.PUT, paramString, finishHandler);
}

function RemoveRecord(cashflowType, record, finishHandler)
{
    const requestUrl = `${config.server_url}/record/${cashflowType.toLowerCase()}`
    const paramString = JSON.stringify(record.ToExternal());

    UseFetchForNoneGet(requestUrl, HTTPMETHOD.DELETE, paramString, finishHandler);
}

function _isEmtpy(object)
{
    if (!object) return true;
    if (Object.keys(object).length === 0) return true;
    return false;
}

export {
    GetRecordsByTimeSpan,
    GetAllCashflowType,
    GetAllPaymentMethod,
    GetAllCurrencies,
    AddRecord,
    EditRecord,
    RemoveRecord,
}