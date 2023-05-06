import React from "react";
import "./Paging.css";
import { Button } from "./Button";

const PAGENUMBER = 7;

function Paging({totalPage, currentPage, setCurrentPage}){
    return (
        <div id="content_paging_zone">
            <Button picture="arrow_back" name="previous" 
                onClickHandler={currentPage === 0 ? null : ()=>setCurrentPage(currentPage - 1)}
                grayed={currentPage === 0}
            />
            {PagingTags({totalPage, currentPage, setCurrentPage})}
            <Button picture="arrow_forward" name="next" reverse
                onClickHandler={currentPage === totalPage - 1 ? null : ()=>setCurrentPage(currentPage + 1)}
                grayed={currentPage === totalPage - 1}
            />
        </div>
    )
}

function PagingTags({totalPage, currentPage, setCurrentPage}){
    let startNumber = currentPage, endNumber = currentPage;
    while (endNumber - startNumber + 1 < Math.min(totalPage, PAGENUMBER)){
        if (startNumber > 0) startNumber --;
        if (endNumber < totalPage - 1) endNumber ++;
    }

    let pageNumber = endNumber - startNumber + 1;

    return (
        <div id="content_paging" >
            <div>
                {startNumber > 0 && <Button name={"..."} grayed/>}
                {[...Array(pageNumber)].map((_, i) => PageTag(currentPage, i + startNumber, setCurrentPage))}
                {endNumber < totalPage - 1 && <Button name={"..."} grayed/>}
            </div>
        </div>
    );
}

function PageTag(currentPage, page, setCurrentPage){
    let grayed = page === currentPage;
    let onClickHandler = grayed ? null : () => setCurrentPage(page);
    return <Button name={page + 1} colored={grayed} key={page} 
        onClickHandler={onClickHandler} grayed={grayed}/>;
}

export { Paging };