import { Alert, Box, Button, Container, Grid, TextField, Typography} from '@mui/material';
import { APIlogin } from '../../api/api';
import { useForm } from 'react-hook-form';
import Cookies from 'js-cookie';
import { useState } from 'react';
import Pages from '../../Common/pages';

export interface LoginDto {
    email: string;
    password: string;
}

const Login = () => {
    const [errorMessage, setErrorMessage] = useState("");
    const [isSubmiting, setIsSubmiting] = useState(false);
    const { register, handleSubmit, reset, formState } = useForm<LoginDto>();
    
    const onSubmit = async (data : LoginDto) => {
        try {
          setIsSubmiting(true);
          setErrorMessage("");

          const token = await APIlogin(data);
          const expirationTime = new Date((new Date()).getTime() + 3 * 60 * 60 * 1000);
          Cookies.set('authToken', token, { expires: expirationTime });

          window.location.href = Pages.DASHBOARD;
        } catch (error: any) {
          setIsSubmiting(false);
          console.error('Błąd logowania:', error);
          setErrorMessage("Incorrect login or password");
        }
      };
      
      const handleReset = () => {
        reset();
        setErrorMessage("");
      };
      
    return (
        <Container maxWidth="xs">
                <Box marginTop={12} marginBottom={3}>
                    <Typography variant='h4' component="span">
                        Log in
                    </Typography>
                </Box>
                
                <form onSubmit={handleSubmit(onSubmit)}>
                    <Grid container spacing={4}>
                        <Grid item xs={12}>
                            {errorMessage && (
                                <Alert severity="error">{errorMessage}</Alert>
                            )}
                        </Grid>
                        <Grid item xs={12}>
                            <TextField
                            {...register('email', { required: 'Field is required' })}
                            label="Email"
                            fullWidth
                            error={!!formState.errors.email}
                            />
                        </Grid>

                        <Grid item xs={12}>
                            <TextField
                            {...register('password', { required: 'Field is required' })}
                            label="Password"
                            type="password"
                            fullWidth
                            error={!!formState.errors.password}
                            />
                        </Grid>
                        <Grid item xs={3}>
                            <Button type="submit" fullWidth variant="contained" color="secondary" disabled={isSubmiting}>
                            Submit
                            </Button>
                        </Grid>
                        <Grid item xs={3}>
                            <Button onClick={handleReset} fullWidth variant="contained" color="info" disabled={isSubmiting}>
                            Reset
                            </Button>
                        </Grid>
                    </Grid>
                </form>   

                
        </Container>
    );
}

export default Login;