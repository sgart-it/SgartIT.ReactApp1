import { FC} from 'react';
import { useRoutes } from 'react-router-dom';
import AboutPage from '../pages/about/AboutPage';
import RootPage from '../pages/root/RootPage';
import NotFoundPage from '../pages/error/NotFoundPage';

const Router: FC = () => {
  const routes = [
    {
      path: "/",
      element: <RootPage />
    },
    {
      path: "about",
      element: <AboutPage />
    },
    {
      path: "*",
      element: <NotFoundPage /> /*,
      children: [
      {
        path: "messages",
        element: <DashboardMessages />,
      },
      { path: "tasks", element: <DashboardTasks /> },
    ],*/
    },
  ];

  return useRoutes(routes);
}

export default Router;