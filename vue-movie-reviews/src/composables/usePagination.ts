import { ref } from 'vue';
import { ALLOWED_PAGE_SIZES } from '@/config.ts';

export function usePagination(fetchFn: (page: number, size: number) => Promise<void>) {
  const pageNumber = ref<number>(1);
  const pageSize = ref<number>(ALLOWED_PAGE_SIZES[0]);

  async function handlePageChange(newPage: number) {
    pageNumber.value = newPage;
    await fetchFn(pageNumber.value, pageSize.value);
  }

  async function handlePageSizeChange(newPageSize: number) {
    pageSize.value = newPageSize;
    pageNumber.value = 1;
    await fetchFn(pageNumber.value, pageSize.value);
  }

  return {
    pageNumber,
    pageSize,
    handlePageChange,
    handlePageSizeChange,
  };
}
