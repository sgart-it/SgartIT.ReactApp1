import { Message } from 'primereact/message';
import { FC } from 'react';


const NotFoundPage: FC = () => {

  return (
    <Message severity="error" text="404 Page Not Found" />
  );

}

export default NotFoundPage;