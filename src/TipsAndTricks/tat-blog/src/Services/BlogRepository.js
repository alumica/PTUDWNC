import axios from 'axios';

export async function getPosts(keyword = '', pageSize = 5, pageNumber = 1, sortColumn = '', sortOrder = '') {
    try {
        const response = await axios.get(`https://localhost:7171/api/posts/get-posts-filter?Keyword=${keyword}&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`);
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

export async function getPostsByAuthorSlug(slug = '', pageSize = 5, pageNumber = 1, sortColumn = '', sortOrder = '') {
    try {
        const response = await axios.get(`https://localhost:7171/api/authors/${slug}/posts?PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`);
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

export async function getPostsByCategorySlug(slug = '', pageSize = 5, pageNumber = 1, sortColumn = '', sortOrder = '') {
    try {
        const response = await axios.get(`https://localhost:7171/api/categories/${slug}/posts?PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`);
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

export async function getPostsByTagSlug(slug = '', pageSize = 5, pageNumber = 1, sortColumn = '', sortOrder = '') {
    try {
        const response = await axios.get(`https://localhost:7171/api/tags/${slug}/posts?PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`);
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

export async function getPost(slug) {
    try {
        const response = await axios.get(`https://localhost:7171/api/posts/byslug/${slug}`);
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
    try {
        const response = await axios.get(`https://localhost:7171/api/comments/bypost/${id}`);
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

export async function addSubscribers(email) {
    try {
        const response = await axios.post(`https://localhost:7171/api/subscribers/subscribe/${email}`);
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