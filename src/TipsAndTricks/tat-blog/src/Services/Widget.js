import { get_api } from "./Method";

export function getCategories(isPaged = false, pageSize = 10, pageNumber = 1, sortColumn = '', sortOrder = '') {
    let url = new URL('https://localhost:7171/api/categories');
    url.searchParams.append('IsPaged', isPaged);
    sortColumn !== '' && url.searchParams.append('SortColumn', sortColumn);
    sortOrder !== '' && url.searchParams.append('SortOrder', sortOrder);
    url.searchParams.append('PageSize', pageSize);
    url.searchParams.append('PageNumber', pageNumber);
    return get_api(url.href);
}

export function getFeaturedPosts(limit) {
    return get_api(`https://localhost:7171/api/posts/featured/${limit}`);
}

export function getRandomPosts(limit) {
    return get_api(`https://localhost:7171/api/posts/random/${limit}`);
}

export async function getTagCloud() {
    return get_api(`https://localhost:7171/api/tags?PageSize=1000&PageNumber=1`);
}

export async function getBestAuthors(limit) {
    return get_api(`https://localhost:7171/api/authors/best/${limit}`);
}

export async function getArchives(limit) {
    return get_api(`https://localhost:7171/api/posts/archives/${limit}`);
}