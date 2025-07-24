<script setup lang="ts">
import axios from 'axios';
import { ref, onMounted } from 'vue';
import type { MovieDto } from '@/dtos/movie.dto.ts';
import { API_URL } from '@/config.ts';
import MovieItem from '@/components/MovieItem.vue';
import { useMoviesStore } from '@/stores/movies.ts';
import { AuthService } from '@/services/auth.service.ts'

const movies = ref<MovieDto[]>([]);
const loading = ref<boolean>(true);
const error = ref<string | null>(null);
const moviesStore = useMoviesStore();

onMounted(async () => {
  console.log(AuthService.verifyTokenIsExpired());
  if (!moviesStore.shouldUpdate()) {
    movies.value = moviesStore.movies;
    return;
  }
  try {
    const response = await axios.get<MovieDto[]>(API_URL + 'movies');
    movies.value = response.data;
    moviesStore.setMovies(movies.value);
  } catch (err) {
    error.value = (err as Error).message;
  } finally {
    loading.value = false;
  }
});
</script>

<template>
  <li v-for="movie in movies" :key="movie.id">
    <router-link :to="`/movies/${movie.id}`">
      <MovieItem
        :title="movie.title"
        :director="movie.director"
        :dateOfPremiere="movie.dateOfPremiere.toString().split('T')[0]"
      />
    </router-link>
  </li>
</template>

<style scoped></style>
