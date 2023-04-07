import './App.css';
import Navbar from './Components/Navbar';
import Sidebar from './Components/Sidebar';
import Footer from './Components/Footer';
import Layout from './Pages/Layout';
import Index from './Pages/Index';
import About from './Pages/About';
import Contact from './Pages/Contact';
import RSS from './Pages/RSS';
import PostDetail from './Components/Blog/PostDetail';
import Author from './Components/Blog/Author';
import Category from './Components/Blog/Category';
import Archives from './Components/Blog/Archives';
import {
    BrowserRouter as Router,
    Routes,
    Route,
} from 'react-router-dom';
import Tag from './Components/Blog/Tag';


function App() {
    return (
        <div>
            <Router>
            <Navbar />
            <div className='container-fluid'>
              <div className='row'>
                <div className='col-9'>
                  <Routes>
                    <Route path='/' element={<Layout/>}>
                      <Route path='/' element={<Index/>}/>
                      <Route path='blog' element={<Index/>}>
                      </Route>
                      
                      <Route path='blog/post/:year/:month/:day/:slug' element={<PostDetail />} />
                      <Route path='blog/archives/:year/:month' element={<Archives />} />
                      <Route path='blog/author/:slug' element={<Author/>} />
                      <Route path='blog/category/:slug' element={<Category/>} />
                      <Route path='blog/tag/:slug' element={<Tag/>} />
                      <Route path='blog/Contact' element={<Contact/>}/>
                      <Route path='blog/About' element={<About/>}/>
                      <Route path='blog/RSS' element={<RSS/>}/>
                    </Route>
                  </Routes>
                </div>
                <div className='col-3 border-start'>
                  <Sidebar/>
                </div>
              </div>
            </div>
            <Footer/>
            </Router>
        </div>
    );
}

export default App;