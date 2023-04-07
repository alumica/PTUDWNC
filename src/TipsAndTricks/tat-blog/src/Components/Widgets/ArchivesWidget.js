import { useState, useEffect } from 'react';
import ListGroup from 'react-bootstrap/ListGroup';
import { Link } from 'react-router-dom';
import { getArchives } from '../../Services/Widget';

const ArchivesWidget = () => {
    const [postList, setPostList] = useState([]);

    useEffect(() => {
        getArchives(5).then(data => {
            if (data)
                setPostList(data);
            else
                setPostList([]);
        });
    }, []);

    return (
        <div className='mb-4'>
            <h3 className='text-success mb-2'>
                Kho lưu trữ
            </h3>
            {postList.length > 0 && 
                <ListGroup>
                    {postList.map((item, index) => {
                        return (
                            <ListGroup.Item key={index}>
                                <Link to={`/blog/archives/${item.year}/${item.month}`}
                                    title={item.description}
                                    key={index}>
                                        {new Date(item.year, item.month-1, 1).toLocaleString('en-US', {month: 'long'})} {item.year} ({item.postCount})
                                </Link>
                            </ListGroup.Item>
                        );
                    })}
                </ListGroup>    
            }
        </div>
    );
}

export default ArchivesWidget;