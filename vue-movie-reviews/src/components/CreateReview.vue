<script setup lang="ts">
import type { CreateReviewDto } from '@/dtos/create.review.dto.ts';
import { ref } from 'vue';
import api from '@/axios.ts';
import { isAxiosError } from 'axios'
import router from '@/router'

const props = defineProps<{
  id: number;
}>();
const createReview = ref<CreateReviewDto>({
  content: '',
  rating: 0,
});
async function onSubmit() {
  try {
    await api.post<CreateReviewDto>('movies/' + props.id + '/reviews', createReview.value);
  } catch (err) {
    if (isAxiosError(err) && err.response) {
      const statusCode = err.response.status;
      console.log(statusCode);
      if (statusCode == 401) {
        router.push('/login');
      }
    }
    console.log(err);
  }
}
</script>

<template>
  <div>
    <h3>Create your own review!</h3>
    <form @submit.prevent="onSubmit">
      <label for="rating" class="form-label">Rating:</label>
      <input
        type="number"
        id="rating"
        name="rating"
        min="1"
        max="10"
        v-model="createReview.rating"
      />
      <label for="content" class="form-label">Content:</label>
      <textarea
        id="content"
        name="content"
        rows="10"
        cols="40"
        placeholder="Your review goes here"
        v-model="createReview.content"
      ></textarea>
      <button type="submit">Submit</button>
    </form>
  </div>
</template>

<style scoped></style>
