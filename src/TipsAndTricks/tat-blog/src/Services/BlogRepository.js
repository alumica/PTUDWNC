import axios from 'axios';
import { get_api, post_api, put_api, delete_api } from './Method';

export function getPostsFilter(keyword = '', authorId = '', categoryId = '', year = '', month = '', pageSize = 10, pageNumber = 1, sortColumn = '', sortOrder = '') {
    let url = new URL('https://localhost:7171/api/posts/get-posts-filter');
    keyword !== '' && url.searchParams.append('Keyword', keyword);
    authorId !== '' && url.searchParams.append('AuthorId', authorId);
    categoryId !== '' && url.searchParams.append('CategoryId', categoryId);
    year !== '' && url.searchParams.append('Year', year);
    month !== '' && url.searchParams.append('Month', month);
    sortColumn !== '' && url.searchParams.append('SortColumn', sortColumn);
    sortOrder !== '' && url.searchParams.append('SortOrder', sortOrder);
    url.searchParams.append('PageSize', pageSize);
    url.searchParams.append('PageNumber', pageNumber);
    return get_api(url.href);
}   

export function getPosts(
    keyword = '', 
    pageSize = 5, 
    pageNumber = 1, 
    sortColumn = '', 
    sortOrder = '') {
    return get_api(`https://localhost:7171/api/posts/get-posts-filter?Keyword=${keyword}&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`);
}

export function getAuthors(
    name = '', 
    pageSize = 5, 
    pageNumber = 1, 
    sortColumn = '', 
    sortOrder = '') {
    return get_api(`https://localhost:7171/api/authors?Name=${name}&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`);
}


export async function getFilter() {
    return get_api(`https://localhost:7171/api/posts/get-filter`)
}

export async function getPostsByAuthorSlug(slug = '', pageSize = 5, pageNumber = 1, sortColumn = '', sortOrder = '') {
    return get_api(`https://localhost:7171/api/authors/${slug}/posts?PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`);
}

export async function getPostsByCategorySlug(slug = '', pageSize = 5, pageNumber = 1, sortColumn = '', sortOrder = '') {
    return get_api(`https://localhost:7171/api/categories/${slug}/posts?PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`);
}

export async function getPostsByTagSlug(slug = '', pageSize = 5, pageNumber = 1, sortColumn = '', sortOrder = '') {
    return get_api(`https://localhost:7171/api/tags/${slug}/posts?PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`);
}

export async function getPost(slug) {
    return get_api(`https://localhost:7171/api/posts/byslug/${slug}`);
}

export async function getPostById(id = 0) {
    if (id > 0)
        return get_api(`https://localhost:7171/api/posts/${id}`);
    return null;
}

export function addOrUpdatePost(formData) {
    return post_api('https://localhost:7171/api/posts', formData);
}

export function togglePublished(id = 0) {
    if (id > 0)
        return put_api(`https://localhost:7171/api/posts/${id}/togglepublished`);
    return null;
}

export async function getArchives(year, month, pageSize = 10, pageNumber = 1) {
    try {
        const response = await axios.get(`https://localhost:7171/api/posts/get-posts-filter?Year=${year}&Month=${month}&PageSize=${pageSize}&PageNumber=${pageNumber}`);
        const data = response.data;
        
        if (data.isSuccess)
            return data.result;
        else
            return null;
    } catch (error) {
        console.log('Error', error.message);
        return null;
    }
}

export async function getCommentsByPost(id) {
    return get_api(`https://localhost:7171/api/comments/bypost/${id}`);
}

export async function getComments(pageSize = 10, pageNumber = 1, sortColumn = '', sortOrder = '') {
    let url = new URL('https://localhost:7171/api/comments');
    sortColumn !== '' && url.searchParams.append('SortColumn', sortColumn);
    sortOrder !== '' && url.searchParams.append('SortOrder', sortOrder);
    url.searchParams.append('PageSize', pageSize);
    url.searchParams.append('PageNumber', pageNumber);
    return get_api(url.href);
}

export async function addSubscribers(email) {
    return post_api(`https://localhost:7171/api/subscribers/subscribe/${email}`);
}

export async function getCategories(isPaged = false, pageSize = 10, pageNumber = 1, sortColumn = '', sortOrder = '') {
    let url = new URL('https://localhost:7171/api/categories');
    url.searchParams.append('IsPaged', isPaged);
    sortColumn !== '' && url.searchParams.append('SortColumn', sortColumn);
    sortOrder !== '' && url.searchParams.append('SortOrder', sortOrder);
    url.searchParams.append('PageSize', pageSize);
    url.searchParams.append('PageNumber', pageNumber);
    return get_api(url.href);
}

export async function deleteCategory(id = 0) {
    return delete_api(`https://localhost:7171/api/categories/${id}`);
}

export async function getTags(isPaged = false, pageSize = 10, pageNumber = 1, sortColumn = '', sortOrder = '') {
    let url = new URL('https://localhost:7171/api/tags');
    url.searchParams.append('IsPaged', isPaged);
    sortColumn !== '' && url.searchParams.append('SortColumn', sortColumn);
    sortOrder !== '' && url.searchParams.append('SortOrder', sortOrder);
    url.searchParams.append('PageSize', pageSize);
    url.searchParams.append('PageNumber', pageNumber);
    return get_api(url.href);
}



export async function getTotalPosts() {
    return get_api('https://localhost:7171/api/statistics/totalpost');
}

export async function getNumberPostsUnpublished() {
    return get_api('https://localhost:7171/api/statistics/postunpublished');
}

export async function getNumberCategories() {
    return get_api('https://localhost:7171/api/statistics/categories');
}

export async function getNumberAuthors() {
    return get_api('https://localhost:7171/api/statistics/authors');
}

export async function getNumberNumberCommentsUnapproved() {
    return get_api('https://localhost:7171/api/statistics/commentunapproved');
}

export async function getNumberSubscribers() {
    return get_api('https://localhost:7171/api/statistics/subscribers');
}

export async function getNumberSubscribersToday() {
    return get_api('https://localhost:7171/api/statistics/subscriberstoday');
}