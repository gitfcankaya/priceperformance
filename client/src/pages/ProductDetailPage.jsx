import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import {
  Container, Grid, Typography, Box, CircularProgress, Alert, Card, CardContent,
  Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Chip, Button
} from '@mui/material';
import { TrendingDown, Store, Public } from '@mui/icons-material';
import { productsApi, priceComparisonApi } from '../services/api';

const ProductDetailPage = () => {
  const { id } = useParams();
  const [product, setProduct] = useState(null);
  const [bestPrice, setBestPrice] = useState(null);
  const [priceHistory, setPriceHistory] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const [productRes, bestPriceRes, historyRes] = await Promise.all([
          productsApi.getById(id),
          priceComparisonApi.getBestPrice(id),
          priceComparisonApi.getPriceHistory(id)
        ]);

        setProduct(productRes.data);
        setBestPrice(bestPriceRes.data);
        setPriceHistory(historyRes.data);
      } catch (err) {
        setError('Failed to load product details. Please try again later.');
        console.error('Error fetching product:', err);
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

  if (error || !product) {
    return (
      <Container sx={{ mt: 4 }}>
        <Alert severity="error">{error || 'Product not found'}</Alert>
      </Container>
    );
  }

  const formatPrice = (price, currencySymbol) => {
    return `${currencySymbol}${price.toFixed(2)}`;
  };

  // Group prices by platform and country
  const latestPrices = priceHistory.reduce((acc, price) => {
    const key = `${price.platformId}-${price.countryId}`;
    if (!acc[key] || new Date(price.priceDate) > new Date(acc[key].priceDate)) {
      acc[key] = price;
    }
    return acc;
  }, {});

  const pricesList = Object.values(latestPrices).sort((a, b) => a.price - b.price);

  return (
    <Container sx={{ mt: 4, mb: 4 }}>
      <Grid container spacing={4}>
        <Grid item xs={12} md={5}>
          <Card>
            <img
              src={product.imageUrl || 'https://via.placeholder.com/500x500?text=No+Image'}
              alt={product.name}
              style={{ width: '100%', height: 'auto', objectFit: 'cover' }}
            />
          </Card>
        </Grid>

        <Grid item xs={12} md={7}>
          <Typography variant="h4" gutterBottom fontWeight="bold">
            {product.name}
          </Typography>

          {product.brand && (
            <Typography variant="h6" color="text.secondary" gutterBottom>
              Brand: {product.brand}
            </Typography>
          )}

          {product.category && (
            <Chip label={product.category.name} sx={{ mb: 2 }} />
          )}

          {bestPrice && (
            <Card sx={{ mb: 3, bgcolor: 'success.light', color: 'success.contrastText' }}>
              <CardContent>
                <Box display="flex" alignItems="center" justifyContent="space-between">
                  <Box>
                    <Typography variant="h5" fontWeight="bold">
                      Best Price: {formatPrice(bestPrice.bestPrice.price, bestPrice.bestPrice.country.currencySymbol)}
                    </Typography>
                    <Typography variant="body1">
                      <Store sx={{ fontSize: 16, mr: 0.5, verticalAlign: 'middle' }} />
                      {bestPrice.bestPrice.platform.name}
                    </Typography>
                    <Typography variant="body2">
                      <Public sx={{ fontSize: 16, mr: 0.5, verticalAlign: 'middle' }} />
                      {bestPrice.bestPrice.country.name}
                    </Typography>
                  </Box>
                  {bestPrice.savingsPercentage > 0 && (
                    <Chip
                      icon={<TrendingDown />}
                      label={`Save ${bestPrice.savingsPercentage.toFixed(0)}%`}
                      color="warning"
                      size="large"
                    />
                  )}
                </Box>
                <Button
                  variant="contained"
                  color="success"
                  fullWidth
                  sx={{ mt: 2 }}
                  href={bestPrice.bestPrice.productUrl}
                  target="_blank"
                  rel="noopener noreferrer"
                >
                  View on {bestPrice.bestPrice.platform.name}
                </Button>
              </CardContent>
            </Card>
          )}

          <Typography variant="h6" gutterBottom fontWeight="bold">
            Description
          </Typography>
          <Typography variant="body1" paragraph>
            {product.description || 'No description available.'}
          </Typography>

          {product.specifications && product.specifications.length > 0 && (
            <>
              <Typography variant="h6" gutterBottom fontWeight="bold" sx={{ mt: 3 }}>
                Specifications
              </Typography>
              <TableContainer component={Paper}>
                <Table size="small">
                  <TableBody>
                    {product.specifications.map((spec) => (
                      <TableRow key={spec.id}>
                        <TableCell component="th" scope="row" sx={{ fontWeight: 'bold' }}>
                          {spec.specificationKey}
                        </TableCell>
                        <TableCell>{spec.specificationValue}</TableCell>
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>
              </TableContainer>
            </>
          )}
        </Grid>

        <Grid item xs={12}>
          <Typography variant="h5" gutterBottom fontWeight="bold">
            Price Comparison
          </Typography>
          <TableContainer component={Paper}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell><strong>Platform</strong></TableCell>
                  <TableCell><strong>Country</strong></TableCell>
                  <TableCell align="right"><strong>Price</strong></TableCell>
                  <TableCell><strong>Status</strong></TableCell>
                  <TableCell align="center"><strong>Action</strong></TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {pricesList.map((price, index) => (
                  <TableRow 
                    key={`${price.platformId}-${price.countryId}`}
                    sx={{ bgcolor: index === 0 ? 'success.light' : 'inherit' }}
                  >
                    <TableCell>{price.platform.name}</TableCell>
                    <TableCell>{price.country.name}</TableCell>
                    <TableCell align="right">
                      <Typography fontWeight={index === 0 ? 'bold' : 'normal'}>
                        {formatPrice(price.price, price.country.currencySymbol)}
                      </Typography>
                    </TableCell>
                    <TableCell>
                      <Chip
                        label={price.isInStock ? 'In Stock' : 'Out of Stock'}
                        color={price.isInStock ? 'success' : 'error'}
                        size="small"
                      />
                    </TableCell>
                    <TableCell align="center">
                      <Button
                        variant="outlined"
                        size="small"
                        href={price.productUrl}
                        target="_blank"
                        rel="noopener noreferrer"
                        disabled={!price.isInStock}
                      >
                        View
                      </Button>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </Grid>
      </Grid>
    </Container>
  );
};

export default ProductDetailPage;
