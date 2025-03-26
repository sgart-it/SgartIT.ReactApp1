import { Todo } from "../models/Todo";

//type SPItemTodo = {
//    Id: number,
//    Title: string,
//    Completed: boolean,
//    Category: string,
//    Created: Date,
//    Modified: Date
//};

//const mapToTodoItem = (item: SPItemTodo): Todo => {
//    return {
//        id: item.Id,
//        title: item.Title,
//        isCompleted: item.Completed ?? false,
//        category: item.Category,
//        modified: new Date(item.Modified),
//        created: new Date(item.Created)
//    }
//};

const mapDate = (item: Todo): void => {
  item.modified = new Date(item.modified);
  item.created = new Date(item.created);
};


const search = async (text: string | undefined): Promise<Todo[] | undefined> => {
  const response = await fetch('api/todo?text=' + (text ?? ''));
  if (response.ok) {
    const items = await response.json() as Todo[];

    items.forEach(item => mapDate(item));

    return items;
  }
  throw new Error(response.statusText);
};

const save = async (todo: Todo): Promise<void> => {
  if (todo.id !== 0) {
    // edit
    await fetch('api/todo/' + todo.id, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(todo)
    });
  } else {
    // add
    await fetch('api/todo', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(todo)
    });
  }
};

const remove = async (id: number): Promise<void> => {
  await fetch('api/todo/' + id, { method: 'DELETE' });
};


export const TodoService = {
  search,
  save,
  remove
};