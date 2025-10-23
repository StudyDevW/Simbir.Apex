import { Outlet } from 'react-router-dom'
import './App.css'
import Navigation from './components/Navigation/Naviagtion'

function App() {

  return (
    <>
      <Navigation/>
      <div className='border border-bottom-0 border-dark rounded-top outlet'>
        <Outlet/>
      </div>
    </>
  )
}

export default App
