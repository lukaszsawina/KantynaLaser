import { ReactNode, createContext, useContext, useEffect, useState } from 'react';
import { UserAccount } from '../../api/userAccount';
import { useUser } from './userprovider';

interface UserContextProps {
  user: UserAccount | null;
  refreshUser: () => void;
}

const UserContext = createContext<UserContextProps | null>(null);

interface UserProviderProps {
  children: ReactNode;
}

export const UserProvider: React.FC<UserProviderProps> = ({ children }) => {
  const { user, refreshUser: refreshUserData } = useUser();

  const [internalUser, setInternalUser] = useState<UserAccount | null>(user);

  useEffect(() => {
    setInternalUser(user);
  }, [user]);

  return (
    <UserContext.Provider value={{ user: internalUser, refreshUser: refreshUserData }}>
      {children}
    </UserContext.Provider>
  );
};

export const useUserContext = () => {
  const context = useContext(UserContext);

  if (!context) {
    throw new Error('useUserContext must be used within a UserProvider');
  }

  return context;
};