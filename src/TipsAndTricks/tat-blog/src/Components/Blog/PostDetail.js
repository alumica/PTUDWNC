import React, { useEffect, useState } from "react";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import TagList from "../TagList";
import { FormControl } from "react-bootstrap";
import { Link, useParams } from "react-router-dom";
import { getPost, getCommentsByPost } from "../../Services/BlogRepository";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faFacebookSquare,
  faTwitterSquare,
} from "@fortawesome/free-brands-svg-icons";

const PostDetail = () => {
  const [fullName, setFullName] = useState(""),
    [description, setDescription] = useState(""),
    [post, setPost] = useState({}),
    [author, setAuthor] = useState({}),
    [commentList, setCommentList] = useState([]);
    
  const { slug } = useParams();

  const handleSubmit = (e) => {
    e.preventDefault();
    let postedDate = new Date(post.postedDate);
    // window.location = `/blog/post/${postedDate.getFullYear()}/${postedDate.getMonth()}/${postedDate.getDay()}/${post.urlSlug}/comment?fn=${fullName}&d=${description}`;
    fetch("https://localhost:7171/api/comments", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        fullName: fullName,
        gender: true,
        approved: false,
        description: description,
        postId: post.id,
      }),
    });
  };

  useEffect(() => {
    getPost(slug).then((data) => {
      if (data) {
        setPost(data);
        setAuthor(data.author);
        getCommentsByPost(data.id).then((cmts) => {
          if (cmts) {
            setCommentList(cmts);
          } else setCommentList([]);
        });
      } 
      else
        setPost({});
    });
    document.title = `Bài viết - ${post.title}`;
  }, []);

  useEffect(() => {
    
    document.title = `Bài viết - ${post.title}`;
  }, []);


  return (
    <div className="p-4">
      <article className="blog-post">
        <h2 className="blog-post-title mb-1">{post.title}</h2>
        <p className="blog-post-meta">
          {post.postedDate} by 
          <Link to={`/blog/author/${author.urlSlug}`}> {author.fullName}</Link>
        </p>

        <div className="d-inline p-2">
          <Link
            target="_blank"
            to={`https://www.facebook.com/sharer/sharer.php?u=${window.location}`}
          >
            <FontAwesomeIcon fontSize={22} icon={faFacebookSquare} />
          </Link>
        </div>

        <div className="d-inline p-2">
          <Link
            target="_blank"
            to={`https://twitter.com/intent/tweet?text=${window.location}`}
          >
            <FontAwesomeIcon fontSize={22} icon={faTwitterSquare} />
          </Link>
        </div>

        <p>{post.shortDescription}</p>
        <hr />
        <p>{post.description}</p>
        <div className="d-inline mb-4">
          <div>Tag:</div>
          <TagList tagList={post.tags}></TagList>
        </div>
      </article>

      <div className="border border-light w-75 p-3">
        <h5>Bình luận</h5>
        <Form method="post" onSubmit={handleSubmit} className="row p-5">
          <Form.Group className="mb-3 col">
            <FormControl
              type="text"
              name="fullName"
              value={fullName}
              onChange={(e) => setFullName(e.target.value)}
              aria-label="Nhập họ tên"
              aria-describedby="btnCommentPost"
              placeholder="Nhập họ tên"
            />
          </Form.Group>
          <Form.Group className="w-100"></Form.Group>
          <Form.Group className="mb-3">
            <FormControl
              as="textarea"
              rows={4}
              type="text"
              name="description"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              aria-label="Nhập nội dung bình luận"
              aria-describedby="btnCommentPost"
              placeholder="Nhập nội dung bình luận"
            />
          </Form.Group>
          <Button id="btnCommentPost" variant="primary" type="submit">
            Gửi
          </Button>
        </Form>

        <hr />
      </div>
        {commentList.map((item) => {
          if (item.approved)
            return (
              <div className="be-comment">
                <div className="be-comment-content">
                  <span className="be-comment-name">
                    <h4>{item.fullName}</h4>
                  </span>
                  <span className="be-comment-time">
                    <i className="fa fa-clock-o"></i>
                    {item.postedDate}
                  </span>

                  <p className="be-comment-text">{item.description}</p>
                </div>
              </div>
            );
        })}
        ;
      </div>
  );
};

export default PostDetail;
