import { useState, useEffect } from 'react';
import { Container, Grid, Typography, Box, CircularProgress, Alert } from '@mui/material';
import { categoriesApi, priceComparisonApi } from '../services/api';
import ProductCard from '../components/ProductCard';

const HomePage = () => {
  const [categories, setCategories] = useState([]);
  const [bestDeals, setBestDeals] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const categoriesRes = await categoriesApi.getRoot();
        setCategories(categoriesRes.data);

        // Get best deals from the first category
        if (categoriesRes.data.length > 0) {
          const dealsRes = await priceComparisonApi.getBestPricesForCategory(categoriesRes.data[0].id);
          setBestDeals(dealsRes.data.slice(0, 8)); // Show top 8 deals
        }
      } catch (err) {
        setError('Failed to load data. Please try again later.');
        console.error('Error fetching data:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="60vh">
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return (
      <Container sx={{ mt: 4 }}>
        <Alert severity="error">{error}</Alert>
      </Container>
    );
  }

  return (
    <Container sx={{ mt: 4, mb: 4 }}>
      {/* Hero Section */}
      <Box sx={{ 
        textAlign: 'center', 
        mb: 6, 
        p: 4, 
        bgcolor: 'primary.main', 
        color: 'white', 
        borderRadius: 2 
      }}>
        <Typography variant="h3" gutterBottom fontWeight="bold">
          Find the Best Prices Worldwide
        </Typography>
        <Typography variant="h6">
          Compare prices across multiple platforms and countries to get the best deals
        </Typography>
      </Box>

      {/* Featured Categories */}
      <Typography variant="h4" gutterBottom fontWeight="bold" sx={{ mb: 3 }}>
        Browse Categories
      </Typography>
      <Grid container spacing={2} sx={{ mb: 6 }}>
        {categories.map((category) => (
          <Grid item xs={12} sm={6} md={4} key={category.id}>
            <Box
              sx={{
                p: 3,
                bgcolor: 'background.paper',
                borderRadius: 2,
                textAlign: 'center',
                cursor: 'pointer',
                transition: 'all 0.3s',
                '&:hover': {
                  bgcolor: 'primary.light',
                  color: 'white',
                  transform: 'translateY(-5px)',
                  boxShadow: 4
                }
              }}
              onClick={() => window.location.href = `/category/${category.id}`}
            >
              <Typography variant="h6" fontWeight="bold">
                {category.name}
              </Typography>
              {category.description && (
                <Typography variant="body2" sx={{ mt: 1, opacity: 0.8 }}>
                  {category.description}
                </Typography>
              )}
            </Box>
          </Grid>
        ))}
      </Grid>

      {/* Best Deals */}
      {bestDeals.length > 0 && (
        <>
          <Typography variant="h4" gutterBottom fontWeight="bold" sx={{ mb: 3 }}>
            Today's Best Deals
          </Typography>
          <Grid container spacing={3}>
            {bestDeals.map((deal) => (
              <Grid item xs={12} sm={6} md={4} lg={3} key={deal.product.id}>
                <ProductCard product={deal.product} bestPrice={deal} />
              </Grid>
            ))}
          </Grid>
        </>
      )}
    </Container>
  );
};

export default HomePage;
