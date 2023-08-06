import React from "react";
import { ContentHeader, ContentBody, ContentTitle} from "../Utils/Layout";

import { CustomizationCatagory } from "./CustomizationCatagory";
import { 
    GetAllCashflowType, 
    GetAllPaymentMethod, 
    AddPaymentMethod, 
    RemovePaymentMethod,
    EditPaymentMethod,
    AddType,
    RemoveType,
    EditType, 
} from "../Utils/AccountantDA";
import { Session } from "../Accounting/AccountingConstants";


function Customization (){
    return (
        <>
            <ContentHeader>
            <CustomizationHeader/>
            </ContentHeader>
            <ContentBody>
                <IncomeTypeCustomization/>
                <ExpenseTypeCustomization/>
                <PaymentMethodCustomization/>
            </ContentBody>
        </>
    );
} 

function CustomizationHeader(){
    return (
        <ContentTitle title="Customization"/>
    );
}

function IncomeTypeCustomization()
{
    return <CustomizationCatagory 
        title="Income Type"
        fetchData={forceRefresh => GetAllCashflowType(Session.INCOME, forceRefresh)}
        extractKey={(type) => type.typeId}
        extractValue={(type) => type.typeName}
        setValue={(type, value) => type.typeName = value}
        addItem={(type, finishHandler) => AddType(Session.INCOME, type, finishHandler)}
        removeItem={(type, finishHandler) => RemoveType(Session.INCOME, type, finishHandler)}
        updateItem={(type, finishHandler) => EditType(Session.INCOME, type, finishHandler)}
    />;
}

function ExpenseTypeCustomization()
{
    return <CustomizationCatagory 
        title="Expense Type"
        fetchData={forceRefresh => GetAllCashflowType(Session.EXPENDITURE, forceRefresh)}
        extractKey={(type) => type.typeId}
        extractValue={(type) => type.typeName}
        setValue={(type, value) => type.typeName = value}
        addItem={(type, finishHandler) => AddType(Session.EXPENDITURE, type, finishHandler)}
        removeItem={(type, finishHandler) => RemoveType(Session.EXPENDITURE, type, finishHandler)}
        updateItem={(type, finishHandler) => EditType(Session.EXPENDITURE, type, finishHandler)}
    />;
}

function PaymentMethodCustomization()
{
    return <CustomizationCatagory 
        title="Payment Method"
        fetchData={forceRefresh => GetAllPaymentMethod(forceRefresh)}
        extractKey={(type) => type.methodId}
        extractValue={(type) => type.methodName}
        setValue={(method, value) => method.methodName = value}
        addItem={AddPaymentMethod}
        removeItem={RemovePaymentMethod}
        updateItem={EditPaymentMethod}
    />;
}

export { Customization };