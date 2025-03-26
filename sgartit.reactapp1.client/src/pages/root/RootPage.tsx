import { FC, useEffect, useState } from 'react';
import './RootPage.css';
import NavigationBar from '../../components/navigationBar/NavigationBar.tsx'
import { Todo } from '../../models/Todo.ts';
import TodoSearchBar, { CommandAction } from '../../components/todoSearch/TodoSearchBar.tsx';
import TodoListView from '../../components/todoListView/TodoListView.tsx';
import { TodoService } from '../../services/todoService.ts';
import TodoEdit from '../../components/todoEdit/TodoEdit.tsx';
import { Message } from 'primereact/message';
import { ConfirmDialog } from 'primereact/confirmdialog';


const RootPage: FC = () => {
  const [todos, setTodos] = useState<Todo[]>();
  const [status, setStatus] = useState({ loading: false, error: "" });
  const [text, setText] = useState<string>("");
  const [selectedTodo, setSelectedTodo] = useState<Todo>();
  const [showDeleteDialog, setShowDeleteDialog] = useState<boolean>(false);
  const [showEdit, setShowEdit] = useState<boolean>(false);

  const populateTodoData = async () => {
    try {
      setStatus({ loading: true, error: "" });
      const items = await TodoService.search(text);
      setTodos(items);
      setStatus({ loading: false, error: "" });
    } catch (err) {
      setStatus({ loading: false, error: err?.toString() ?? ""});
    }
  };

  useEffect(() => {
    populateTodoData().catch(() => { });
  },[text]);


  const handleDialogDelete = async () => {
    console.debug("delete");
    if (selectedTodo !== undefined) {
      await TodoService.remove(selectedTodo?.id);
      setSelectedTodo(undefined);
      await populateTodoData();
    }
    setShowDeleteDialog(false);
  };

  const handleEditComplete = async (item?: Todo) => {
    console.debug("editComplete", item);
    if (item) {
      // save
      setSelectedTodo(undefined);
      await TodoService.save(item);
      await populateTodoData();
      //const itemSelected = todos?.find(x => x.id === item.id);
      //if(itemSelected){
      //  setSelectedTodo(itemSelected);
      //}
    }
    setShowEdit(false);
  };

  const handleCommand = async (action: CommandAction): Promise<void> => {
    console.debug("handleCommand", action);
    switch (action) {
      case "refresh":
        await populateTodoData();
        break;
      case "clear":
        setText("");
        break;
      case "new":
        setSelectedTodo(undefined);
        setShowEdit(true);
        break;
      case "edit":
        setShowEdit(true);
        break;
      case "delete":
        setShowDeleteDialog(true);
        break;
      default:
        break;
    }
  };


  return (
    <div>
      <h2 id="tableLabel">Todo</h2>
      <NavigationBar />
      <p>This component demonstrates fetching data from the server.</p>
      {showEdit
        ? <TodoEdit item={selectedTodo} onComplete={(item) => void handleEditComplete(item)} />
        : <>
          <TodoSearchBar text={text} selected={selectedTodo} loading={status.loading} onChangeText={text => setText(text)} onCommand={(action) => void handleCommand(action)} />
          {status.error !== "" && <Message severity="error" text={status.error} />}
          <TodoListView items={todos} selected={selectedTodo} onSelected={item => setSelectedTodo(item)} />
        </>
      }

      <ConfirmDialog group="declarative" visible={showDeleteDialog}
        message="Are you sure you want to proceed?"
        header="Confirmation"
        icon="pi pi-exclamation-triangle"
        defaultFocus="reject"
        onHide={() => setShowDeleteDialog(false)}
        accept={() => void handleDialogDelete()} />
    </div>
  );

}

export default RootPage;