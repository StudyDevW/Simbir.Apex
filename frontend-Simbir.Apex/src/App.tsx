// src/App.tsx
import React, { useState } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import UsersPage, { type User } from './users/Users';
import EditUserPage from './editing/Editing';
import CreateUserPage from './create/Create';

const App: React.FC = () => {
  const [users, setUsers] = useState<User[]>([
    {
      id: 1,
      fullName: 'Epeweee Ф.Б.',
      email: 'eremeev531@gmail.com',
      phone: '+7(962)-876-80-87',
      password: 'qwert123',
      status: 'online'
    },
    {
      id: 2,
      fullName: 'Марков А.М.',
      email: 'markovA01@gmail.com',
      phone: '+7(937)-823-84-32',
      password: 'helmik789',
      status: 'offline'
    },
    {
      id: 3,
      fullName: 'Орлеев К.С.',
      email: 'orleevKKK@gmail.com',
      phone: '+7(905)-421-92-92',
      password: 'gamalion456',
      status: 'online'
    }
  ]);

  const handleEditUser = (userId: number) => {
    window.location.href = `/edit-user/${userId}`;
  };

  const handleCreateUser = () => {
    window.location.href = '/create-user';
  };

  const handleDeleteUser = (userId: number) => {
    if (window.confirm('Вы уверены, что хотите удалить пользователя?')) {
      setUsers(prevUsers => prevUsers.filter(user => user.id !== userId));
    }
  };

  const handleSaveUser = (updatedUser: User) => {
    setUsers(prevUsers => 
      prevUsers.map(user => 
        user.id === updatedUser.id ? updatedUser : user
      )
    );
  };

  const handleCreateNewUser = (userData: Omit<User, 'id'>) => {
    const newUser: User = {
      ...userData,
      id: Math.max(...users.map(u => u.id)) + 1
    };
    setUsers(prevUsers => [...prevUsers, newUser]);
  };

  return (
    <Router>
      <Routes>
        <Route 
          path="/users" 
          element={
            <UsersPage 
              users={users}
              onEditUser={handleEditUser}
              onCreateUser={handleCreateUser}
              onDeleteUser={handleDeleteUser}
            />
          } 
        />
        <Route 
          path="/edit-user/:userId" 
          element={
            <EditUserPage 
              users={users}
              onSaveUser={handleSaveUser}
            />
          } 
        />
        <Route 
          path="/create-user" 
          element={
            <CreateUserPage 
              onCreateUser={handleCreateNewUser}
            />
          } 
        />
        <Route path="/" element={<UsersPage 
          users={users}
          onEditUser={handleEditUser}
          onCreateUser={handleCreateUser}
          onDeleteUser={handleDeleteUser}
        />} />
      </Routes>
    </Router>
  );
};

export default App;