import { useState, useEffect } from 'react';
import ListGroup from 'react-bootstrap/ListGroup';
import { Link } from 'react-router-dom';
import { getFeaturedPosts } from '../../Services/Widget';

const FeaturedPostsWidget = () => {
    const [postList, setPostList] = useState([]);

    useEffect(() => {
        getFeaturedPosts(3).then(data => {
            if (data)
                setPostList(data);
            else
                setPostList([]);
        });
    }, []);

    return (
        <div className='mb-4'>
            <h3 className='text-success mb-2'>
                Top 3 bài viết
            </h3>
            {postList.length > 0 && 
                <ListGroup>
                    {postList.map((item, index) => {
                        let postedDate = new Date(item.postedDate);
                        return (
                            <ListGroup.Item key={index}>
                                <Link to={`/blog/post/${postedDate.getFullYear()}/${postedDate.getMonth()}/${postedDate.getDay()}/${item.urlSlug}`}
                                    title={item.description}
                                    key={index}>
                                        {item.title}
                                </Link>
                            </ListGroup.Item>
                        );
                    })}
                </ListGroup>    
            }
        </div>
    );
}

export default FeaturedPostsWidget;