import { useEffect, useState } from "react";
import {
  getTotalPosts,
  getNumberPostsUnpublished,
  getNumberCategories,
  getNumberAuthors,
  getNumberNumberCommentsUnapproved,
  getNumberSubscribers,
  getNumberSubscribersToday
 } from "../../Services/BlogRepository";

const Index = () => {
    const [totalPosts, setTotalPosts] = useState(0),
        [numberPostsUnpublished, setNumberPostsUnpublished] = useState(0),
        [bumberCategories, setNumberCategories] = useState(0),
        [numberAuthors, setNumberAuthors] = useState(0),
        [numberCommentsUnapproved, setNumberCommentsUnapproved] = useState(0),
        [numberSubscribers, setNumberSubscribers] = useState(0),
        [numberSubscribersToday, setNumberSubscribersToday] = useState(0);
    useEffect(() => {
        document.title = 'Bảng điều khiển';
        getTotalPosts().then(data => {
          setTotalPosts(data);
        });
        getNumberPostsUnpublished().then(data => {
          setNumberPostsUnpublished(data);
        });
        getNumberCategories().then(data => {
          setNumberCategories(data);
        });
        getNumberAuthors().then(data => {
          setNumberAuthors(data);
        });
        getNumberNumberCommentsUnapproved().then(data => {
          setNumberCommentsUnapproved(data);
        });
        getNumberSubscribers().then(data => {
          setNumberSubscribers(data);
        });
        getNumberPostsUnpublished().then(data => {
          setNumberSubscribersToday(data);
        });
    });

    return (
      <div className="d-flex flex-row mw-75 flex-wrap mt-3">
        <div className="card text-black mb-3 me-5" style={{ minWidth: 18 + 'rem' }}>
          <div className="card-header text-white bg-primary">
            Tổng số bài viết
          </div>
          <div className="card-body border-primary border">
            <h4 className="card-title">{totalPosts}</h4>
          </div>
        </div>

        <div
          className="card text-black mb-3 me-5"
          style={{ minWidth: 18 + 'rem' }}>
            <div className="card-header text-white bg-danger">
                Số bài viết chưa xuất bản
            </div>
            <div className="card-body border-danger border">
                <h4 className="card-title">{numberPostsUnpublished}</h4>
            </div>
        </div>

        <div
          className="card text-black mb-3 me-5"
          style={{ minWidth: 18 + 'rem' }}>
          <div className="card-header text-white bg-warning">
            Số lượng chủ đề
          </div>
          <div className="card-body border-warning border">
            <h4 className="card-title">{bumberCategories}</h4>
          </div>
        </div>

        <div
          className="card text-black mb-3 me-5"
          style={{ minWidth: 18 + 'rem' }}>
            <div className="card-header text-white bg-info">Số lượng tác giả</div>
            <div className="card-body border-info border">
                <h4 className="card-title">{numberAuthors}</h4>
            </div>
        </div>

        <div
          className="card text-black mb-3 me-5"
          style={{ minWidth: 18 + 'rem' }}>
            <div className="card-header text-white bg-dark">
                Số lượng bình luận chờ duyệt
            </div>
            <div className="card-body border-dark border">
                <h4 className="card-title">{numberCommentsUnapproved}</h4>
            </div>
        </div>

        <div
            className="card text-black mb-3 me-5"
            style={{ minWidth: 18 + 'rem' }}>
            <div className="card-header text-black bg-light">
                Số lượng người theo dõi
            </div>
            <div className="card-body border-light border">
                <h4 className="card-title">{numberSubscribers}</h4>
            </div>
        </div>

        <div
          className="card text-black mb-3 me-5"
          style={{ minWidth: 18 + 'rem' }}>
            <div className="card-header text-white bg-success">
                Số lượng người theo dõi mới
            </div>
            <div className="card-body border-success border">
                <h4 className="card-title">{numberSubscribersToday}</h4>
            </div>
        </div>
      </div>
  );
};
export default Index;
