import { JSX, useState } from 'react';
import { InputText } from 'primereact/inputtext';
import { Button } from 'primereact/button';
import { Todo } from '../../models/Todo';
import { InputSwitch } from 'primereact/inputswitch';

type TodoEditProps = {
  item?: Todo;
  onComplete: (item?: Todo) => void;
};

export default function TodoEdit({ item, onComplete }: TodoEditProps): JSX.Element {
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
    onComplete(editItem);
  };

  return (
    <div className="todoEdit">
      <div><label>Title</label> <InputText value={title} onChange={(e) => setTitle(e.target.value)} /></div>
      {isNew === false && <div><label>Completed</label> <InputSwitch checked={isCompleted} onChange={e => setIsCompleted(e.value)} /></div>}
      <div><label>Category</label> <InputText value={category} onChange={(e) => setCategory(e.target.value)} /></div>
      <div>*{isCompleted.toString()}*
        <Button label="Cancel" icon="pi pi-refresh" onClick={() => onComplete()} />
        <Button label="Save" icon="pi pi-times" onClick={handleSave} />

      </div>
    </div>
  );
}

