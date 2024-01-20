import { Box, Button, Card, CardActions, CardContent, Container, Divider, Pagination, PaginationItem, Tab, Tabs, TextField, Typography } from "@mui/material";
import { SyntheticEvent, useEffect, useState } from "react";
import { useUserContext } from "./usercontext";
import {RecipeDto} from '../../api/recipe';
import AddIcon from '@mui/icons-material/Add';
import EditIcon from '@mui/icons-material/Edit';
import CloseIcon from '@mui/icons-material/Close';
import SaveIcon from '@mui/icons-material/Save';
import editUserValidationSchema from './edit-validation';
import { APIget, APIput } from "../../api/api";
import { format } from "date-fns";
import { useForm } from "react-hook-form";
import { UserAccount } from "../../api/userAccount";
import { yupResolver } from "@hookform/resolvers/yup";
import { useNavigate } from "react-router-dom";
import Pages from "../../Common/pages";

const MyAccount = () => {
    const {user, refreshUser} = useUserContext();
    const navigate = useNavigate();
    const [recipes, setRecipes] = useState<RecipeDto[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [isEdit, setIsEdit] = useState(false);
    const [currentPage, setCurrentPage] = useState(1);
    const itemsPerPage = 5;
    const [currentTabIndex, setCurrentTabIndex] = useState(0);
 
  const handleTabChange = (event: SyntheticEvent<Element, Event>, tabIndex : number) => {
    console.log(tabIndex);
    setCurrentTabIndex(tabIndex);
  };

    const emotkiList = ['😀', '😂', '😍', '🎉', '🤔', '🚀', '👍', '👀', '🌈', '🍕'];
    const [selectedEmotka, setSelectedEmotka] = useState('😀');

    useEffect(() => {
        const randomIndex = Math.floor(Math.random() * emotkiList.length);
        const randomEmotka = emotkiList[randomIndex];
        setSelectedEmotka(randomEmotka);
    },[user]);


    useEffect(() => {
        if (user) {
        setIsLoading(false);
        }
    }, [user]); 

    useEffect(() => {
        const fetchRecipes = async () => {
          try {
            if (user) {
                const response = await APIget<RecipeDto[]>(`Recipes/user/${user.id}`);
                setRecipes(response);
              } 
          } catch (error) {
            console.error('Error fetching other data:', error);
          }
        };
    
        fetchRecipes();
      }, [user]);

    const handleEdit = () => {
        setIsEdit(!isEdit);
    }

    const { register, handleSubmit, formState } = useForm<UserAccount>({resolver: yupResolver(editUserValidationSchema), reValidateMode: 'onChange', });

    const onSubmit = async (data : UserAccount) => {
        try {
            await APIput("Users",data);

            refreshUser();
            
            handleEdit();
        } catch (error: any) {
          console.error('Błąd logowania:', error);
        }
      };
    
    const title = `Hello ${user?.firstName} ${user?.lastName} ${selectedEmotka}`;

    const handlePageChange = (event: React.ChangeEvent<unknown>, value: number) => {
        setCurrentPage(value);
      };

    const start = (currentPage - 1) * itemsPerPage;
    const end = start + itemsPerPage;
    const displayedRecipes = recipes?.filter(recipe => {
        if (currentTabIndex === 1) {
          return recipe.isPublic === true;
        } else if (currentTabIndex === 2) {
          return recipe.isPublic === false;
        } else {
          return true; 
        }
      }).slice(start, end);

    if (isLoading) {
        return (
            <Container maxWidth="lg" sx={{heiht: '100vh'}}>
                <Box marginTop={12} marginBottom={3}>
                    <Typography variant='h4' component="span">
                        Loading...
                    </Typography>
                </Box>
            </Container>
        );
    }
    else{
        return (
            <Container maxWidth="lg" sx={{heiht: '100vh', paddingBottom: '30px'}}>
                <Box marginTop={12} marginBottom={3} sx={{ display: 'inline-flex', flexDirection: 'column'}}>
                    {!isEdit &&<Typography variant='h4' component="span">
                        {title}
                    </Typography>}
                    {isEdit &&
                    
                    <Typography variant='h4' component="span" sx={{display: 'inline-flex', justifyContent: 'flex-start', alignItems: 'center'}}>
                        Hello
                        <form>
                        <input type="hidden" value={user?.id} {...register('id')}/>
                        <input type="hidden" value={user?.email} {...register('email')}/>
                        <TextField
                            {...register('firstName', { required: 'Field is required' })}
                            label="First Name"
                            defaultValue={user?.firstName}
                            sx={{margin: '4px'}}
                            error={!!formState.errors.firstName}
                            />
                        <TextField
                            {...register('lastName', { required: 'Field is required' })}
                            label="Last Name"
                            defaultValue={user?.lastName}
                            sx={{margin: '4px'}}
                            error={!!formState.errors.lastName}
                            />
                    </form>
                    </Typography>   
                    }
                    <Box>
                        <Button onClick={handleEdit} sx={{width: "100px", marginTop: '8px'}}>{isEdit ? <CloseIcon/> : <EditIcon />}{!isEdit ? "Edit" : "Close"}</Button>
                        {isEdit && <Button onClick={handleSubmit(onSubmit)}
                            type="submit" variant="contained" color="secondary" sx={{width: "100px", margin: '4px' }}>
                            <SaveIcon/>   Save
                        </Button>}
                    </Box>
                </Box>
                <Box sx={{ display: 'flex', justifyContent: 'space-between'}}>
                    <Typography variant='h5' component="span">
                        Your recipes
                    </Typography>
                    <Button size="large" onClick={() => {navigate(`${Pages.NEWUSERRECIPE}`)}}><AddIcon />Add new</Button>
                </Box>
                <Tabs
                    value={currentTabIndex}
                    onChange={handleTabChange}
                    textColor="secondary"
                    indicatorColor="secondary"
                    aria-label="secondary tabs example"
                    >
                    <Tab label="All" />
                    <Tab label="Public" />
                    <Tab label="Private" />
                </Tabs>
                <Container maxWidth="md">
                    {displayedRecipes &&
                        displayedRecipes.map((recipe) => (
                        <div key={recipe.id}>
                        <Card sx={{ minWidth: 275, backgroundColor: '#f2f2f2 ', margin: '16px 0' }}>
                            <CardContent>
                            <Typography sx={{ fontSize: '14' }} color="text.secondary" gutterBottom>
                                Added {format(new Date(recipe.createdAt), 'dd/MM/yyyy HH:mm')}
                            </Typography>
                            <Typography variant="h5" component="div">
                                {recipe.title}
                            </Typography>
                            <Typography sx={{ fontSize: '14', paddingBottom: '8px'}} color="text.secondary">
                                {recipe.isPublic ? "Public" : "Private"}
                            </Typography>
                            <Typography variant="body2">
                                Preparation time: {recipe.preparationTime} / Cooking time: {recipe.cookingTime}
                            </Typography>
                            </CardContent>
                            <CardActions>
                            <Button size="large" onClick={() => {navigate(`${Pages.MYACCOUNT}/Recipe/${recipe.id}`)}}>See more</Button>
                            </CardActions>
                        </Card>
                        <Divider variant='middle' />
                        </div>
                    ))}
                </Container>
                <Box sx={{display: 'flex', justifyContent:'center', alignItems: 'center', marginTop: '8px'}}>
                    <Pagination
                        count={Math.ceil(recipes.filter(recipe => {
                            if (currentTabIndex === 1) {
                              return recipe.isPublic === true;
                            } else if (currentTabIndex === 2) {
                              return recipe.isPublic === false;
                            } else {
                              return true; 
                            }
                          })?.length / itemsPerPage)}
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
}

export default MyAccount;