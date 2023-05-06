import React, { useState } from "react";
import { ContentHeader, ContentBody, ContentFooter, ContentTitle} from "../Utils/Layout";
import { Button } from "../Utils/Button";
import { Paging } from "../Utils/Paging";

import { AccountingSessions } from "./AccountingSession"
import { AccountingTable } from "./AccountingTable";
import { HorizontalDivider } from "../Utils/Layout";
import { Session } from "./AccountingConstants";
import { AddRecordPopup, FilterPopup } from "./AccountingPopup";
import { LoadingMask } from "../Utils/Mask";


import { GetRecordsByTimeSpan } from "./AccountantDA";

import "./Accounting.css";

const RECORDPERPAGE = 10

function Accounting (){
    const [currentSession, setCurrentSession] = useState(Session.EXPENDITURE);
    const [currentPage, setCurrentPage] = useState(0);
    const [addPopup, setAddPopup] = useState(false);
    const [filterPopup, setFilterPopup] = useState(false);

    let {data: records, loading} = GetRecordsByTimeSpan(
        currentSession,
        new Date(2023, 3, 3),
        new Date(2023, 3, 3)
    );
    let totalPage = Math.max(Math.ceil(records.length / RECORDPERPAGE), 1);

    return (
        <>
            <ContentHeader>
                <AccountingHeader addRecordClickHandler={()=>setAddPopup(true)} filterClickHandler={()=>setFilterPopup(true)}/>
            </ContentHeader>
            <ContentBody>
                <AccountingSessions currentSession={currentSession} setCurrentSession={setCurrentSession}/>
                <HorizontalDivider/>
                <AccountingTable records={records}/>
            </ContentBody>
            <ContentFooter>
                <AccountingPaging totalPage={totalPage} currentPage={currentPage} setCurrentPage={setCurrentPage} />
            </ContentFooter>
            {loading && <LoadingMask/>}
            {addPopup && <AddRecordPopup closeHandler={()=>setAddPopup(false)} cashflowType={currentSession}/>}
            {filterPopup && <FilterPopup closeHandler={()=>setFilterPopup(false)} cashflowType={currentSession}/>}
        </>
    );
}

function AccountingHeader({addRecordClickHandler, filterClickHandler}){
    return (
        <>
            <ContentTitle title={"Accounting"}/>
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