import React, { useRef, useState } from "react";
import { ContentHeader, ContentBody, ContentFooter, ContentTitle} from "../Utils/Layout";
import { Button } from "../Utils/Button";
import { Paging } from "../Utils/Paging";

import { AccountingSessions } from "./AccountingSession";
import { AccountingTable } from "./AccountingTable";
import { Session } from "./AccountingConstants";
import { AccountingFilter } from "./AccountingFilter";
import { HorizontalDivider } from "../Utils/Layout";
import { AddRecordPopup, EditPopup, FilterPopup, RemovePopup } from "./AccountingPopup";
import { LoadingMask } from "../Utils/Mask";


import { GetRecordsByTimeSpan } from "./AccountantDA";

import "./Accounting.css";
import { Record } from "./AccountantRecord";

const RECORDPERPAGE = 12;
const POPUP_TYPE = {
    NONE: "NONE",
    ADD: "ADD",
    FILTER: "FILTER",
    EDIT: "EDIT",
    REMOVE: "REMOVE",
}

function Accounting (){
    const [currentSession, setCurrentSession] = useState(Session.EXPENDITURE);
    const [currentPage, setCurrentPage] = useState(0);
    const [filter, setFilter] = useState(new AccountingFilter());
    const [popup, setPopup] = useState(POPUP_TYPE.NONE);

    const popupData = useRef();

    const closePopUpHandler = ()=>setPopup(POPUP_TYPE.NONE);

    let {data: exteralRecords, loading} = GetRecordsByTimeSpan(currentSession, filter);
    
    let totalPage = Math.max(Math.ceil(exteralRecords.length / RECORDPERPAGE), 1);
    let records = exteralRecords
        .slice(currentPage * RECORDPERPAGE, (currentPage + 1) * RECORDPERPAGE)
        .map((r, _) => Record.FromExternal(r));


    return (
        <>
            <ContentHeader>
                <AccountingHeader 
                    addRecordClickHandler={()=>setPopup(POPUP_TYPE.ADD)} 
                    filterClickHandler={()=>setPopup(POPUP_TYPE.FILTER)}
                />
            </ContentHeader>
            <ContentBody>
                <AccountingSessions currentSession={currentSession} setCurrentSession={setCurrentSession}/>
                <HorizontalDivider/>
                <AccountingTable records={records} popupDataProvider={value => popupData.current = value} 
                    editRecordClickHandler={()=>setPopup(POPUP_TYPE.EDIT)} removeRecordClickHandler={()=>setPopup(POPUP_TYPE.REMOVE)}/>
            </ContentBody>
            <ContentFooter>
                <AccountingPaging totalPage={totalPage} currentPage={currentPage} setCurrentPage={setCurrentPage} />
            </ContentFooter>
            {loading && <LoadingMask/>}
            {popup === POPUP_TYPE.ADD && <AddRecordPopup closeHandler={closePopUpHandler} cashflowType={currentSession}/>}
            {popup === POPUP_TYPE.FILTER && <FilterPopup closeHandler={closePopUpHandler} cashflowType={currentSession} filter={filter} setFilter={setFilter}/>}
            {popup === POPUP_TYPE.EDIT && <EditPopup closeHandler={closePopUpHandler} cashflowType={currentSession} record={popupData.current}/>}
            {popup === POPUP_TYPE.REMOVE && <RemovePopup closeHandler={closePopUpHandler} cashflowType={currentSession} record={popupData.current}/>}
        </>
    );
}

function AccountingHeader({addRecordClickHandler, filterClickHandler}){
    return (
        <>
            <ContentTitle title="Accounting"/>
            <div id="accounting_filter_zone">
                <Button picture="filter_alt" name="Filter" border onClickHandler={filterClickHandler}/>
                <Button picture="add" name="Add Record" colored onClickHandler={addRecordClickHandler} />
            </div>
        </>
    );
}


function AccountingPaging({totalPage, currentPage, setCurrentPage}){
    return (
        <Paging totalPage={totalPage} currentPage={currentPage} setCurrentPage={setCurrentPage} />
    )
}

export { Accounting };