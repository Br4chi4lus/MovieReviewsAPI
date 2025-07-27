<script setup lang="ts">
import type { UpdateMovieDto } from '@/dtos/update.movie.dto.ts';
import { onMounted, ref } from 'vue';
import type { MovieDto } from '@/dtos/movie.dto.ts';
import { useMoviesStore } from '@/stores/movies.ts';
import { useRoute } from 'vue-router';
import api from '@/axios.ts';
import { formatDateIfNullReturnsEmptyString } from '@/utils.ts';
import { isAxiosError } from 'axios';
import router from '@/router';

const route = useRoute();
const id = Number(route.params.id);

const updateMovieDto = ref<UpdateMovieDto>({
  description: '',
  dateOfPremiere: '',
});
const moviesStore = useMoviesStore();
const movieDto = ref<MovieDto | null>(null);
const error = ref<string | null>(null);
const loading = ref<boolean>(false);
async function onSubmit() {
  if (updateMovieDto.value.dateOfPremiere === '') updateMovieDto.value.dateOfPremiere = null;
  if (updateMovieDto.value.description === '') updateMovieDto.value.description = null;
  try {
    const response = await api.put<UpdateMovieDto>(`/movies/${id}`, updateMovieDto.value);
    router.push(`/movies/${id}`);
  } catch (err) {
    console.error(err);
    if (isAxiosError(err) && err.response) {
      const data = err.response.data;
      if (data.message) {
        error.value = data.message;
      } else if (data.errors) {
        const flatErrors = Object.values(data.errors).flat();
        error.value = flatErrors.join(',');
      } else {
        error.value = 'Unknown error occurred.';
      }
    } else {
      error.value = 'Unknown error occurred.';
    }
  }
}
onMounted(async () => {
  loading.value = true;
  const existingMovie = moviesStore.findMovieById(id);
  if (!existingMovie) {
    try {
      const response = await api.get<MovieDto>('movies/' + id);
      movieDto.value = response.data;
    } catch (err) {
      console.log(err);
      if (isAxiosError(err) && err.response) {
        const data = err.response.data;
        if (data.message) {
          error.value = data.message;
        } else if (data.errors) {
          const flatErrors = Object.values(data.errors).flat();
          error.value = flatErrors.join(',');
        } else {
          error.value = 'Unknown error occurred.';
        }
      } else {
        error.value = 'Unknown error occurred.';
      }
    } finally {
      loading.value = false;
    }
  } else {
    movieDto.value = existingMovie;
  }
  if (movieDto.value) {
    updateMovieDto.value.dateOfPremiere = formatDateIfNullReturnsEmptyString(
      movieDto.value.dateOfPremiere,
    );
    updateMovieDto.value.description = movieDto.value.description ? movieDto.value.description : '';
  }
});
</script>

<template>
  <div v-if="movieDto">
    <label for="updateMovieForm">{{ movieDto.title }}</label>
    <form @submit.prevent="onSubmit" id="updateMovieForm">
      <label for="dateOfPremiere">Date of Premiere:</label>
      <input
        type="date"
        name="dateOfPremiere"
        id="dateOfPremiere"
        v-model="updateMovieDto.dateOfPremiere"
      />
      <label for="description">Description:</label>
      <textarea
        name="description"
        id="description"
        v-model="updateMovieDto.description"
        rows="10"
        cols="40"
      ></textarea>
      <button type="submit" :disabled="loading">Submit</button>
    </form>
  </div>
  <div v-if="error">
    {{ error }}
  </div>
</template>

<style scoped></style>
