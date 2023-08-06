import React, { useEffect, useRef, useState } from "react"

import { HorizontalDivider } from "../Utils/Layout"

import "./Customization.css"
import { EditCustomizationItem, AddCustomizationItem } from "./CustomizationItem";
import { Button } from "../Utils/Button";


function CustomizationCatagory(
    { title, fetchData, extractValue, setValue, addItem, updateItem, removeItem}
)
{
    const [isExpand, setIsExpand] = useState(false); 
    const [forceRefreshHelper, setForceRefreshHelper] = useState(0);
    const forceRefresh = () => setForceRefreshHelper(forceRefreshHelper + 1);

    const {data: _items} = fetchData(forceRefreshHelper);
    let items = _items.filter(d => d.typeId != 0)
    
    return (
        <>
            <CustomizationCatagoryHeader title={title} isExpand={isExpand} setIsExpand={setIsExpand}/>
            {
                isExpand && <CustomizationCatagoryBody
                    items={items} 
                    extractValue={extractValue}
                    setValue={setValue}
                    addItem={addItem}
                    removeItem={removeItem}
                    updateItem={updateItem}
                    forceRefresh={forceRefresh}
                />
            }
            <HorizontalDivider/>
        </>
    );
}

function CustomizationCatagoryHeader({title, isExpand, setIsExpand})
{
    const expand = () => setIsExpand(true);
    const collapse = () => setIsExpand(false);

    return (
        <div className="customization_catagory_title" onClick={isExpand ? null : expand}>
            <p > {title} </p>
            <div style={{flex:"1 1"}}/>
            <CustomizationCatagoryExpand isExpand={isExpand} expand={expand} collapse={collapse}/>
        </div>
    );
}

function CustomizationCatagoryExpand({isExpand, expand, collapse})
{
    return(
        isExpand ? 
        <Button picture="expand_less" onClickHandler={collapse}/>
        :
        <Button picture="expand_more" onClickHandler={expand}/>
    );
}

function CustomizationCatagoryBody({items, extractValue, setValue, addItem, updateItem, removeItem, forceRefresh})
{
    return (
        <div className="customization_catagory_body">
            {items.map(
                (item, i) => <EditCustomizationItem
                    item={item}
                    key={i}
                    setValue={setValue}
                    extractValue={extractValue}
                    updateItem={updateItem}
                    removeItem={removeItem}
                    forceRefresh={forceRefresh}
                />
            )}
            <AddCustomizationItem setValue={setValue} addItem={addItem} forceRefresh={forceRefresh}/>
        </div>
    );
}


export { CustomizationCatagory }