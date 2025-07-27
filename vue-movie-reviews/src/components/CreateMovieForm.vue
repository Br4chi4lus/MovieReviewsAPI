<script setup lang="ts">
import { ref } from 'vue';
import type { CreateMovieDto } from '@/dtos/create.movie.dto.ts';
import api from '@/axios.ts';
import { isAxiosError } from 'axios';
import router from '@/router';

const createMovieDto = ref<CreateMovieDto>({
  title: '',
  description: '',
  directorFirstName: '',
  directorLastName: '',
  dateOfPremiere: '',
});
const error = ref<string | null>(null);
const loading = ref<boolean>(false);

async function onSubmit() {
  loading.value = true;
  try {
    if (createMovieDto.value.description === '') {
      createMovieDto.value.description = null;
    }
    if (createMovieDto.value.dateOfPremiere === '') {
      createMovieDto.value.dateOfPremiere = null;
    }
    const response = await api.post<CreateMovieDto>('movies', createMovieDto.value);
    const location = response.headers['location'];
    console.log(response.headers);
    router.push(location.split('api')[1]);
  } catch (err) {
    console.log(err);
    if (isAxiosError(err) && err.response) {
      const statusCode = err.response.status;
      if (statusCode === 401) {
        error.value = 'You are not authorized to create movie entry or you are not logged in.';
      } else if (statusCode === 400) {
        if (err.response.data && err.response.data.errors) {
          const data = err.response.data;
          const flatErrors = Object.values(data.errors).flat();
          error.value = flatErrors.join(', ');
        } else error.value = 'Unknown error occurred.';
      } else error.value = 'Unknown error occurred.';
    } else error.value = 'Unknown error occurred.';
  } finally {
    loading.value = false;
  }
}
</script>

<template>
  <div>
    <h3>Create Movie Entry</h3>
    <form @submit.prevent="onSubmit">
      <label for="title">Title:</label>
      <input type="text" name="title" id="title" v-model="createMovieDto.title" required />
      <label for="directorFirstName">Title:</label>
      <input
        type="text"
        name="directorName"
        id="directorName"
        v-model="createMovieDto.directorFirstName"
        required
      />
      <label for="directorLastName">Title:</label>
      <input
        type="text"
        name="directorLastName"
        id="directorLastName"
        v-model="createMovieDto.directorLastName"
        required
      />
      <label for="dateOfPremiere">Date of Premiere:</label>
      <input
        type="date"
        name="dateOfPremiere"
        id="dateOfPremiere"
        v-model="createMovieDto.dateOfPremiere"
      />
      <label for="description">Description:</label>
      <textarea
        name="description"
        id="description"
        v-model="createMovieDto.description"
        rows="10"
        cols="40"
      ></textarea>
      <button type="submit" :disabled="loading">Submit</button>
    </form>
    <div v-if="error" class="errors">
      {{ error }}
    </div>
  </div>
</template>

<style scoped>
div.errors {
  display: flex;
  color: red;
}
</style>
