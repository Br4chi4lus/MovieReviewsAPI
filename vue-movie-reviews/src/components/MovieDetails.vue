<script setup lang="ts">
import { useRoute } from 'vue-router';
import MovieReviews from '@/components/MovieReviews.vue';
import CreateReview from '@/components/CreateReview.vue';
import { useMoviesStore } from '@/stores/movies.ts';
import { onMounted, ref } from 'vue';
import api from '@/axios.ts';
import type { MovieDto } from '@/dtos/movie.dto.ts';
import { formatDate } from '../utils.ts'

const route = useRoute();
const id = Number(route.params.id);
const moviesStore = useMoviesStore();
const movie = ref<MovieDto | null>(null);
const error = ref<string | null>(null);
onMounted(async () => {
  const existingMovie = moviesStore.findMovieById(id);
  if (!existingMovie) {
    try {
      const response = await api.get<MovieDto>('movies/' + id);
      movie.value = response.data;
    } catch (err) {
      console.log(err);
    }
  } else movie.value = existingMovie;
});
</script>

<template>
  <div v-if="movie">
    <h2>{{ movie.title + ' ' + movie.averageRating }}</h2>
    <h3>{{ formatDate(movie.dateOfPremiere) }}</h3>
    <h3>{{ movie.director }}</h3>
    <p>{{ movie.description }}</p>
  </div>
  <div v-else>{{ error }}</div>
  <CreateReview :id="id" />
  <MovieReviews :movie-id="id" />
</template>

<style scoped></style>
