import { useState, useEffect } from "react";

const HTTPMETHOD = {
    GET: "GET",
    POST: "POST",
    PUT: "PUT",
    DELETE: "DELETE"
}

// Won't fetch if url is empty
function useFetchData(url, httpMethod, paramString)
{
    const [data, setData] = useState({});
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(true);

    if (httpMethod === HTTPMETHOD.GET) paramString = undefined;
    
    useEffect(() => {
        async function init()
        {   
            try {
                const response = await simpleFetch(url, httpMethod, paramString);
                
                if (response.ok) {
                    let json = await response.json();
                    json = json ? json : {};
                    setData(json);
                }else{
                    throw response;
                }
            }catch (e) {
                setError(e);
            }finally{
                setLoading(false);
            }
        }
        init();
    }, [url, httpMethod, paramString]);

    return { data, error, loading };
}


async function simpleFetch(url, httpMethod, paramString)
{
    return await fetch(
        url, 
        {
            method: httpMethod,
            headers: {
                'Content-Type': 'application/json',
                "Origin": "http://localhost"
            },
            body: paramString,
        }
    );
}

export { useFetchData, simpleFetch, HTTPMETHOD };