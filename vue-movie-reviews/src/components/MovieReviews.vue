<script setup lang="ts">
import { onMounted, ref } from 'vue';
import type { ReviewDto } from '@/dtos/review.dto.ts';
import { useReviewsStore } from '@/stores/reviews.ts';
import ReviewItem from '@/components/ReviewItem.vue';
import api from '@/axios.ts';

const props = defineProps<{
  movieId: number;
}>();

const reviews = ref<ReviewDto[]>([]);
const error = ref<string | null>(null);
const reviewStore = useReviewsStore();

onMounted(async () => {
  if (!reviewStore.shouldUpdate(props.movieId)) {
    reviews.value = reviewStore.reviews;
    return;
  }
  try {
    const response = await api.get<ReviewDto[]>('movies/' + props.movieId + '/reviews');
    reviews.value = response.data;
    reviewStore.setReviews(reviews.value, props.movieId);
  } catch (err) {
    console.log(err);
  }
});
</script>

<template>
  <h3 v-if="reviews.length === 0">No reviews for this movie</h3>
  <ReviewItem
    v-for="review in reviews"
    :key="review.id"
    :content="review.content"
    :author="review.userName"
    :rating="review.rating"
  />
</template>

<style scoped></style>
