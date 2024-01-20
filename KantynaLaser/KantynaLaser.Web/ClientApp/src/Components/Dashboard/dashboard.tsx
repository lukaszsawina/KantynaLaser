import { Box, Button, Card, CardActions, CardContent, Container, Divider, Pagination, PaginationItem, Typography } from "@mui/material";
import { format } from "date-fns";
import { useEffect, useState } from "react";
import { RecipeDto } from "../../api/recipe";
import { APIget } from "../../api/api";
import { useNavigate } from "react-router-dom";
import Pages from "../../Common/pages";

const Dashboard = () => {
    const navigate = useNavigate();

    const [recipes, setRecipes] = useState<RecipeDto[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    const itemsPerPage = 5;
    
    useEffect(() => {
        const fetchRecipes = async () => {
          try {
            const response = await APIget<RecipeDto[]>(`Recipes/public`);
            const shuffledRecipes = shuffleArray(response);

            setRecipes(shuffledRecipes);
          } catch (error) {
            console.error('Error fetching other data:', error);
          }
        };
    
        fetchRecipes();
      }, []);  


      const handlePageChange = (event: React.ChangeEvent<unknown>, value: number) => {
        setCurrentPage(value);
      };
    
      const start = (currentPage - 1) * itemsPerPage;
      const end = start + itemsPerPage;
      const displayedRecipes = recipes?.slice(start, end);

    return (
        <Container maxWidth="lg" sx={{heiht: '100vh', paddingBottom: '30px'}}>
            <Box marginTop={12} marginBottom={3} sx={{ display: 'inline-flex', flexDirection: 'column'}}>
                <Typography variant='h3' component="span" marginBottom={2} marginTop={2}>
                    Welcome to Kantyna Laser
                </Typography>    
                <Typography variant='subtitle1'  align="justify">
                    Discover the joy of cooking with Recipe Keeper, your go-to platform for organizing and sharing your favorite recipes. 
                    Whether you're a seasoned chef or a kitchen novice, 
                    our app is designed to simplify your cooking experience and bring out the inner chef in you.
                    Embark on a culinary journey where creativity knows no bounds, and every meal becomes a delightful masterpiece with Kantyna Laser.
                </Typography>    
                <Typography variant='h4' component="span" marginTop={4}>
                    See some public recipes
                </Typography>        
            </Box>
               
            <Container maxWidth="md">
                {displayedRecipes &&
                displayedRecipes.map((recipe) => (
                    <div key={recipe.id}>
                    <Card sx={{ minWidth: 275, backgroundColor: '#f2f2f2 ', margin: '16px 0' }}>
                        <CardContent>
                        <Typography sx={{ fontSize: 14 }} color="text.secondary" gutterBottom>
                            Added {format(new Date(recipe.createdAt), 'dd/MM/yyyy HH:mm')}
                        </Typography>
                        <Typography variant="h5" component="div">
                            {recipe.title}
                        </Typography>
                        <Typography variant="body2">
                            Preparation time: {recipe.preparationTime} / Cooking time: {recipe.cookingTime}
                        </Typography>
                        </CardContent>
                        <CardActions>
                        <Button size="large" onClick={() => {navigate(`/Recipe/${recipe.id}`)}}>See more</Button>
                        </CardActions>
                    </Card>
                    <Divider variant='middle' />
                    </div>
                ))}
            </Container>
            <Box sx={{display: 'flex', justifyContent:'center', alignItems: 'center', marginTop: '8px'}}>
                <Pagination
                    count={Math.ceil(recipes?.length / itemsPerPage)}
                    page={currentPage}
                    onChange={handlePageChange}
                    renderItem={(item) => (
                    <PaginationItem
                        component="div"
                        {...item}
                    />
                    )}
                />
            </Box>
        </Container>    
    );
}

export default Dashboard;

const shuffleArray = (array: any[]) => {
    const shuffledArray = [...array];
    for (let i = shuffledArray.length - 1; i > 0; i--) {
      const j = Math.floor(Math.random() * (i + 1));
      [shuffledArray[i], shuffledArray[j]] = [shuffledArray[j], shuffledArray[i]];
    }
    return shuffledArray;
  };