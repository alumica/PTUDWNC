import React, { useEffect } from "react";

const Contact = () => {
    useEffect(() => {
        document.title = 'RSS Feed';
    }, []);

    return (
        <h1>
            Đây là RSS Feed
        </h1>
    )
}

export default Contact;