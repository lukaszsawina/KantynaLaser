import * as yup from 'yup';

const editUserValidationSchema = yup.object().shape({
  firstName: yup.string().required('First name is required').max(200,"First name is to long"),
  lastName: yup.string().required('Last name is required').max(200,"Last name is to long"),
  email: yup.string().email('Invalid email address').required('Email is required'),
  id:  yup.string().required()
});

export default editUserValidationSchema;