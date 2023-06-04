class Record
{
    cashFlowId;
    amount;
    typeId;
    typeName;
    currIso;
    currName; 
    methodId;
    methodName;
    note;
    _date;
    
    get date (){
        return formateDate(this._date);
    }

    set date (value){
        this._date = new Date(value);
    }

    constructor({amount, typeId, typeName, currIso, currName, _date, methodId, methodName, note, cashFlowId} = {})
    {
        this.cashFlowId = cashFlowId ? cashFlowId : -1;
        this.amount = amount ? amount : "";
        this.typeId = typeId ? typeId : 0;
        this.currIso = currIso ? currIso : "UNK";
        this._date = _date ? _date : new Date();
        this.methodId = methodId ? methodId : 0;
        this.note = note ? note : "";

        this.typeName = typeName;
        this.methodName = methodName;
        this.currName = currName;
    }

    static FromExternal(exteral_record)
    {        
        let _date = new Date(exteral_record.happenUtc);
        _date = new Date(_date.getTime() - _date.getTimezoneOffset()*60*1000);

        return new Record({
            ...exteral_record,
            _date: _date,
        });
    }

    ToExternal()
    {
        return {
            Amount: parseFloat(this.amount),
            TypeId: parseInt(this.typeId),
            CurrIso: this.currIso,
            HappenUtc: this._date,
            MethodId: parseInt(this.methodId),
            Note: this.note,
            CashFlowId: parseInt(this.cashFlowId),
        }
    }
}

function formateDate(datetime)
{
    datetime = new Date(datetime.getTime() - datetime.getTimezoneOffset()*60*1000);
    let dateString = datetime.toISOString();
    let [date, time] = dateString.split("T");
    time = time.split(".")[0];
    return date + "T" + time;
}

export { Record };