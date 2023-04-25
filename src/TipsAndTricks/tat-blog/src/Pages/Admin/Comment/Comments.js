import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import CommentFilterPane from "../../../Components/Admin/CommentFilterPane";
import { getComments } from "../../../Services/BlogRepository";
import Loading from "../../../Components/Loading";
import Table from "react-bootstrap/Table";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck, faTimes, faTrash } from "@fortawesome/free-solid-svg-icons";
import Button  from "react-bootstrap/Button";

const Comments = () => {
    const [commentsList, setCommentsList] = useState([]),
        [isVisibleLoading, setIsVisibleLoading] = useState(true);

    let { id } = useParams(),
        p = 1, ps = 10;
    useEffect(() => {
        document.title = 'Danh sách bình luận';

        getComments(ps, p).then(data => {
            if (data)
                setCommentsList(data.items);
            else
                setCommentsList([]);
            setIsVisibleLoading(false);  
        });
    }, [
        p, ps]);

    const handleSubmit = () => {
        var c = confirm('Bạn có muốn xóa bình luận này không ?')
            ? deleteCategory(id) : console.log("Hủy");
    }

    
    return (
        <>
            <h1>Danh sách bình luận</h1>
            <CommentFilterPane />
            {isVisibleLoading ? <Loading/> :
                <Table striped responsive bordered>
                    <thead>
                        <tr>
                            <th>Bài viết</th>
                            <th>Họ và tên</th>
                            <th>Nội dung bình luận</th>
                            <th>Ngày bình luận</th>
                            <th>Đã phê duyệt</th>
                            <th>Xóa</th>
                        </tr>
                    </thead>
                    <tbody>
                        {commentsList?.length > 0 ? commentsList.map((item, index) =>
                            <tr key={index}>
                                <td>
                                    <Link to={`/admin/comments/edit/${item.id}`}>
                                        {item.post.title}
                                    </Link>
                                    <p className="text-muted">{item.post.shorDescription}</p>
                                </td>
                                <td>{item.fullname}</td>
                                <td>{item.description}</td>
                                <td>{item.postedDate}</td>
                                
                                <td>{item.approved ? 
                                    <Button variant="success"><FontAwesomeIcon icon={faCheck} /></Button> : 
                                    <Button variant="danger"><FontAwesomeIcon icon={faTimes} /></Button> }</td>
                                <td><Button variant="danger" onClick={handleSubmit}><FontAwesomeIcon icon={faTrash} /></Button> </td>
                            </tr>
                            ) :
                                <tr>
                                    <td colSpan={6}>
                                        <h4 className="text-danger text-center">Không tìm thấy bình luận nào</h4>
                                    </td>
                                </tr>
                        }
                    </tbody>
                </Table>
            }
        </>
    ) 
}

export default Comments;