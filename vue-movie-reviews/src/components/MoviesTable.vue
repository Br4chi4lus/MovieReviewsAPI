<script setup lang="ts">
import { ref, onMounted } from 'vue';
import type { MovieDto } from '@/dtos/movie.dto.ts';
import { API_URL } from '@/config.ts';
import MovieItem from '@/components/MovieItem.vue';
import { useMoviesStore } from '@/stores/movies.ts';
import type { PagedResult } from '@/dtos/paged.result.ts';
import PaginationControls from '@/components/PaginationControls.vue';
import { usePagination } from '@/composables/usePagination.ts';
import { AuthService } from '@/services/auth.service.ts';
import api from '@/axios.ts';
import router from '@/router';
import { formatDate } from '@/utils.ts';

const movies = ref<MovieDto[]>([]);
const loading = ref<boolean>(true);
const error = ref<string | null>(null);
const moviesStore = useMoviesStore();
const totalPages = ref<number>(0);
const { pageNumber, pageSize, handlePageChange, handlePageSizeChange } = usePagination(fetchMovies);

async function fetchMovies(page: number, pageSize: number) {
  try {
    const response = await api.get<PagedResult<MovieDto>>(API_URL + 'movies', {
      params: { PageNumber: page, PageSize: pageSize },
    });
    movies.value = response.data.items;
    totalPages.value = response.data.totalPages;
    moviesStore.setPageSize(pageSize);
    moviesStore.setMovies(response.data);
  } catch (err) {
    error.value = (err as Error).message;
  } finally {
    loading.value = false;
  }
}

async function deleteMovie(id: number) {
  try {
    const response = await api.delete('movies/' + id);

    if (response.status === 204) await fetchMovies(pageNumber.value, pageSize.value);
  } catch (error) {
    console.log(error);
  }
}

onMounted(async () => {
  pageSize.value = moviesStore.pageSize;
  if (!moviesStore.shouldUpdate(pageSize.value, pageNumber.value)) {
    movies.value = moviesStore.pagedMovies.items;
    totalPages.value = moviesStore.pagedMovies.totalPages;
    return;
  } else {
    await fetchMovies(pageNumber.value, pageSize.value);
  }
});
</script>

<template>
  <div>
    <label for="newMovie">Create new movie entry:</label>
    <button v-if="AuthService.isModOrAdmin()" id="newMovie" @click="router.push(`/movies/create`)">
      +
    </button>
  </div>
  <div v-for="movie in movies" :key="movie.id">
    <router-link :to="`/movies/${movie.id}`">
      <MovieItem
        :title="movie.title"
        :director="movie.director"
        :dateOfPremiere="formatDate(movie.dateOfPremiere)"
      />
    </router-link>
    <div>
      <button v-if="AuthService.isModOrAdmin()" @click="deleteMovie(movie.id)" class="danger">
        Delete
      </button>
    </div>
  </div>
  <PaginationControls
    :page-number="pageNumber"
    :total-page-count="totalPages"
    :page-size="pageSize"
    @pageChange="handlePageChange"
    @pageSizeChange="handlePageSizeChange"
  />
</template>

<style scoped>
a {
  display: block;
}

.danger {
  color: red;
  font-weight: bold;
  border: none;
  background: none;
  cursor: pointer;
}
</style>
