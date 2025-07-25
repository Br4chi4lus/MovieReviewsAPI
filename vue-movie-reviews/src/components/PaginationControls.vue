<script setup lang="ts">
import { ALLOWED_PAGE_SIZES } from '@/config.ts';
import { computed } from 'vue';

const props = defineProps<{
  pageNumber: number;
  totalPageCount: number;
  pageSize: number;
}>();
const emits = defineEmits<{
  (e: 'pageChange', pageNumber: number): void;
  (e: 'pageSizeChange', pageSize: number): void;
}>();

const pagesToShow = computed(() => {
  const pages: number[] = [];
  const start = Math.max(1, props.pageNumber - 2);
  const end = Math.min(props.totalPageCount, props.pageNumber + 2);

  for (let i = start; i <= end; i++) {
    pages.push(i);
  }

  return pages;
});

function goToPreviousPage() {
  if (props.pageNumber > 1) {
    emits('pageChange', props.pageNumber - 1);
  }
}

function goToNextPage() {
  if (props.pageNumber < props.totalPageCount) {
    emits('pageChange', props.pageNumber + 1);
  }
}

function changePageSize(event: Event) {
  const value = parseInt((event.target as HTMLInputElement).value);
  emits('pageSizeChange', value);
}
</script>

<template>
  <div>
    <button @click="goToPreviousPage" :disabled="props.pageNumber <= 1">Previous</button>
    <button
      v-for="page in pagesToShow"
      :key="page"
      :disabled="page === props.pageNumber"
      @click="() => emits('pageChange', page)"
    >
      {{ page }}
    </button>
    <button @click="goToNextPage" :disabled="props.pageNumber >= props.totalPageCount">Next</button>
    <label>
      Page size:
      <select :value="pageSize" @change="changePageSize">
        <option v-for="size in ALLOWED_PAGE_SIZES" :key="size" :value="size">{{ size }}</option>
      </select>
    </label>
  </div>
</template>

<style scoped>
button {
  background: none;
  border: none;
  cursor: pointer;
}

button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
</style>
