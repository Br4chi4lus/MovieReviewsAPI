export interface PagedResult<Type> {
  items: Type[];
  totalPages: number;
  totalItemsCount: number;
  itemFrom: number;
  itemTo: number;
}
