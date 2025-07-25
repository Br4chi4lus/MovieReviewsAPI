<script setup lang="ts">
import { onMounted, ref } from 'vue';
import type { ReviewDto } from '@/dtos/review.dto.ts';
import { useReviewsStore } from '@/stores/reviews.ts';
import ReviewItem from '@/components/ReviewItem.vue';
import api from '@/axios.ts';
import type { PagedResult } from '@/dtos/paged.result.ts';
import { usePagination } from '@/composables/usePagination.ts';
import PaginationControls from '@/components/PaginationControls.vue';

const props = defineProps<{
  movieId: number;
}>();

const reviews = ref<ReviewDto[]>([]);

const error = ref<string | null>(null);
const reviewStore = useReviewsStore();
const totalPages = ref<number>(0);
const { pageNumber, pageSize, handlePageChange, handlePageSizeChange } =
  usePagination(fetchReviews);

async function fetchReviews(page: number, size: number) {
  try {
    const response = await api.get<PagedResult<ReviewDto>>('movies/' + props.movieId + '/reviews', {
      params: { PageNumber: page, PageSize: size },
    });
    reviews.value = response.data.items;
    totalPages.value = response.data.totalPages;
    reviewStore.setReviews(response.data, props.movieId);
    reviewStore.setPageSize(pageSize.value);
  } catch (err) {
    console.log(err);
  }
}

onMounted(async () => {
  pageSize.value = reviewStore.pageSize;
  if (!reviewStore.shouldUpdate(props.movieId, pageSize.value, pageNumber.value)) {
    reviews.value = reviewStore.pagedReviews.items;
    totalPages.value = reviewStore.pagedReviews.totalPages;
    return;
  } else {
    await fetchReviews(pageNumber.value, pageSize.value);
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
  <PaginationControls
    :page-number="pageNumber"
    :total-page-count="totalPages"
    :page-size="pageSize"
    @pageSizeChange="handlePageSizeChange"
    @pageChange="handlePageChange"
  />
</template>

<style scoped></style>
