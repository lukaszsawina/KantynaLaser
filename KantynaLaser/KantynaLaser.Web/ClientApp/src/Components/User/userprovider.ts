import { useEffect, useState } from "react";
import { UserAccount } from "../../api/userAccount";
import { APIget } from "../../api/api";
import Cookies from "js-cookie";

export const useUser = () => {
  const [user, setUser] = useState<UserAccount | null>(null);

  const fetchData = async () => {
      try {
          var token = Cookies.get("authToken");

          if (token) {
              const currentUser = await APIget<UserAccount>('Users/current');
              setUser(currentUser);
          } else {
              setUser(null);
          }
      } catch (error) {
          console.error('Błąd w pobieraniu danych użytkownika:', error);
      }
  };

  const refreshUser = async () => {
      await fetchData();
  };

  useEffect(() => {
      fetchData();
  }, []);

  return { user, refreshUser };
};