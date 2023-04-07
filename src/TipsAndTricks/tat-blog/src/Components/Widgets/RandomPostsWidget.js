import { useState, useEffect } from 'react';
import ListGroup from 'react-bootstrap/ListGroup';
import { Link } from 'react-router-dom';
import { getRandomPosts } from '../../Services/Widget';

const RandomPostsWidget = () => {
    const [postList, setPostList] = useState([]);

    useEffect(() => {
        getRandomPosts(5).then(data => {
            if (data)
                setPostList(data);
            else
                setPostList([]);
        });
    }, []);

    return (
        <div className='mb-4'>
            <h3 className='text-success mb-2'>
                Top 5 bài viết ngẫu nhiên
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

export default RandomPostsWidget;