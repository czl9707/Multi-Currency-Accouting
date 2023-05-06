class Record
{
    amount 
    type 
    currency 
    date
    payment 
    note 

    constructor(amount, type, currency, date, payment, note)
    {
        this.amount = "0";
        this.type = "0";
        this.currency = "0";
        this.date = formateDate(new Date())
        this.payment = "0";
        this.note = "";
        
        this.amount = amount ? amount : this.amount;
        this.type = type ? type : this.type;
        this.currency = currency ? currency : this.currency;
        this.date = date ? formateDate(date) : this.date;
        this.payment = payment ? payment : this.payment;
        this.note = note ? note : this.note;
    }

    static FromExternal(recordFromBack)
    {
        return Record(

        );
    }

    ToExternal(record)
    {
        return {

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