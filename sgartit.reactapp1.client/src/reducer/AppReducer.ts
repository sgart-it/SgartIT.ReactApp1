import { Todo } from "../models/Todo";
import { ACT,  AppState } from "./AppState";

export type AppTypeAction = {
  type: 'search-todo' | 'loading' | 'loaded' | 'new-todo' | 'delete-todo';
};

export type AppTextAction = {
  type: 'set-text' | 'set-error';
  value?: string | undefined;
};

export type AppTodosAction = {
  type: 'set-todos';
  values: Todo[];
};

export type AppSetSelectedAction = {
  type: 'set-selected-todo' | 'save-todo';
  value: Todo;
};

export type AppSetBooleanAction = {
  type: 'set-show-delete' | 'set-show-edit';
  value: boolean;
};

export type AppAction = AppTypeAction
  | AppTextAction
  | AppTodosAction
  | AppSetSelectedAction
  | AppSetBooleanAction;

export const AppReducer = (state: AppState, action: AppAction) => {
  const { type } = action;

  switch (type) {
    case 'new-todo':
      return {
        ...state,
        actionTypeAsync: ACT.None,
        error: undefined,
        selectedTodo: undefined,
        showEdit: true
      };
    case 'loading':
      return {
        ...state,
        actionTypeAsync: ACT.None,
      };
    case 'set-text':
      return {
        ...state,
        actionTypeAsync: ACT.SearchTodoAsync,
        text: action.value ?? ''
      };

    case 'loaded':
      return {
        ...state,
        actionTypeAsync: ACT.None,
      };
    case 'set-todos':
      return {
        ...state,
        actionTypeAsync: ACT.None,
        todos: action.values,
        error: undefined,
        showDelete: false,
        showEdit: false
      };
    case 'set-selected-todo':
      return {
        ...state,
        actionTypeAsync: ACT.None,
        selectedTodo: action.value,
        error: undefined
      };
    case 'set-error':
      return {
        ...state,
        actionTypeAsync: ACT.None,
        error: action.value ?? '',
        loading: false
      };
    case 'set-show-delete':
      return {
        ...state,
        error: undefined,
        showDelete: action.value
      };
    case 'set-show-edit':
      return {
        ...state,
        actionTypeAsync: ACT.None,
        error: undefined,
        showEdit: action.value
      };
    case 'search-todo':
      return {
        ...state,
        actionTypeAsync: ACT.SearchTodoAsync,
        selectedTodo: undefined,
        error: undefined,
        showDelete: false,
        showEdit: false
      };
    case 'save-todo':
      return {
        ...state,
        actionTypeAsync: ACT.SaveTodoAsync,
        error: undefined,
        saveTodo: action.value
      };
    case 'delete-todo':
      return {
        ...state,
        actionTypeAsync: ACT.DeleteTodoAsync,
        error: undefined
      };
    default:
      //throw new Error(type);
      return state;
  }
};
