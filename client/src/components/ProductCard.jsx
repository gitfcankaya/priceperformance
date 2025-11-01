import { Card, CardContent, CardMedia, Typography, Button, Box, Chip } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { TrendingDown } from '@mui/icons-material';

const ProductCard = ({ product, bestPrice }) => {
  const navigate = useNavigate();

  const formatPrice = (price, currencySymbol) => {
    return `${currencySymbol}${price.toFixed(2)}`;
  };

  const handleClick = () => {
    navigate(`/product/${product.id}`);
  };

  return (
    <Card 
      sx={{ 
        height: '100%', 
        display: 'flex', 
        flexDirection: 'column',
        cursor: 'pointer',
        transition: 'transform 0.2s',
        '&:hover': {
          transform: 'scale(1.03)',
          boxShadow: 6
        }
      }}
      onClick={handleClick}
    >
      <CardMedia
        component="img"
        height="200"
        image={product.imageUrl || 'https://via.placeholder.com/300x200?text=No+Image'}
        alt={product.name}
        sx={{ objectFit: 'cover' }}
      />
      <CardContent sx={{ flexGrow: 1 }}>
        <Typography gutterBottom variant="h6" component="div" noWrap>
          {product.name}
        </Typography>
        
        {product.brand && (
          <Typography variant="body2" color="text.secondary" gutterBottom>
            {product.brand}
          </Typography>
        )}

        {bestPrice && (
          <Box sx={{ mt: 2 }}>
            <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', mb: 1 }}>
              <Typography variant="h5" color="primary" fontWeight="bold">
                {formatPrice(bestPrice.bestPrice.price, bestPrice.bestPrice.country.currencySymbol)}
              </Typography>
              {bestPrice.savingsPercentage > 0 && (
                <Chip 
                  icon={<TrendingDown />}
                  label={`Save ${bestPrice.savingsPercentage.toFixed(0)}%`}
                  color="success"
                  size="small"
                />
              )}
            </Box>
            
            <Typography variant="caption" color="text.secondary">
              Best price on {bestPrice.bestPrice.platform.name}
            </Typography>
            
            {bestPrice.bestPrice.country && (
              <Typography variant="caption" color="text.secondary" display="block">
                in {bestPrice.bestPrice.country.name}
              </Typography>
            )}
          </Box>
        )}

        <Button 
          variant="contained" 
          fullWidth 
          sx={{ mt: 2 }}
          onClick={(e) => {
            e.stopPropagation();
            handleClick();
          }}
        >
          Compare Prices
        </Button>
      </CardContent>
    </Card>
  );
};

export default ProductCard;
