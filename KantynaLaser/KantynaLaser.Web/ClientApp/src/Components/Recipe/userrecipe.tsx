import { Box, Button, ButtonGroup, Container, IconButton, List, ListItem, ListItemText, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { RecipeDto } from "../../api/recipe";
import { APIdelete, APIget } from "../../api/api";
import EditIcon from '@mui/icons-material/Edit';
import DeleteOutlineIcon from '@mui/icons-material/DeleteOutline';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import Pages from "../../Common/pages";

const UserRecipe = () => {
    const navigate = useNavigate();
    const [recipe, setRecipe] = useState<RecipeDto>();
    const {id} = useParams();
    const [open, setOpen] = useState(false);

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

    const handleClickOpen = () => {
        setOpen(true);
      };
    
      const handleClose = () => {
        setOpen(false);
      };
    
      const handleConfirm = async () => {
        try {
            await APIdelete(`Recipes/${id}`);
        } catch (error) {
            console.error('Error fetching other data:', error);
        }
        handleClose();
        navigate(Pages.MYACCOUNT);
      };

    return (
        <Container maxWidth="lg" sx={{heiht: '100vh', paddingBottom: '30px'}}>
            <Box marginTop={12} marginBottom={3} sx={{ display: 'inline-flex', flexDirection: 'column', width: "100%"}}>
                <Typography variant='h3' component="span" marginTop={2}>
                    {recipe?.title}
                </Typography>  
                <Typography variant='caption' component="span">
                    {recipe?.isPublic ? "Public recipe" : "Private recipe"}
                </Typography>  
                <ButtonGroup>
                    <IconButton  onClick={() => {navigate('Edit')}} sx={{marginTop: '8px', marginBottom:'8px'}} color="secondary" ><EditIcon /></IconButton >  
                    <IconButton  onClick={handleClickOpen} sx={{ marginTop: '8px', marginBottom:'8px'}} color="primary"  ><DeleteOutlineIcon /></IconButton >
                </ButtonGroup>
                  
                <Typography variant='h6' component="span">
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
    
            <Dialog open={open} onClose={handleClose}>
                <DialogTitle>Delete Recipe</DialogTitle>
                <DialogContent>
                <DialogContentText>
                    Are you sure you want to delete?
                </DialogContentText>
                </DialogContent>
                <DialogActions>
                    <ButtonGroup>
                        <Button onClick={handleClose} color="primary" >
                            No
                        </Button>
                        <Button onClick={handleConfirm} color="secondary" variant="contained">
                            Yes
                        </Button>
                    </ButtonGroup>
                </DialogActions>
            </Dialog>

        </Container>
        
    );
}

export default UserRecipe;

export const formatTimeString = (timeString: string): string => {
    // Parsowanie ciągu czasu do obiektu daty
    const time = new Date(`1970-01-01T${timeString}Z`);
  
    // Pobranie godzin i minut z obiektu daty
    const hours = time.getUTCHours();
    const minutes = time.getUTCMinutes();
  
    // Formatowanie do postaci "HH:mm"
    const formattedTime = `${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}`;
  
    return formattedTime;
  };
  