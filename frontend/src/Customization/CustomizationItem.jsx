import React, { useRef, useState } from "react";
import {lengthValidator} from "../Utils/Validator";

import "./Customization.css";

function EditCustomizationItem({item, extractValue, setValue, updateItem, removeItem, forceRefresh})
{
    const [editing, setEditing] = useState(false);
    const [text, setText] = useState(extractValue(item));
    const edit_customization_item = useRef(null);

    const changeText = (e) => 
    {
        let value = e.target.value;
        value = lengthValidator(20)(value);
        setText(value);
    }
    const abortChange = (e) =>
    {
        if (edit_customization_item.current.contains(e.relatedTarget)) return;
        setText(extractValue(item));
        setEditing(false);
    }
    const updateItemWrapper = () => {
        setValue(item, text);
        updateItem(item, forceRefresh);
    }
    const removeItemWrapper = () => {
        removeItem(item, forceRefresh);
    }

    return (
        <div className="customization_item customization_item_colored" ref={edit_customization_item} onBlur={abortChange}>
            <input type="text" name="item_name" id="item_name" value={text} 
                style={{width:(text.length + 1) + "ch"}}
                onFocus={() => setEditing(true)} 
                onChange={changeText} 
            />
            {editing ? 
            (
                <>
                    <i className="material-symbols-rounded" tabIndex="1" onClick={updateItemWrapper}>done</i>
                    <i className="material-symbols-rounded" tabIndex="1" onClick={removeItemWrapper}>delete</i>
                </>
            ) 
            : 
            (
                <i className="material-symbols-rounded" onClick={() => setEditing(true)}>edit</i>
            )}
        </div>
    );
}

function AddCustomizationItem({setValue, addItem, forceRefresh})
{
    const [editing, setEditing] = useState(false);
    const [text, setText] = useState("");
    const add_customization_item = useRef(null);

    const changeText = (e) => 
    {
        let value = e.target.value;
        value = lengthValidator(20)(value);
        setText(value);
    }
    const abortChange = (e) =>
    {
        if (add_customization_item.current.contains(e.relatedTarget)) return;
        setText("");
        setEditing(false);
    }
    const addItemWrapper = () => {
        let item = {};
        setValue(item, text);
        addItem(item, forceRefresh);
    }

    return (
        <div className="customization_item customization_item_white" onBlur={abortChange} ref={add_customization_item}>
            <input type="text" name="item_name" id="item_name" value={editing ? text : "Add"} 
                style={{width: (editing ? text.length : "Add".length) + 1 +"ch"}}
                onFocus={() => setEditing(true)} 
                onChange={changeText} 
            />
            {editing ? 
            (
                <i className="material-symbols-rounded" tabIndex="1" onClick={addItemWrapper}>done</i>
            ) 
            : 
            (
                <i className="material-symbols-rounded" onClick={() => setEditing(true)}>add</i>
            )}
        </div>
    );
}


export {EditCustomizationItem, AddCustomizationItem}