import { Link } from "react-router-dom";
import Form from "react-bootstrap/Form";

const TagFilterPane = () => {
    return (
        <Form
            className="row gy-2 gx-3 align-items-center p-2">
            <Form.Group className="col-auto">
                <Link to="/admin/categories/edit" className="btn btn-success ms-2">Thêm mới</Link>
            </Form.Group>
        </Form>
    );
}

export default TagFilterPane;