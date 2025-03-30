import { FC } from "react";
import { Column } from "primereact/column";
import { DataTable } from "primereact/datatable";
import { Todo } from "../../models/Todo";
import { ShowDate } from "../showDate/ShowDate";
import { useAppContext } from "../../reducer/AppContext";

const TodoListView: FC = () => {
  const { state, dispatch } = useAppContext();
  const { todos, selectedTodo } = state;

  const modifiedTemplate = (rowData: Todo) => <ShowDate date={rowData.modified} />;
  const createdTemplate = (rowData: Todo) => <ShowDate date={rowData.modified} />;

  return (
    <DataTable value={todos ?? []} tableStyle={{ minWidth: '50rem' }}
      selectionMode={"single"}
      dataKey="id"
      metaKeySelection={false}
      emptyMessage=""
      selection={selectedTodo}
      onSelectionChange={e => dispatch({ type: 'set-selected-todo', value: e.value as Todo })}
    >
      <Column field="title" header="Title"></Column>
      <Column field="isCompleted" header="Completed"></Column>
      <Column field="category" header="Category"></Column>
      <Column header="Created" body={modifiedTemplate}></Column>
      <Column key="modified" header="Modified" body={createdTemplate}></Column>

    </DataTable>
  );
}

export default TodoListView;