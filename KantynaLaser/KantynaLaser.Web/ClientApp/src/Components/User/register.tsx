import { Alert, Box, Button, Container, Grid, TextField, Typography} from '@mui/material';
import { APIlogin, APIpost } from '../../api/api';
import { useForm } from 'react-hook-form';
import Cookies from 'js-cookie';
import { useState } from 'react';
import Pages from '../../Common/pages';
import registerValidationSchema from './register-validation';
import { yupResolver } from '@hookform/resolvers/yup';
import ErrorOutlineIcon from '@mui/icons-material/ErrorOutline';
import { LoginDto } from './login';

export interface RegisterDto {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    confirmPassword: string;
}

const Register = () => {
    const [isSubmiting, setIsSubmiting] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");
    const [successfullMessage, setSuccessfullMessage] = useState("");
    const { register, handleSubmit, reset, formState } = useForm<RegisterDto>({resolver: yupResolver(registerValidationSchema), reValidateMode: 'onChange', });

    const createApiPayload = (formData: RegisterDto) => {
        const { confirmPassword, ...apiPayload } = formData;
        return apiPayload;
    };

    const convertToLoginDto = (registerDto: RegisterDto): LoginDto => {
        const { confirmPassword, ...loginDto } = registerDto;
        return loginDto;
    };

    const onSubmit = async (data : RegisterDto) => {
        try {
          setErrorMessage("");
          setIsSubmiting(true);

          var requestData = createApiPayload(data);

          const result = await APIpost("Identity/register",requestData);    

          if(result === 'Successfully Registered')
          {
            setSuccessfullMessage(result);
            var loginData = convertToLoginDto(data);
            const token = await APIlogin(loginData);
            const expirationTime = new Date((new Date()).getTime() + 3 * 60 * 60 * 1000);
            Cookies.set('authToken', token, { expires: expirationTime });
  
            window.location.href = Pages.DASHBOARD;
          }
          else
          {
            setErrorMessage(result);
            setIsSubmiting(false);
          }
        } catch (error: any) {
          setIsSubmiting(false);
          console.error('Błąd rejestracji:', error);
        }
      };
      
      const handleReset = () => {
        setErrorMessage("");
        reset();
      };
      
    return (
        <Container maxWidth="xs">
                <Box marginTop={12} marginBottom={3}>
                    <Typography variant='h4' component="span">
                        Sign in
                    </Typography>
                </Box>
                
                <form onSubmit={handleSubmit(onSubmit)}>
                    <Grid container spacing={4}>
                        <Grid item xs={12}>
                            {errorMessage && (
                                <Alert severity="error">{errorMessage}</Alert>
                            )}
                            {successfullMessage && (
                                <Alert severity="success">{successfullMessage}</Alert>
                            )}
                        </Grid>
                        <Grid item xs={12}>
                            <TextField
                            {...register('firstName', { required: 'Field is required' })}
                            label="First Name"
                            fullWidth
                            error={!!formState.errors.firstName}
                            />
                            {!!formState.errors.firstName && <Typography variant="body1" sx={{color: 'red'}}>{formState.errors.firstName.message}</Typography>}
                        </Grid>
                        <Grid item xs={12}>
                            <TextField
                            {...register('lastName', { required: 'Field is required' })}
                            label="Last Name"
                            fullWidth
                            error={!!formState.errors.lastName}
                            />
                            {!!formState.errors.lastName && <Typography variant="body1" sx={{color: 'red'}}><ErrorOutlineIcon />{formState.errors.lastName.message}</Typography>}

                        </Grid>

                        <Grid item xs={12}>
                            <TextField
                            {...register('email', { required: 'Field is required' })}
                            label="Email"
                            fullWidth
                            error={!!formState.errors.email}
                            />
                            {!!formState.errors.email && <Typography variant="body1" sx={{color: 'red', display: 'flex', alignItems: 'center', marginTop: "6px"}} ><ErrorOutlineIcon sx={{marginRight: "6px"}} />{formState.errors.email.message}</Typography>}
                        </Grid>
                        <Grid item xs={12}>
                            <TextField
                            {...register('password', { required: 'Field is required' })}
                            label="Password"
                            type="password"
                            fullWidth
                            error={!!formState.errors.password}
                            />
                            {!!formState.errors.password && <Typography variant="body1" sx={{color: 'red'}}>{formState.errors.password.message}</Typography>}

                        </Grid>
                        <Grid item xs={12}>
                            <TextField
                            {...register('confirmPassword', { required: 'Field is required' })}
                            label="Confirm Password"
                            type="password"
                            fullWidth
                            error={!!formState.errors.password}
                            />
                            {!!formState.errors.confirmPassword && <Typography variant="body1" sx={{color: 'red'}}>{formState.errors.confirmPassword.message}</Typography>}

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

export default Register;