import { Toaster } from 'react-hot-toast';
import { Outlet } from 'react-router-dom';

function App() {

  return (
    <>
      <Outlet />
      <Toaster position="top-right" toastOptions={{ duration: 1000 }} />
    </>
  )
}

export default App