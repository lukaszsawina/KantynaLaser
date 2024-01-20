import { Alert, Box, Button, Checkbox, Container, FormControlLabel, IconButton, InputAdornment, List, ListItem, ListItemText, MenuItem, TextField, Typography } from "@mui/material";
import {  useEffect, useState } from "react";
import { Ingredient, RecipeDto, Step, Tool } from "../../api/recipe";
import SaveIcon from '@mui/icons-material/Save';
import DeleteIcon from '@mui/icons-material/Delete';
import AddIcon from '@mui/icons-material/Add';
import {  APIget, APIpostAuth, APIput } from "../../api/api";
import {  SubmitHandler, useForm } from "react-hook-form";
import { useNavigate, useParams } from "react-router-dom";
import Pages from "../../Common/pages";
import { formatTimeString } from "./userrecipe";



interface Recipe {
    title: string;
    ingredients: Ingredient[];
    tools: Tool[];
    steps: Step[];
    preparationTime: string;
    cookingTime: string; 
    isPublic: boolean;
}

interface RecipeMainData {
    title: string;
    preparationTime: string;
    cookingTime: string;
    isPublic: boolean;
}



const EditRecipe = () => {
    const {id} = useParams();
    const [createdAt, setCreatedAt] = useState("");
    const [initialIsPublicValue, setInitialIsPublicValue] = useState(false);
    const [successfullMessage, setSuccessfullMessage] = useState("");
    const { register: registerRecipe, handleSubmit: handleSubmitRecipe, formState: { errors }, setValue: setValueRecipe, getValues } = useForm<RecipeMainData>();
    const { register: registerIngredient, handleSubmit: handleSubmitIngredient, formState: formStateIngredient, setValue: setValueIngredient } = useForm<Ingredient>();
    const { register: registerTools, handleSubmit: handleSubmitTools, formState: formStateTools, setValue: setValueTools } = useForm<Tool>();
    const { register: registerSteps, handleSubmit: handleSubmitSteps, formState: formStateSteps, setValue: setValueSteps } = useForm<Step>();

    const [ingredientsList, setIngredientsList] = useState<Ingredient[]>([]);
    const [toolList, setToolList] = useState<Tool[]>([]);
    const [stepList, setStepList] = useState<Step[]>([]);

    const navigate = useNavigate();

    useEffect(() => {
      const fetchRecipes = async () => {
        try {
          const response = await APIget<RecipeDto>(`Recipes/${id}`);
          
          setIngredientsList(response.ingredients);
          setToolList(response.tools);
          setStepList(response.steps);


          setValueRecipe("title", response.title);
          setValueRecipe("preparationTime", formatTimeString(response.preparationTime ? response?.preparationTime : "00:00"));
          setValueRecipe("cookingTime", formatTimeString(response.cookingTime ? response?.cookingTime : "00:00"));
          setValueRecipe("isPublic", response.isPublic);
          setInitialIsPublicValue(response.isPublic);
          setCreatedAt(response.createdAt);

        } catch (error) {
          console.error('Error fetching other data:', error);
        }
      };
  
      fetchRecipes();
    }, []); 

    const units = [
        {
          value: 'kg',
          label: 'kg',
        },
        {
          value: 'g',
          label: 'g',
        },
        {
          value: 'L',
          label: 'L',
        },
        {
          value: 'ml',
          label: 'ml',
        },
      ];
      const sleep = (ms: number) => new Promise(resolve => setTimeout(resolve, ms));

      const onSubmit: SubmitHandler<RecipeMainData> = async (data: RecipeMainData) => {
        try {
            const requestData: RecipeDto = {
                title: data.title,
                preparationTime: `${data.preparationTime}:00`,
                cookingTime: `${data.cookingTime}:00`,
                ingredients: ingredientsList,
                tools: toolList,
                steps: stepList,
                isPublic: data.isPublic,
                id: id ? id : '',
                createdAt: createdAt
              };

            const result = await APIput("Recipes",requestData);    
            setSuccessfullMessage("Successfully saved!");
            await sleep(1000);
            navigate(Pages.MYACCOUNT);

        } catch (error: any) {
          console.error('Błąd:', error);
        }
      };

      const onIngredientAdd = async (data : Ingredient) => {
        try {
            setIngredientsList((prevList) => [...prevList, data]);

            setValueIngredient('name', '');
            setValueIngredient('quantity', 0);
            setValueIngredient('unit', 'kg');
        } catch (error: any) {
          console.error('Błąd:', error);
        }
      };

      const removeIngredient = (indexToRemove: number) => {
        setIngredientsList((prevList) => {
          const updatedList = prevList.filter((_, index) => index !== indexToRemove);
          return updatedList;
        });
      };

      const onToolAdd = async (data : Tool) => {
        try {
            setToolList((prevList) => [...prevList, data]);

            setValueTools('name', '');
            setValueTools('amount', 0);
        } catch (error: any) {
          console.error('Błąd:', error);
        }
      };

      const removeTool = (indexToRemove: number) => {
        setToolList((prevList) => {
          const updatedList = prevList.filter((_, index) => index !== indexToRemove);
          return updatedList;
        });
      };

      const onStepAdd = async (data : Step) => {
        try {
            setStepList((prevList) => [...prevList, data]);

            setValueSteps('name', '');
        } catch (error: any) {
          console.error('Błąd:', error);
        }
      };

      const removeStep = (indexToRemove: number) => {
        setStepList((prevList) => {
          const updatedList = prevList.filter((_, index) => index !== indexToRemove);
          return updatedList;
        });
      };

    return (
        <Container maxWidth="lg" sx={{heiht: '100vh', paddingBottom: '30px'}}>
            <form onSubmit={handleSubmitRecipe(onSubmit)}>
            <Box marginTop={12} marginBottom={3} sx={{ display: 'inline-flex', flexDirection: 'column', width: "100%"}}>
                <Typography variant='h3' component="span" marginTop={2} marginBottom={2}>
                    Edit recipe
                </Typography>
                <Typography variant='h6' component="span" marginTop={4}>
                    Title
                </Typography> 
                
                <TextField
                  {...registerRecipe('title', { required: 'Title is required' })}
                  label="Title"
                  sx={{ margin: '4px' }}
                  error={!!errors.title}
                  InputLabelProps={{
                    shrink: true,
                  }}
                  helperText={errors.title?.message}
                />

              <Typography variant="h6" component="span" marginTop={4}>
                Preparation time
              </Typography>
              <TextField
                {...registerRecipe('preparationTime', {
                  required: 'Field is required',
                  pattern: {
                    value: /^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$/,
                    message: 'Time must be in format HH:mm',
                  },
                })}
                label="Preparation time"
                sx={{ margin: '4px' }}
                error={!!errors.preparationTime}
                helperText={'Time must be in format HH:mm'}
                InputLabelProps={{
                  shrink: true,
                }}
              />

              <Typography variant="h6" component="span" marginTop={4}>
                Cooking time
              </Typography>
              <TextField
                {...registerRecipe('cookingTime', {
                  required: 'Field is required',
                  pattern: {
                    value: /^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$/,
                    message: 'Time must be in format HH:mm',
                  },
                })}
                label="Cooking time"
                sx={{ margin: '4px' }}
                error={!!errors.cookingTime}
                helperText={'Time must be in format HH:mm'}
                InputLabelProps={{
                  shrink: true,
                }}
              />
                <Typography variant='h6' component="span" marginTop={4}>
                    Ingredients
                </Typography> 
                <Box>
                    <TextField
                    {...registerIngredient('name', { required: 'Field is required' })}
                    label="Name"
                    sx={{margin: '4px'}}
                    error={!!formStateIngredient.errors.name}
                        helperText={!!formStateIngredient.errors.name && formStateIngredient.errors.name.message}
                    /> 
                    <TextField
                        {...registerIngredient('quantity', {
                            required: 'Field is required',
                            min: { value: 0.0001, message: 'Quantity must be greater than 0' }
                        })}
                        type="number"
                        label="Quantity"
                        sx={{ margin: '4px' }}
                        error={!!formStateIngredient.errors.quantity}
                        helperText={!!formStateIngredient.errors.quantity && formStateIngredient.errors.quantity.message}
                        />
                    <TextField sx={{width: '160px', margin: '4px'}}
                        select
                        label="Select"
                        defaultValue={'kg'}
                        helperText="Please select your unit"
                        {...registerIngredient('unit', { required: 'Field is required' })}
                        >
                        {units.map((unit) => (
                            <MenuItem key={unit.value} value={unit.value}>
                                {unit.label}
                            </MenuItem>
                        ))}
                        </TextField>
                        
                    
                </Box>
                <Button onClick={handleSubmitIngredient(onIngredientAdd)}
                        type="button" variant="contained" color="secondary" sx={{width: "100px", margin: '4px' }}>
                        <AddIcon/>Add
                    </Button>

                <List>
                    {ingredientsList.map((ingredient, index) => (

                      <ListItem key={index}
                        secondaryAction={
                          <IconButton edge="end" aria-label="delete" onClick={() => removeIngredient(index)} >
                            <DeleteIcon />
                          </IconButton>
                        }
                      >
                       <ListItemText
                            primary={ingredient.name}
                            secondary={ingredient.quantity ? `Quantity: ${ingredient.quantity} ${ingredient.unit}` : null}  
                        />
                      </ListItem>
                    
                    ))}
                </List>
                <Typography variant='h6' component="span" marginTop={4}>
                    Tools
                </Typography> 
                <Box>
                    <TextField
                    {...registerTools('name', { required: 'Field is required' })}
                    label="Name"
                    sx={{margin: '4px'}}
                    error={!!formStateTools.errors.name}
                    helperText={!!formStateTools.errors.name && formStateTools.errors.name.message}
                    /> 
                   <TextField
                        type="number"
                        {...registerTools('amount', {
                            required: 'Field is required',
                            min: { value: 1, message: 'Amount must be greater than 0' }
                        })}
                        label="Amount"
                        sx={{ margin: '4px' }}
                        error={!!formStateTools.errors.amount}
                        helperText={!!formStateTools.errors.amount && formStateTools.errors.amount.message}
                        />
                </Box>
                <Button onClick={handleSubmitTools(onToolAdd)}
                        type="button" variant="contained" color="secondary" sx={{width: "100px", margin: '4px' }}>
                        <AddIcon/>Add
                    </Button>

                <List>
                    {toolList.map((tool, index) => (

                      <ListItem key={index}
                        secondaryAction={
                          <IconButton edge="end" aria-label="delete" onClick={() => removeTool(index)} >
                            <DeleteIcon />
                          </IconButton>
                        }
                      >
                       <ListItemText
                            primary={tool.name}
                            secondary={tool.amount ? `Amount: ${tool.amount}` : null}  
                        />
                      </ListItem>
                    
                    ))}
                </List>
                <Typography variant='h6' component="span" marginTop={4}>
                    Steps
                </Typography> 
                <Box>
                    <TextField
                    {...registerSteps('name', { required: 'Field is required' })}
                    label="Step"
                    sx={{margin: '4px', width: '100%'}}
                    error={!!formStateSteps.errors.name}
                    helperText={!!formStateSteps.errors.name && formStateSteps.errors.name.message}
                    /> 
                </Box>
                <Button onClick={handleSubmitSteps(onStepAdd)}
                        type="button" variant="contained" color="secondary" sx={{width: "100px", margin: '4px' }}>
                        <AddIcon/>Add
                    </Button>

                <List>
                    {stepList.map((step, index) => (

                    <ListItem key={index}
                        secondaryAction={
                          <IconButton edge="end" aria-label="delete" onClick={() => removeStep(index)} >
                            <DeleteIcon />
                          </IconButton>
                        }
                      >
                       <ListItemText
                           primary={`Step: ${index+1}`}
                           secondary={step.name ? step.name : null}  
                        />
                      </ListItem>
                    
                    ))}
                </List>
                <FormControlLabel
                  control={<Checkbox {...registerRecipe('isPublic')}/>}
                  label="Is Public"
                />
                
                <Button
                    type="submit"
                    variant="contained"
                    color="secondary"
                    sx={{ width: '100px', margin: '4px' }}
                >
                    <SaveIcon /> Save
                </Button>
                
                {successfullMessage && (
                <Alert severity="success" sx={{marginTop: '8px'}}>{successfullMessage}</Alert>
            )}
            </Box>
            </form>
        </Container>
    );
}

export default EditRecipe;
