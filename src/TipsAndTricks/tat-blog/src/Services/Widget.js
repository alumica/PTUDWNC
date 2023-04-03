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