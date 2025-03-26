import { FC } from 'react';
import { InputText } from 'primereact/inputtext';
import './TodoSearchBar.css';
import { Button } from 'primereact/button';
import { IconField } from 'primereact/iconfield';
import { InputIcon } from 'primereact/inputicon';
import { Todo } from '../../models/Todo';
import { ProgressSpinner } from 'primereact/progressspinner';

export type CommandAction = "clear" | "new" | "edit" | "delete" | "refresh" | "excel";

export type TodoSearchProps = {
  text: string;
  selected?: Todo,
  loading: boolean,
  onChangeText: (text: string) => void;
  onCommand: (action: CommandAction) => void;
}

const TodoSearchBar: FC<TodoSearchProps> = ({ text, selected, loading, onChangeText, onCommand }) => {
  const disabled = !selected;

  const handleTextChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    onChangeText(event.target.value)
  };

  return (
    <div className="todoSearch">
      <IconField iconPosition="left">
        <InputIcon className="pi pi-search" />
        <InputText placeholder="Search" value={text} onChange={handleTextChange} />
      </IconField>
      <Button label="Clear" icon="pi pi-times" onClick={() => onCommand("clear")} disabled={text?.length === 0} />
      <Button label="Refresh" icon="pi pi-refresh" onClick={() => onCommand("refresh")} />
      <a href={"/api/todo/excel?text=" + text} target="_blank" rel="noopener noreferrer" className="p-button font-bold" style={{ textDecoration: "none" }}><i className="pi pi-file-excel"></i> Excel</a>
      <a href={"/api/todo/pdf?text=" + text} target="_blank" rel="noopener noreferrer" className="p-button font-bold" style={{ textDecoration: "none" }}><i className="pi pi-file-pdf"></i> PDF</a>
      <Button label="New" icon="pi pi-plus" onClick={() => onCommand("new")} />
      <Button label="Edit" icon="pi pi-pencil" onClick={() => onCommand("edit")} disabled={disabled} />
      <Button label="Delete" icon="pi pi-trash" onClick={() => onCommand("delete")} disabled={disabled} />
      *{text}*{selected?.id.toString()}*
      {loading === true && <div><ProgressSpinner style={{ width: '32px', height: '32px' }} strokeWidth="4" fill="var(--surface-ground)" animationDuration=".5s" /></div>}
    </div>
  );
};

export default TodoSearchBar;

//export const TodoSearch = ({ text, onChangeText }: TodoSearchProps) => <div className="todoSearch">
//  Search: <input type="text" value={text} onChange={(event: React.ChangeEvent<HTMLInputElement>) => onChangeText(event.target.value)} />
//</div>;