import { redirect  } from "react-router-dom";
import { useFetchData, simpleFetch, HTTPMETHOD} from "./useFetch"

import config from  "../config.json"

// forceRefresh an int, change it to trigger refresh
function UseFetchForGet(url, httpMethod, paramString, forceRefresh)
{
    const {data, error, loading} = useFetchData(url + " ".repeat(forceRefresh), httpMethod, paramString);

    if ( error ) redirect("/404");  
    if ( _isEmtpy(data) ) 
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


function GetRecordsByTimeSpan(cashflowType, filter, forceRefresh){
    const requestUrl = `${config.server_url}/record/${cashflowType.toLowerCase()}/byTime?` + filter.ToQueryString();

    return UseFetchForGet(requestUrl, HTTPMETHOD.GET, undefined, forceRefresh);
}

function GetAllCashflowType(cashflowType, forceRefresh){
    const requestUrl = `${config.server_url}/type/${cashflowType.toLowerCase()}`;

    return UseFetchForGet(requestUrl, HTTPMETHOD.GET, undefined, forceRefresh);
}

function GetAllPaymentMethod(forceRefresh){
    const requestUrl = `${config.server_url}/paymentmethod`;

    return UseFetchForGet(requestUrl, HTTPMETHOD.GET, undefined, forceRefresh);
}

function GetAllCurrencies(forceRefresh){
    const requestUrl = `${config.server_url}/currency/all`;

    return UseFetchForGet(requestUrl, HTTPMETHOD.GET, undefined, forceRefresh);
}

function AddPaymentMethod(method, finishHandler){
    const requestUrl = `${config.server_url}/paymentmethod`;
    const paramString = JSON.stringify(method);

    UseFetchForNoneGet(requestUrl, HTTPMETHOD.POST, paramString, finishHandler);
}

function RemovePaymentMethod(method, finishHandler){
    const requestUrl = `${config.server_url}/paymentmethod`;
    const paramString = JSON.stringify(method);

    UseFetchForNoneGet(requestUrl, HTTPMETHOD.DELETE, paramString, finishHandler);
}

function EditPaymentMethod(method, finishHandler){
    const requestUrl = `${config.server_url}/paymentmethod`;
    const paramString = JSON.stringify(method);

    UseFetchForNoneGet(requestUrl, HTTPMETHOD.PUT, paramString, finishHandler);
}

function AddType(cashflowType, type, finishHandler){
    const requestUrl = `${config.server_url}/type/${cashflowType.toLowerCase()}`;
    const paramString = JSON.stringify(type);

    UseFetchForNoneGet(requestUrl, HTTPMETHOD.POST, paramString, finishHandler);
}

function RemoveType(cashflowType, type, finishHandler){
    const requestUrl = `${config.server_url}/type/${cashflowType.toLowerCase()}`;
    const paramString = JSON.stringify(type);

    UseFetchForNoneGet(requestUrl, HTTPMETHOD.DELETE, paramString, finishHandler);
}

function EditType(cashflowType, type, finishHandler){
    const requestUrl = `${config.server_url}/type/${cashflowType.toLowerCase()}`;
    const paramString = JSON.stringify(type);

    UseFetchForNoneGet(requestUrl, HTTPMETHOD.PUT, paramString, finishHandler);
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
    AddPaymentMethod,
    EditPaymentMethod,
    RemovePaymentMethod,
    AddType,
    EditType,
    RemoveType,
}