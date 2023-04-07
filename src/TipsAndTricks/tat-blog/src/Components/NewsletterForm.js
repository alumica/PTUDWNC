import { useState } from "react";
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPaperPlane } from "@fortawesome/free-solid-svg-icons";
import { FormControl } from "react-bootstrap";
import { addSubscribers } from "../Services/BlogRepository";

const NewsletterForm = () => {
    const [email, setEmail] = useState('');

    const handleSubmit = (e) => {
        e.preventDefault();
        window.location = `/blog/newsletter/subscribe?email=${email}`;
        
        addSubscribers(email).then((data) => {
            if (data) {
              setPost(data);
            } 
            else
              setPost({});
          });
    };
    
    return (
        <div className="mb-4">
            <h3 className='text-success mb-2'>
                Đăng ký nhận tin
            </h3>
            <Form method="get" onSubmit={handleSubmit}>
                <Form.Group className="input-group mb-3">
                    <FormControl 
                        type="text"
                        name="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        aria-label="Nhập email"
                        aria-describedby="btnSubcribe"
                        placeholder="Nhập email"/>
                    <Button
                        id="btnSubcribe"
                        variant="outline-secondary"
                        type="submit">
                        <FontAwesomeIcon icon={faPaperPlane}/>
                    </Button>
                </Form.Group>
            </Form>
        </div>
    );
}

export default NewsletterForm;