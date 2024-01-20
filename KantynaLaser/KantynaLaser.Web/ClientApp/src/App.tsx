import './App.css';
import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import Header from './Components/Header/header';
import Login from './Components/User/login';
import Register from './Components/User/register';
import Pages from './Common/pages';
import MyAccount from './Components/User/myaccount';
import { UserProvider } from './Components/User/usercontext';
import Cookies from 'js-cookie';
import Dashboard from './Components/Dashboard/dashboard';
import UserRecipe from './Components/Recipe/userrecipe';
import NewUserRecipe from './Components/Recipe/newRecipe';
import Recipe from './Components/Recipe/recipes';
import EditRecipe from './Components/Recipe/editRecipe';

function App() {

    const token = Cookies.get("authToken");

    return (
        <UserProvider>
            <div className='App' >
            
            <BrowserRouter>
                <div className='MainContent'>
                    <Header />
                    <Routes>
                        <Route path={Pages.DASHBOARD} element={<Dashboard />} />
                        {token && (<Route path={Pages.MYACCOUNT} element={<MyAccount />} />)}
                        {token && (<Route path={Pages.USERRECIPE} element={<UserRecipe/>} />)}
                        {token && (<Route path={Pages.NEWUSERRECIPE} element={<NewUserRecipe/>} />)}
                        {token && (<Route path={Pages.EDITUSERRECIPE} element={<EditRecipe/>} />)}
                        <Route path={Pages.RECIPE} element={<Recipe />} />
                        <Route path={Pages.LOGIN} element={<Login />} />
                        <Route path={Pages.SIGNIN} element={<Register />} />
                        <Route path="*" element={<Navigate to={Pages.DASHBOARD} />} />
                    </Routes>
                </div>
            </BrowserRouter>
            </div>
        </UserProvider>
        
      );
}

export default App;
