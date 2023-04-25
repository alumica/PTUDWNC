import { useState, useEffect } from "react";
import { deleteCategory, getCategories } from "../../../Services/BlogRepository";
import { Link, useParams } from "react-router-dom";
import CategoryFilterPane from "../../../Components/Admin/CategoryFilterPane";
import Loading from "../../../Components/Loading";
import Table from "react-bootstrap/Table";
import Button from "react-bootstrap/Button";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck, faTimes, faTrash } from "@fortawesome/free-solid-svg-icons";


const Categories = () => {
    const [categoriesList, setCategoriesList] = useState([]),
        [isVisibleLoading, setIsVisibleLoading] = useState(true);

    let { id } = useParams(),
        p = 1, ps = 10;
    useEffect(() => {
        document.title = 'Danh sách chủ đề';

        getCategories(true, ps, p).then(data => {
            if (data)
                setCategoriesList(data.items);
            else
                setCategoriesList([]);
            setIsVisibleLoading(false);  
        });
    }, [
        true,
        p, ps]);

    const handleSubmit = () => {
        var c = confirm('Bạn có muốn xóa chủ đề này không ?')
            ? deleteCategory(id) : console.log("Hủy");
    }

    
    return (
        <>
            <h1>Danh sách chủ đề</h1>
            <CategoryFilterPane />
            {isVisibleLoading ? <Loading/> :
                <Table striped responsive bordered>
                    <thead>
                        <tr>
                            <th>Tên chủ đề</th>
                            <th>Số bài viết</th>
                            <th>Hiển thị trên menu</th>
                            <th>Xóa</th>
                        </tr>
                    </thead>
                    <tbody>
                        {categoriesList.length > 0 ? categoriesList.map((item, index) =>
                            <tr key={index}>
                                <td>
                                    <Link to={`/admin/categories/edit/${item.id}`}>
                                        {item.name}
                                    </Link>
                                    <p className="text-muted">{item.description}</p>
                                </td>
                                <td>{item.postCount}</td>
                                
                                <td>{item.showOnMenu ? 
                                    <Button variant="success"><FontAwesomeIcon icon={faCheck} /></Button> : 
                                    <Button variant="danger"><FontAwesomeIcon icon={faTimes} /></Button> }</td>
                                <td><Button variant="danger" onClick={handleSubmit}><FontAwesomeIcon icon={faTrash} /></Button> </td>
                            </tr>
                            ) :
                                <tr>
                                    <td colSpan={4}>
                                        <h4 className="text-danger text-center">Không tìm thấy chủ đề nào</h4>
                                    </td>
                                </tr>
                        }
                    </tbody>
                </Table>
            }
        </>
    ) 
}

export default Categories;