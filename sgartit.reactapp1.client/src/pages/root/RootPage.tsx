import { FC } from 'react';
import { useAppContext } from '../../reducer/AppContext.ts';
import './RootPage.css';
import NavigationBar from '../../components/navigationBar/NavigationBar.tsx'
import TodoSearchBar from '../../components/todoSearch/TodoSearchBar.tsx';
import TodoListView from '../../components/todoListView/TodoListView.tsx';
import TodoEdit from '../../components/todoEdit/TodoEdit.tsx';
import { Message } from 'primereact/message';
import { ConfirmDialog } from 'primereact/confirmdialog';


const RootPage: FC = () => {
  const { state, dispatch } = useAppContext();
  const { error, showDelete, showEdit } = state;

  return (
    <div>
      <h2 id="tableLabel">Todo</h2>
      <NavigationBar />
      <p>This component demonstrates fetching data from the server.</p>
      {showEdit
        ? <TodoEdit />
        : <>
          <TodoSearchBar />
          {error && <Message severity="error" text={error} />}
          <TodoListView />
        </>
      }

      <ConfirmDialog group="declarative" visible={showDelete}
        message="Are you sure you want to proceed?"
        header="Confirmation"
        icon="pi pi-exclamation-triangle"
        defaultFocus="reject"
        onHide={() => dispatch({ type: 'set-show-delete', value: false })}
        accept={() => dispatch({ type: 'delete-todo' })} />
    </div>
  );

}

export default RootPage;