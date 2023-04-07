import React, { useEffect, useState } from "react";
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';

const Contact = () => {
    const [fullName, setFullName] = useState(''),
        [email, setEmail] = useState(''),
        [subject, setSubject] = useState(''),
        [description, setDescription] = useState('');


    useEffect(() => {
        document.title = 'Liên hệ';

        
    }, []);

    const handleSubmit = (e) => {
        e.preventDefault();
        fetch("https://localhost:7171/api/contacts.", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(
            {
            "fullName": fullName,
            "email": email,
            "subject": subject,
            "description": description
            }),
        });
    }

    return (
        <div className="p-5">
            <h4>Liên hệ</h4>
    <Form method="post" onSubmit={handleSubmit}>
      <Form.Group className="mb-3 w-50" controlId="formBasicFullName">
        <Form.Label>Họ tên</Form.Label>
        <Form.Control type="text" placeholder="Nhập họ tên" />
      </Form.Group>
      <Form.Group className="mb-3 w-50" controlId="formBasicEmail">
        <Form.Label>Email</Form.Label>
        <Form.Control type="email" placeholder="Nhập email" />
      </Form.Group>
      <Form.Group className="mb-3 w-50" controlId="formBasicSubject">
        <Form.Label>Tiêu đề</Form.Label>
        <Form.Control
            placeholder="Nhập tiêu đề" 
            type="text"
            name="subject"
            value={subject}
            aria-label="Nhập nội dung"
            aria-describedby="btnContactPost"
            onChange={(e) => setSubject(e.target.value)}/>
      </Form.Group>
      <Form.Group className="mb-3 w-50">
        <Form.Label>Nội dung</Form.Label>
        <Form.Control 
            as="textarea" 
            rows={5} 
            placeholder="Nhập nội dung" 
            type="text"
            name="description"
            value={description}
            aria-label="Nhập nội dung"
            aria-describedby="btnContactPost"
            onChange={(e) => setDescription(e.target.value)}/>
      </Form.Group>

      <Button id="btnContactPost" variant="primary" type="submit">
        Gửi
      </Button>
    </Form>
        </div>
        
  );
}

export default Contact;