import React, { useEffect } from "react";

const RSS = () => {
    useEffect(() => {
        document.title = 'RSS Feed';
    }, []);

    return (
        <h1>
            Đây là RSS Feed
        </h1>
    )
}

export default RSS;