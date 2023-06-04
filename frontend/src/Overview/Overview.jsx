import React from "react";
import { ContentHeader, ContentBody, ContentFooter, ContentTitle} from "../Utils/Layout";

function Overview (){
    return (
        <>
            <ContentHeader>
                <OverviewHeader/>
                </ContentHeader>
            <ContentBody>
            </ContentBody>
            <ContentFooter>
            </ContentFooter>
        </>
    );
}

function OverviewHeader(){
    return (
        <>
            <ContentTitle title="Overview"/>
        </>
    );
}

export { Overview };