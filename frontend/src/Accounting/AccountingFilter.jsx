class AccountingFilter
{
    _startDate;
    _endDate;
    currency;
    type;
    method;
    
    get startDate() {
        return formateDate(this._startDate);
    }

    set startDate(value) {
        const date = new Date(value);
        this._startDate = new Date( date.getTime() + date.getTimezoneOffset()*60*1000);
    }

    get endDate() {
        return formateDate(this._endDate);
    }

    set endDate(value) {
        const date = new Date(value);
        this._endDate = new Date( date.getTime() + date.getTimezoneOffset()*60*1000);
    }


    constructor ({_startDate, _endDate, currency, type, method }={})
    {
        const today = new Date();
        this._startDate = new Date(today.getFullYear(), today.getMonth(), 1);        
        this._endDate = new Date(today.getFullYear(), today.getMonth() + 1, 1); 

        this._startDate = _startDate ? _startDate : this._startDate;
        this._endDate = _endDate ? _endDate : this._endDate;
        this.currency = currency ? currency : "ALL";
        this.type = type ? type : -1;
        this.method = method ? method : -1;
    }

    ToQueryString()
    {
        const queryStrings = [];
        queryStrings.push(`startDate=${this._startDate.toISOString()}`);
        queryStrings.push(`endDate=${this._endDate.toISOString()}`);
        if (this.currency !== "ALL") queryStrings.push(`currency=${this.currency}`);
        if (this.method >= 0) queryStrings.push(`method=${this.method}`);
        if (this.type >= 0) queryStrings.push(`type=${this.type}`);

        return queryStrings.join("&");
    }
}

function formateDate(datetime)
{
    datetime = new Date(datetime.getTime() - datetime.getTimezoneOffset()*60*1000);
    let dateString = datetime.toISOString();
    let [date, _] = dateString.split("T");
    return date;
}

export { AccountingFilter };