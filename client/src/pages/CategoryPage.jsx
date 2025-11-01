import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { Container, Grid, Typography, Box, CircularProgress, Alert, Breadcrumbs, Link } from '@mui/material';
import { NavigateNext } from '@mui/icons-material';
import { categoriesApi, priceComparisonApi } from '../services/api';
import ProductCard from '../components/ProductCard';

const CategoryPage = () => {
  const { id } = useParams();
  const [category, setCategory] = useState(null);
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const categoryRes = await categoriesApi.getById(id);
        setCategory(categoryRes.data);

        const productsRes = await priceComparisonApi.getBestPricesForCategory(id);
        setProducts(productsRes.data);
      } catch (err) {
        setError('Failed to load category data. Please try again later.');
        console.error('Error fetching category:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id]);

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="60vh">
        <CircularProgress />
      </Box>
    );
  }

  if (error || !category) {
    return (
      <Container sx={{ mt: 4 }}>
        <Alert severity="error">{error || 'Category not found'}</Alert>
      </Container>
    );
  }

  return (
    <Container sx={{ mt: 4, mb: 4 }}>
      <Breadcrumbs separator={<NavigateNext fontSize="small" />} sx={{ mb: 3 }}>
        <Link underline="hover" color="inherit" href="/">
          Home
        </Link>
        <Link underline="hover" color="inherit" href="/categories">
          Categories
        </Link>
        <Typography color="text.primary">{category.name}</Typography>
      </Breadcrumbs>

      <Typography variant="h3" gutterBottom fontWeight="bold">
        {category.name}
      </Typography>

      {category.description && (
        <Typography variant="body1" color="text.secondary" paragraph>
          {category.description}
        </Typography>
      )}

      {category.subCategories && category.subCategories.length > 0 && (
        <Box sx={{ mb: 4 }}>
          <Typography variant="h5" gutterBottom fontWeight="bold">
            Subcategories
          </Typography>
          <Grid container spacing={2}>
            {category.subCategories.map((subCat) => (
              <Grid item xs={12} sm={6} md={4} key={subCat.id}>
                <Box
                  sx={{
                    p: 2,
                    bgcolor: 'background.paper',
                    borderRadius: 2,
                    textAlign: 'center',
                    cursor: 'pointer',
                    transition: 'all 0.3s',
                    '&:hover': {
                      bgcolor: 'primary.light',
                      color: 'white',
                      transform: 'translateY(-3px)',
                      boxShadow: 3
                    }
                  }}
                  onClick={() => window.location.href = `/category/${subCat.id}`}
                >
                  <Typography variant="h6">
                    {subCat.name}
                  </Typography>
                </Box>
              </Grid>
            ))}
          </Grid>
        </Box>
      )}

      <Typography variant="h5" gutterBottom fontWeight="bold" sx={{ mt: 4 }}>
        Products ({products.length})
      </Typography>

      {products.length === 0 ? (
        <Alert severity="info">No products found in this category.</Alert>
      ) : (
        <Grid container spacing={3}>
          {products.map((product) => (
            <Grid item xs={12} sm={6} md={4} lg={3} key={product.product.id}>
              <ProductCard product={product.product} bestPrice={product} />
            </Grid>
          ))}
        </Grid>
      )}
    </Container>
  );
};

export default CategoryPage;
