import { useNavigate } from "react-router-dom";
import { useFetch, HTTPMETHOD} from "../Utils/useFetch"

import config from  "../config.json"

function fetchWithErrorNavigate(url, httpMethod, paramString, emptyAsList)
{
    const navigate = useNavigate();
    const {data, error, loading} = useFetch(url, httpMethod, paramString);

    if ( error ) navigate("/404");
    if (_isEmtpy(data) && emptyAsList) 
    {
        var _data = []
        return {data: _data, loading, error};
    }
    return {data, loading, error};
}


function GetRecordsByTimeSpan(cashflowType, startDate, endDate){
    const requestUrl = `${config.server_url}/record/${cashflowType}/byTime?startDate=${startDate.toISOString()}&endDate=${endDate.toISOString()}`

    return fetchWithErrorNavigate(requestUrl, HTTPMETHOD.GET, undefined, true);
}

function AddRecord(cashflowType, record)
{
    const requestUrl = `${config.server_url}/record/${cashflowType}`
    const paramString = JSON.stringify(record);

    return fetchWithErrorNavigate(requestUrl, HTTPMETHOD.POST, paramString, undefined);
}

function GetAllCashflowType(cashflowType){
    const requestUrl = `${config.server_url}/type/${cashflowType}`;

    return fetchWithErrorNavigate(requestUrl, HTTPMETHOD.GET, undefined, true);
}

function GetAllPaymentMethod(){
    const requestUrl = `${config.server_url}/paymentmethod`;

    return fetchWithErrorNavigate(requestUrl, HTTPMETHOD.GET, undefined, true);
}

function GetAllCurrencies(){
    const requestUrl = `${config.server_url}/currency/all`;

    return fetchWithErrorNavigate(requestUrl, HTTPMETHOD.GET, undefined, true);
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
    GetAllCurrencies
}