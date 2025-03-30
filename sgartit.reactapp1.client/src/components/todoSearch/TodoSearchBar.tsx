import { FC } from 'react';
import { InputText } from 'primereact/inputtext';
import './TodoSearchBar.css';
import { Button } from 'primereact/button';
import { IconField } from 'primereact/iconfield';
import { InputIcon } from 'primereact/inputicon';
import { ProgressSpinner } from 'primereact/progressspinner';
import { useAppContext } from '../../reducer/AppContext';

export type CommandAction = "clear" | "new" | "edit" | "delete" | "refresh" | "excel";

const TodoSearchBar: FC = () => {
  const { state, dispatch } = useAppContext();
  const { text, loading, selectedTodo } = state;

  const disabled = !selectedTodo;

  const handleTextChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    dispatch({ type: 'set-text', value: event.target.value });
  };

  return (
    <div className="todoSearch">
      <IconField iconPosition="left">
        <InputIcon className="pi pi-search" />
        <InputText placeholder="Search" value={text} onChange={handleTextChange} />
      </IconField>
      <Button label="Clear" icon="pi pi-times" onClick={() => dispatch({ type: 'set-text', value: '' })} disabled={text?.length === 0} />
      <Button label="Refresh" icon="pi pi-refresh" onClick={() => dispatch({ type: 'search-todo' })} />
      <a href={"/api/todo/excel?text=" + text} target="_blank" rel="noopener noreferrer" className="p-button font-bold" style={{ textDecoration: "none" }}><i className="pi pi-file-excel"></i> Excel</a>
      <a href={"/api/todo/pdf?text=" + text} target="_blank" rel="noopener noreferrer" className="p-button font-bold" style={{ textDecoration: "none" }}><i className="pi pi-file-pdf"></i> PDF</a>
      <Button label="New" icon="pi pi-plus" onClick={() => dispatch({ type: 'new-todo' })} />
      <Button label="Edit" icon="pi pi-pencil" onClick={() => dispatch({ type: 'set-show-edit', value: true })} disabled={disabled} />
      <Button label="Delete" icon="pi pi-trash" onClick={() => dispatch({ type: 'set-show-delete', value: true })} disabled={disabled} />
      *{text}*{selectedTodo?.id.toString()}*
      {loading === true && <div><ProgressSpinner style={{ width: '32px', height: '32px' }} strokeWidth="4" fill="var(--surface-ground)" animationDuration=".5s" /></div>}
    </div>
  );
};

export default TodoSearchBar;

//export const TodoSearch = ({ text, onChangeText }: TodoSearchProps) => <div className="todoSearch">
//  Search: <input type="text" value={text} onChange={(event: React.ChangeEvent<HTMLInputElement>) => onChangeText(event.target.value)} />
//</div>;