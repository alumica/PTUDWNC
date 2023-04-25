import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import AuthorFilterPane from "../../../Components/Admin/AuthorFilterPane";
import Loading from "../../../Components/Loading";
import Table from "react-bootstrap/Table";
import Button from "react-bootstrap/Button";
import { getAuthors } from "../../../Services/BlogRepository";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck, faTimes, faTrash } from "@fortawesome/free-solid-svg-icons";

const Authors = () => {
    const [authorsList, setAuthorsList] = useState([]),
        [isVisibleLoading, setIsVisibleLoading] = useState(true);

    let { id } = useParams(),
        p = 1, ps = 5;
    useEffect(() => {
        document.title = 'Danh sách tác giả';

        getAuthors('',ps, p).then(data => {
            if (data)
                setAuthorsList(data.items);
            else
                setAuthorsList([]);
            setIsVisibleLoading(false);  
        });
    }, [
        p, ps]);

    const handleSubmit = () => {
        var c = confirm('Bạn có muốn xóa tác giả này không ?')
            ? deleteCategory(id) : console.log("Hủy");
    }

    
    return (
        <>
            <h1>Danh sách chủ đề</h1>
            <AuthorFilterPane />
            {isVisibleLoading ? <Loading/> :
                <Table striped responsive bordered>
                    <thead>
                        <tr>
                            <th>Họ & tên</th>
                            <th>Email</th>
                            <th>Ngày tham gia</th>
                            <th>Số bài viết</th>
                            <th>Xóa</th>
                        </tr>
                    </thead>
                    <tbody>
                        {authorsList.length > 0 ? authorsList.map((item, index) =>
                            <tr key={index}>
                                <td>
                                    <Link to={`/admin/authors/edit/${item.id}`}>
                                        {item.fullName}
                                    </Link>
                                    <p className="text-muted">{item.notes}</p>
                                </td>
                                <td>{item.email}</td>
                                <td>{item.joinedDate}</td>
                                <td>{item.postCount}</td>
                                <td><Button variant="danger" onClick={handleSubmit}><FontAwesomeIcon icon={faTrash} /></Button> </td>
                            </tr>
                            ) :
                                <tr>
                                    <td colSpan={4}>
                                        <h4 className="text-danger text-center">Không tìm thấy tác giả nào</h4>
                                    </td>
                                </tr>
                        }
                    </tbody>
                </Table>
            }
        </>
    ) 
}

export default Authors;