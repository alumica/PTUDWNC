import { useState, useEffect } from 'react';
import ListGroup from 'react-bootstrap/ListGroup';
import { Link } from 'react-router-dom';
import { getTagCloud } from '../../Services/Widget';
import TagList from '../TagList';

const TagCloudWidget = () => {
    const [tagList, setTagList] = useState([]);

    useEffect(() => {
        getTagCloud().then(data => {
            if (data)
                setTagList(data);
            else
                setTagList([]);
        });
    }, []);

    return (
        <div className='mb-4'>
            <h3 className='text-success mb-2'>
                Tag cloud
            </h3>
            {tagList.length > 0 && 
                <TagList tagList={tagList}/>   
            }
        </div>
    );
}

export default TagCloudWidget;