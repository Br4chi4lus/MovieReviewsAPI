import { defineStore } from 'pinia';
import type { ReviewDto } from '@/dtos/review.dto.ts';
import type { PagedResult } from '@/dtos/paged.result.ts';
import { ALLOWED_PAGE_SIZES } from '@/config.ts';

export const useReviewsStore = defineStore('reviews', {
  state: () => ({
    pagedReviews: {
      items: [],
      totalPages: 0,
      totalItemsCount: 0,
      itemFrom: 0,
      itemTo: 0,
    } as PagedResult<ReviewDto>,
    lastUpdated: null as Date | null,
    movieId: null as number | null,
    pageSize: ALLOWED_PAGE_SIZES[0] as number,
  }),
  actions: {
    setReviews(pagedReviews: PagedResult<ReviewDto>, movieId: number) {
      this.pagedReviews = pagedReviews;
      this.lastUpdated = new Date();
      this.movieId = movieId;
    },
    setPageSize(size: number) {
      this.pageSize = size;
    },
    clear() {
      this.pagedReviews.items = [];
      this.pagedReviews.totalPages = 0;
      this.pagedReviews.itemFrom = 0;
      this.pagedReviews.itemTo = 0;
      this.pagedReviews.totalItemsCount = 0;
      this.lastUpdated = null as Date | null;
      this.movieId = null as number | null;
      this.pageSize = ALLOWED_PAGE_SIZES[0];
    },
    shouldUpdate(movieId: number, pageSize: number, pageNumber: number) {
      if (this.movieId != movieId || !this.lastUpdated) return true;

      if (this.pageSize !== pageSize) return true;

      if (pageNumber !== Math.floor(this.pagedReviews.itemFrom / this.pageSize)) return true;

      const now = new Date();
      const diff = now.getTime() - this.lastUpdated.getTime();

      return diff > 5 * 60 * 1000;
    },
  },
});
