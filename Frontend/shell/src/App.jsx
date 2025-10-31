import { Outlet } from 'react-router-dom'
import './App.css'
import Navigation from './components/Navigation/Naviagtion'
import { Toaster } from 'react-hot-toast';

function App() {

  return (
    <>
      <Navigation/>
      <div className='border border-bottom-0 border-dark rounded-top outlet'>
        <Outlet/>
      </div>
      <Toaster position="top-right" toastOptions={{ duration: 1000 }} />
    </>
  )
}

export default App
