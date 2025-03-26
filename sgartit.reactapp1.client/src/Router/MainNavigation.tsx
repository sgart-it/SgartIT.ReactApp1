import { FC } from 'react';
import { MegaMenu } from 'primereact/megamenu';
import { MenuItem } from 'primereact/menuitem';
import { useNavigate } from 'react-router-dom';

const MainNavigation: FC = () => {
  const navigate = useNavigate();

  const items: MenuItem[] = [
    {
      label: 'Home',
      icon: 'pi pi-home',
      command:  () => void navigate("/")
    },
    {
      label: 'Count',
      icon: 'pi pi-stopwatch',
      command: () => void navigate("/count")
    },
    {
      label: 'About',
      icon: 'pi pi-info-circle',
      command: () => void navigate("/about")
      //template: () => <Link to="/about" />
    },
    {
      label: 'Sgart.it',
      icon: 'pi pi-link',
      url: 'https://www.sgart.it/',
      target: '_blank'
    }
  ];

  return <MegaMenu model={items} breakpoint="960px" />;
}

export default MainNavigation;