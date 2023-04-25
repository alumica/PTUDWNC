import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { getTags } from "../../../Services/BlogRepository";
import TagFilterPane from "../../../Components/Admin/TagFilterPane";
import Loading from "../../../Components/Loading";
import Table from "react-bootstrap/Table";
import Button from "react-bootstrap/Button";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck, faTimes, faTrash } from "@fortawesome/free-solid-svg-icons";

const Tags = () => {
    const [tagsList, setTagsList] = useState([]),
        [isVisibleLoading, setIsVisibleLoading] = useState(true);

    let { id } = useParams(),
        p = 1, ps = 10;
    useEffect(() => {
        document.title = 'Danh sách thẻ';

        getTags(true, ps, p).then(data => {
            if (data)
                setTagsList(data.items);
            else
                setTagsList([]);
            setIsVisibleLoading(false);  
        });
    }, [true,
        p, ps]);

    const handleSubmit = () => {
        var c = confirm('Bạn có muốn xóa thẻ này không ?')
            ? deleteCategory(id) : console.log("Hủy");
    }

    
    return (
        <>
            <h1>Danh sách chủ đề</h1>
            <TagFilterPane />
            {isVisibleLoading ? <Loading/> :
                <Table striped responsive bordered>
                    <thead>
                        <tr>
                            <th>Tên thẻ</th>
                            <th>Số bài viết</th>
                            <th>Xóa</th>
                        </tr>
                    </thead>
                    <tbody>
                        {tagsList?.length > 0 ? tagsList.map((item, index) =>
                            <tr key={index}>
                                <td>
                                    <Link to={`/admin/categories/edit/${item.id}`}>
                                        {item.name}
                                    </Link>
                                    <p className="text-muted">{item.description}</p>
                                </td>
                                <td>{item.postCount}</td>
                                
                            
                                <td><Button variant="danger" onClick={handleSubmit}><FontAwesomeIcon icon={faTrash} /></Button> </td>
                            </tr>
                            ) :
                                <tr>
                                    <td colSpan={4}>
                                        <h4 className="text-danger text-center">Không tìm thấy thẻ nào</h4>
                                    </td>
                                </tr>
                        }
                    </tbody>
                </Table>
            }
        </>
    ) 
}

export default Tags;