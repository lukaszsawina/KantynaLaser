import { AppBar, Box, Toolbar, Typography, Button, IconButton, Menu, MenuItem, Avatar, Breadcrumbs, Link} from '@mui/material';
import { useLocation, useNavigate } from 'react-router-dom';
import React from 'react';
import Cookies from 'js-cookie';
import Pages from '../../Common/pages';
import { useUserContext } from '../User/usercontext';


const Header = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const {user} = useUserContext();

    const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);

    const handleLoginClick = () => {
        navigate(Pages.LOGIN);
    }
    const handleSignInClick = () => {
      navigate(Pages.SIGNIN);
  }

    const handleMenu = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
      };

    const handleClose = () => {
        setAnchorEl(null);
    };

    const handleMyAccount = () => {
      navigate(Pages.MYACCOUNT);
  }

    const handleLogout  = () => {
        Cookies.remove('authToken');
        window.location.reload();
    }
    
    const pathnames = location.pathname.split('/').slice(0, 3).filter((x) => x);
    const lastPathname = pathnames[pathnames.length - 1];
    if (lastPathname && lastPathname.length === 36) {
      pathnames.pop();
    }

    return (
        <Box  sx={{ flexGrow: 1, position: 'fixed',
          top: 0,
          left: 0,
          width: '100vw',
          zIndex: 99
         }}>
        <AppBar position="static" color='secondary'>
            <Toolbar>
            <Typography variant="h4" component="h4" sx={{ flexGrow: 1 , cursor: 'default'}}>
                    Kantyna Laser                
            </Typography>

            {!user && (<>
              {location.pathname !== Pages.LOGIN && (<Button color="inherit" onClick={handleLoginClick}>Log in</Button>)}
              {location.pathname !== Pages.SIGNIN && (<Button color="inherit" onClick={handleSignInClick}>Sign in</Button>)}
            </>
            )}

            {user && (
            <div>
              <IconButton
                size="large"
                aria-label="account of current user"
                aria-controls="menu-appbar"
                aria-haspopup="true"
                onClick={handleMenu}
                color="inherit"
              >
                <Avatar>{user?.firstName[0]}{user?.lastName[0]}</Avatar>
              </IconButton>
              <Menu
                id="menu-appbar"
                anchorEl={anchorEl}
                anchorOrigin={{
                  vertical: 'top',
                  horizontal: 'right',
                }}
                keepMounted
                transformOrigin={{
                  vertical: 'top',
                  horizontal: 'right',
                }}
                open={Boolean(anchorEl)}
                onClose={handleClose}
              >
                <MenuItem onClick={handleMyAccount}>My account</MenuItem>
                <MenuItem onClick={handleLogout}>Logout</MenuItem>
              </Menu>
            </div>
          )}

            </Toolbar>
        </AppBar>
        {user && (
        <Breadcrumbs aria-label="breadcrumb"sx={{padding: '10px'}}>
          {location.pathname !== Pages.DASHBOARD && (<Link color="inherit" sx={{textDecoration: 'none', cursor: 'pointer'}} onClick={() => {
              navigate(Pages.DASHBOARD);
            }}>
            Dashboard
          </Link>)}

          {pathnames.map((name, index) => {
          const isLast = index === pathnames.length - 1;

          return isLast ? (
            <Typography key={name} color="text.primary" sx={{cursor: 'default'}}>
              {name === 'Dashboard' ? null : name}
            </Typography>
          ) : (
            <Link
            key={name}
            sx={{textDecoration: 'none', cursor: 'pointer'}} 
            color="text.primary"
            aria-current="page"
            onClick={() => {
              switch (name) {
              case 'MyAccount':
                navigate(Pages.MYACCOUNT);
                break;
              case 'Recipe':
                navigate(Pages.USERRECIPE);
                break;
              default:
                navigate(Pages.DASHBOARD);
            }
            }}
            >
              {name}
            </Link>
          );
        })}
        </Breadcrumbs>
      )}
        </Box>
    );
}

export default Header;