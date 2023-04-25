import React, { useEffect, useState } from "react";
import Table from "react-bootstrap/Table";
import { Link, useParams } from "react-router-dom";
import { getPostsFilter, togglePublished } from "../../../Services/BlogRepository";
import Loading from "../../../Components/Loading";
import { isInteger } from "../../../Utils/Utils";
import PostFilterPane from "../../../Components/Admin/PostFilterPane";
import { useSelector } from "react-redux";
import Button from "react-bootstrap/Button";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck, faTimes } from "@fortawesome/free-solid-svg-icons";
import Pager from "../../../Pages/Admin/Post/Pager";

const Posts = () => {
    const [postsList, setPostsList] = useState([]),
        [isVisibleLoading, setIsVisibleLoading] = useState(true),
        postFilter = useSelector(state => state.postFilter),
        [metadata, setMetadata] = useState({});
    
    let { id } = useParams(),
        p = 1, ps = 5;

    useEffect(() => {
        document.title = 'Danh sách bài viết';

        getPostsFilter(postFilter.keyword,
            postFilter.authorId,
            postFilter.categoryId,
            postFilter.year,
            postFilter.month,
            ps, p).then(data => {
            if (data) {
                setPostsList(data.items);
                setMetadata(data.metadata);
            }
            else
                setPostsList([]);

            setIsVisibleLoading(false);
        });
    }, [
        postFilter.keyword,
        postFilter.authorId,
        postFilter.categoryId,
        postFilter.year,
        postFilter.month,
        p, ps]);

    useEffect(() => {
        window.scrollTo(0, 0);
    }, [postsList]);

    return (
        <div>
            <h1>Danh sách bài viết</h1>
            <PostFilterPane />
            {isVisibleLoading ? <Loading/> :
                <Table striped responsive bordered>
                    <thead>
                        <tr>
                            <th>Tiêu đề</th>
                            <th>Tác giả</th>
                            <th>Chủ đề</th>
                            <th>Xuất bản</th>
                        </tr>
                    </thead>
                    <tbody>
                        {postsList.length > 0 ? postsList.map((item, index) =>
                            <tr key={index}>
                                <td>
                                    <Link to={`/admin/posts/edit/${item.id}`}>
                                        {item.title}
                                    </Link>
                                    <p className="text-muted">{item.shortDescription}</p>
                                </td>
                                <td>{item.author.fullName}</td>
                                <td>{item.category.name}</td>
                                <td>{item.published ? 
                                    <Button variant="success"><FontAwesomeIcon icon={faCheck} /></Button> : 
                                    <Button variant="danger"><FontAwesomeIcon icon={faTimes} /></Button> }</td>
                            </tr>
                            ) :
                                <tr>
                                    <td colSpan={4}>
                                        <h4 className="text-danger text-center">Không tìm thấy bài viết nào</h4>
                                    </td>
                                </tr>
                        }
                    </tbody>
                </Table>
            }
            <Pager postquery={{}} metadata={metadata}/>
        </div>
    )
}

export default Posts;