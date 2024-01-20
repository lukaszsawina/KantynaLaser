import { Box, Container, List, ListItem, ListItemText, Tab, Tabs, Typography } from "@mui/material";
import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { RecipeDto } from "../../api/recipe";
import { APIget } from "../../api/api";

const Recipe = () => {
    const [recipe, setRecipe] = useState<RecipeDto>();
    const {id} = useParams();
    
    useEffect(() => {
        const fetchRecipes = async () => {
          try {
            const response = await APIget<RecipeDto>(`Recipes/${id}`);
            setRecipe(response);
          } catch (error) {
            console.error('Error fetching other data:', error);
          }
        };
    
        fetchRecipes();
      }, []); 

    return (
        <Container maxWidth="lg" sx={{heiht: '100vh', paddingBottom: '30px'}}>
            <Box marginTop={12} marginBottom={3} sx={{ display: 'inline-flex', flexDirection: 'column', width: "100%"}}>
                <Typography variant='h3' component="span" marginTop={2}>
                    {recipe?.title}
                </Typography>    
                <Typography variant='h6' component="span" marginTop={4}>
                    Preparation time: {formatTimeString(recipe ? recipe?.preparationTime : "")}
                </Typography> 
                <Typography variant='h6' component="span">
                    Cooking time: {formatTimeString(recipe ? recipe?.cookingTime : "")}
                </Typography> 
                <Typography variant='h4' component="span" marginTop={4}>
                    Ingredients
                </Typography>        
                <List >
                {recipe?.ingredients.map(ingredient => (
                    <ListItem key={ingredient.name} divider> 
                        <ListItemText
                        primary={ingredient.name}
                        secondary={ingredient.quantity ? `Quantity: ${ingredient.quantity} ${ingredient.unit}` : null}  
                        />
                    </ListItem>
                    ))}
                </List>
                <Typography variant='h4' component="span">
                    Tools
                </Typography>        
                <List >
                {recipe?.tools.map(tool => (
                    <ListItem key={tool.name} divider> 
                        <ListItemText
                        primary={tool.name}
                        secondary={tool.amount ? `Amount: ${tool.amount}` : null}  
                        />
                    </ListItem>
                    ))}
                </List>

                <Typography variant='h4' component="span">
                    Steps
                </Typography>        
                <List >
                {recipe?.steps.map((step, index) => (
                    <ListItem key={step.name} > 
                        <ListItemText
                        primary={`Step: ${index+1}`}
                        secondary={step.name ? step.name : null}  
                        />
                    </ListItem>
                    ))}
                </List>
                
            </Box>
    
        </Container>
    );
}

export default Recipe;

const formatTimeString = (timeString: string): string => {
    const time = new Date(`1970-01-01T${timeString}Z`);
  
    const hours = time.getUTCHours();
    const minutes = time.getUTCMinutes();
  
    const formattedTime = `${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}`;
  
    return formattedTime;
  };
  