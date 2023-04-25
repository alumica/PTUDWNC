import "./App.css";
import Footer from "./Components/Footer";
import Layout from "./Pages/Layout";
import Index from "./Pages/Index";
import About from "./Pages/About";
import Contact from "./Pages/Contact";
import RSS from "./Pages/RSS";
import PostDetail from "./Components/Blog/PostDetail";
import Author from "./Components/Blog/Author";
import Category from "./Components/Blog/Category";
import Archives from "./Components/Blog/Archives";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Tag from "./Components/Blog/Tag";
import AdminLayout from "./Pages/Admin/Layout";
import * as AdminIndex from "./Pages/Admin/Index";
import NotFound from "./Pages/NotFound";
import BadRequest from "./Pages/BadRequest";
import Posts from "./Pages/Admin/Post/Posts";
import Edit from "./Pages/Admin/Post/Edit";
import Categories from "./Pages/Admin/Category/Categories";
import Authors from "./Pages/Admin/Author/Authors";
import Tags from "./Pages/Admin/Tag/Tags";
import Comments from "./Pages/Admin/Comment/Comments";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route path="/" element={<Index />} />
          <Route path="blog" element={<Index />} />

          <Route
            path="blog/post/:year/:month/:day/:slug"
            element={<PostDetail />}/>
          <Route path="blog/archives/:year/:month" element={<Archives />} />
          <Route path="blog/author/:slug" element={<Author />} />
          <Route path="blog/category/:slug" element={<Category />} />
          <Route path="blog/tag/:slug" element={<Tag />} />
          <Route path="blog/Contact" element={<Contact />} />
          <Route path="blog/About" element={<About />} />
          <Route path="blog/RSS" element={<RSS />} />
          {/* <Route path="/400" element={<BadRequest />} /> */}
          <Route path="*" element={<NotFound />} />
        </Route>
        <Route path="/admin" element={<AdminLayout />}>
          <Route path="/admin" element={<AdminIndex.default />}/>
          <Route path="/admin/authors" element={<Authors/>}/>
          <Route path="/admin/categories" element={<Categories />}/>
          <Route path="/admin/comments" element={<Comments />}/>
          <Route path="/admin/posts" element={<Posts />}/>
          <Route path="/admin/posts/edit" element={<Edit />}/>
          <Route path="/admin/posts/edit/:id" element={<Edit />}/>
          <Route path="/admin/tags" element={<Tags />}/>
        </Route>
      </Routes>
      
      <Footer />
    </Router>
  );
}

export default App;
