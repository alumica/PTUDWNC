import axios from "axios";

export async function getCategories() {
    try {
        const response = await axios.get(`https://localhost:7171/api/categories?PageSize=100&PageNumber=1&isPaged=false`);
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

export async function getFeaturedPosts(limit) {
    try {
        const response = await axios.get(`https://localhost:7171/api/posts/featured/${limit}`);
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

export async function getRandomPosts(limit) {
    try {
        const response = await axios.get(`https://localhost:7171/api/posts/random/${limit}`);
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

export async function getTagCloud() {
    try {
        const response = await axios.get(`https://localhost:7171/api/tags?PageSize=1000&PageNumber=1`);
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

export async function getBestAuthors(limit) {
    try {
        const response = await axios.get(`https://localhost:7171/api/authors/best/${limit}`);
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

export async function getArchives(limit) {
    try {
        const response = await axios.get(`https://localhost:7171/api/posts/archives/${limit}`);
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