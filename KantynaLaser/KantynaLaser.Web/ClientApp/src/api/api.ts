import Cookies from "js-cookie";

const baseURL = 'https://localhost:5001/api';

const APIlogin = async (credentials : any) => {
    const response = await fetch(`${baseURL}/Identity/login`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(credentials),
    });
  
    if (!response.ok) {
      throw new Error('Błąd logowania');
    }
  
    const token = await response.text(); // Odczytaj token jako tekst
    return token;
  };

const APIget = async <T>(endpoint : string): Promise<T> => {
    const response = await fetch(`${baseURL}/${endpoint}`, {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${Cookies.get('authToken')}`
        },
      });

      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }
  
      const data: T = await response.json();
      return data;
  };

   

const APIpost = async (endpoint : string, body : any) => {
  const response = await fetch(`${baseURL}/${endpoint}`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(body),
  });
  const data = await response.text();
  return data;
};

const APIpostAuth = async (endpoint : string, body : any) => {
  const response = await fetch(`${baseURL}/${endpoint}`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${Cookies.get('authToken')}`
    },
    body: JSON.stringify(body),
  });
  const data = await response.text();
  return data;
};

const APIput = async (endpoint : string, body : any) => {
  const response = await fetch(`${baseURL}/${endpoint}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${Cookies.get('authToken')}`
    },
    body: JSON.stringify(body),
  });
  const data = await response.text();
  return data;
};

const APIdelete = async (endpoint : string) => {
  const response = await fetch(`${baseURL}/${endpoint}`, {
    method: 'DELETE',
    headers: {
      'Authorization': `Bearer ${Cookies.get('authToken')}`
    },
  });
  const data = await response.json();
  return data;
};

export { APIget, APIpost, APIput, APIdelete, APIlogin, APIpostAuth };