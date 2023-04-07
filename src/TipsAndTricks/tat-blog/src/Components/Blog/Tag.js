import React, { useEffect, useState } from "react";
import { useLocation, useParams } from "react-router-dom";
import PostItem from '../PostItem';
import Pager from '../Pager';
import { getPostsByTagSlug } from '../../Services/BlogRepository';

const Tag = () => {
    const { slug } = useParams();

    const [postList, setPostList] = useState([]);
    const [metadata, setMetadata] = useState({});

    function useQuery () {
        const { search } = useLocation();
        return React.useMemo(() => new URLSearchParams(search), [search]);
    }

    let query = useQuery(),
        k = query.get('k') ?? '',
        p = query.get('p') ?? 1,
        ps = query.get('ps') ?? 5;

    useEffect(() => {
        document.title = 'Thẻ ';

        getPostsByTagSlug(slug, ps, p).then(data => {
            if (data) {
                setPostList(data.items);
                setMetadata(data.metadata);
            }
            else
                setPostList([]);
        })
    }, [slug, p, ps]);

    

    useEffect(() => {
        window.scrollTo(0, 0);
    }, [postList]);
                        
    if (postList.length > 0)
        return (
            <div>
                {postList.map((item, index) => {
                    return (
                        <div className="m-4">
                            <PostItem postItem={item} key={index}/>
                        </div>
                        
                    );
                })};
                <Pager postquery={{ 'keyword': k}} metadata={metadata}></Pager>
            </div>
        )
    else return (
        <></>
    );  
}

export default Tag;