import { FC } from "react";
import { Column } from "primereact/column";
import { DataTable } from "primereact/datatable";
import { Todo } from "../../models/Todo";
import { ShowDate } from "../showDate/ShowDate";

type TodoListViewProps = {
  items: Todo[] | undefined,
  selected: Todo | undefined;
  onSelected: (item: Todo) => void
}

const TodoListView: FC<TodoListViewProps> = ({ items, selected, onSelected }) => {
  const modifiedTemplate = (rowData: Todo) => {
    return <ShowDate date={rowData.modified} />
  };

  const createdTemplate = (rowData: Todo) => <ShowDate date={rowData.modified} />;

  return (
    <DataTable value={items ?? []} tableStyle={{ minWidth: '50rem' }}
      selectionMode={"single"}
      dataKey="id"
      metaKeySelection={false}
      emptyMessage=""
      selection={selected}
      onSelectionChange={e => onSelected(e.value as Todo)}
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