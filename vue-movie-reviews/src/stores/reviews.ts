import { defineStore } from 'pinia';
import type { ReviewDto } from '@/dtos/review.dto.ts';

export const useReviewsStore = defineStore('reviews', {
  state: () => ({
    reviews: [] as ReviewDto[],
    lastUpdated: null as Date | null,
    movieId: null as number | null,
  }),
  actions: {
    setReviews(reviews: ReviewDto[], moviewId: number) {
      this.reviews = reviews;
      this.lastUpdated = new Date();
      this.movieId = moviewId;
    },
    clear() {
      this.lastUpdated = null as Date | null;
      this.movieId = null as number | null;
    },
    shouldUpdate(movieId: number) {
      if (this.movieId != movieId || !this.lastUpdated) return true;
      const now = new Date();
      const diff = now.getTime() - this.lastUpdated.getTime();

      return diff > 5 * 60 * 1000;
    },
  },
});
