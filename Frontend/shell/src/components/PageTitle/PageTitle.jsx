import { Link } from 'react-router-dom';
import { ArrowLeft } from "react-bootstrap-icons";
import './PageTitle.css'

const PageTitle = ({ title }) => {
    return (
        <>
            <div className='page-title p-3 fs-3 rounded fw-bold text-white mb-2'>
                <Link to='/' className='text-white text-decoration-none d-flex align-items-center'>
                    <ArrowLeft />
                    &nbsp;
                    <span className='mb-1'>{title}</span>
                </Link>
            </div>
        </>
    );
};

export default PageTitle;