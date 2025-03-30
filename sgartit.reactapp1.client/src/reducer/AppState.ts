import { Todo } from "../models/Todo";

export const ACT = {
  None: "None",
  SearchTodoAsync: "SearchTodoAsync",
  SaveTodoAsync: "SaveTodoAsync",
  DeleteTodoAsync: "DeleteTodoAsync"
} as const;

export type ActionTypeAsyncValues = typeof ACT[keyof typeof ACT];

export type AppState = {
  actionTypeAsync: ActionTypeAsyncValues;  // usato in useEffect per invocare le chiamate alle API asincrone
  error?: string;
  loading: boolean;
  text: string;
  todos: Todo[];
  selectedTodo?: Todo | undefined;
  saveTodo?: Todo | undefined;
  showDelete: boolean;
  showEdit: boolean;
};

export const AppInitialState: AppState = {
  actionTypeAsync: ACT.None,
  error: undefined,
  loading: false,
  text: '',
  todos: [],
  selectedTodo: undefined,
  saveTodo: undefined,
  showDelete: false,
  showEdit: false
};