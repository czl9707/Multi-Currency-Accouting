import { useState, useEffect } from "react";

const HTTPMETHOD = {
    GET: "GET",
    POST: "POST",
    PUT: "PUT",
    DELETE: "DELETE"
}

function useFetch(url, httpMethod, paramString)
{
    const [data, setData] = useState({});
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        async function init()
        {
            try {
                if (httpMethod === HTTPMETHOD.GET) paramString = undefined;
                const response = await fetch(url, 
                    {
                        method: httpMethod,
                        header: {
                            'Content-Type': 'application/json',
                            "Origin": "http://localhost"
                        },
                        body: paramString,
                    });
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
    }, []);

    return { data, error, loading };
}

export { useFetch, HTTPMETHOD };