import * as yup from 'yup';

const registerValidationSchema = yup.object().shape({
  firstName: yup.string().required('First name is required').max(200,"First name is to long"),
  lastName: yup.string().required('Last name is required').max(200,"Last name is to long"),
  email: yup.string().email('Invalid email address').required('Email is required'),
  password:  yup.string()
  .required('Password is required')
  .min(8, 'Password must be at least 8 characters')
  .matches(/[a-zA-Z]/, 'Password must contain at least one letter')
  .matches(/[0-9]/, 'Password must contain at least one digit')
  .matches(/[!@#$%^&*(),.?":{}|<>]/, 'Password must contain at least one special character'),
  confirmPassword: yup.string()
    .oneOf([yup.ref('password')], 'Passwords must match')
    .required('Confirm Password is required'),
});

export default registerValidationSchema;