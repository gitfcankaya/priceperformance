import axios from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const productsApi = {
  getAll: (categoryId) => {
    const params = categoryId ? { categoryId } : {};
    return api.get('/products', { params });
  },
  getById: (id) => api.get(`/products/${id}`),
  search: (query) => api.get('/products/search', { params: { query } }),
};

export const categoriesApi = {
  getAll: () => api.get('/categories'),
  getById: (id) => api.get(`/categories/${id}`),
  getRoot: () => api.get('/categories/root'),
};

export const priceComparisonApi = {
  getBestPrice: (productId) => api.get(`/pricecomparison/best-price/${productId}`),
  getBestPricesForCategory: (categoryId) => api.get(`/pricecomparison/best-prices-category/${categoryId}`),
  getPriceHistory: (productId, startDate) => {
    const params = startDate ? { startDate } : {};
    return api.get(`/pricecomparison/price-history/${productId}`, { params });
  },
};

export const platformsApi = {
  getAll: () => api.get('/platforms'),
  getById: (id) => api.get(`/platforms/${id}`),
};

export const countriesApi = {
  getAll: () => api.get('/countries'),
  getById: (id) => api.get(`/countries/${id}`),
};

export default api;
