import { JSX, useState } from 'react';
import { useAppContext } from '../../reducer/AppContext';
import { InputText } from 'primereact/inputtext';
import { Button } from 'primereact/button';
import { Todo } from '../../models/Todo';
import { InputSwitch } from 'primereact/inputswitch';

export default function TodoEdit(): JSX.Element {
  const { state, dispatch } = useAppContext();
  const { selectedTodo: item } = state;

  const [title, setTitle] = useState<string>(item?.title ?? "");
  const [isCompleted, setIsCompleted] = useState<boolean>(item?.isCompleted ?? false);
  const [category, setCategory] = useState<string>(item?.category ?? "");

  const isNew = item?.id ?? 0 === 0;

  const handleSave = () => {
    const editItem: Todo = {
      id: item?.id ?? 0,
      title,
      isCompleted: isNew ? false : isCompleted,
      category,
      modified: new Date(),
      created: item?.created ?? new Date()
    };
    dispatch({ type: 'save-todo', value: editItem });
  };

  return (
    <div className="todoEdit">
      <div><label>Title</label> <InputText value={title} onChange={(e) => setTitle(e.target.value)} /></div>
      {isNew === false && <div><label>Completed</label> <InputSwitch checked={isCompleted} onChange={e => setIsCompleted(e.value)} /></div>}
      <div><label>Category</label> <InputText value={category} onChange={(e) => setCategory(e.target.value)} /></div>
      <div>*{isCompleted.toString()}*
        <Button label="Cancel" icon="pi pi-refresh" onClick={() => dispatch({ type: 'set-show-edit', value: false })} />
        <Button label="Save" icon="pi pi-times" onClick={handleSave} />

      </div>
    </div>
  );
}

