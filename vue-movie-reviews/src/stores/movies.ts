import { defineStore } from 'pinia';
import type { MovieDto } from '@/dtos/movie.dto.ts';
import type { PagedResult } from '@/dtos/paged.result.ts';
import { ALLOWED_PAGE_SIZES } from '@/config.ts';

export const useMoviesStore = defineStore('movies', {
  state: () => ({
    pagedMovies: {
      items: [],
      totalPages: 0,
      totalItemsCount: 0,
      itemFrom: 0,
      itemTo: 0,
    } as PagedResult<MovieDto>,
    lastUpdated: null as Date | null,
    pageSize: ALLOWED_PAGE_SIZES[0] as number,
  }),
  actions: {
    setMovies(pagedMovies: PagedResult<MovieDto>) {
      this.pagedMovies = pagedMovies;
      this.lastUpdated = new Date();
    },
    setPageSize(pageSize: number) {
      this.pageSize = pageSize;
    },
    clear() {
      this.pagedMovies.items = [] as MovieDto[];
      this.pagedMovies.totalPages = 0;
      this.pagedMovies.itemFrom = 0;
      this.pagedMovies.itemTo = 0;
      this.pagedMovies.totalItemsCount = 0;
      this.lastUpdated = null as Date | null;
      this.pageSize = ALLOWED_PAGE_SIZES[0];
    },
    shouldUpdate(pageSize: number, pageNumber: number) {
      if (!this.lastUpdated) return true;

      if (pageSize !== this.pageSize) return true;

      if (pageNumber !== Math.floor(this.pagedMovies.itemFrom / this.pageSize) + 1) return true;

      const now = new Date();
      const diff = now.getTime() - this.lastUpdated.getTime();

      return diff > 5 * 60 * 1000;
    },
    findMovieById(id: number) {
      return this.pagedMovies.items.find((movie) => movie.id === id);
    },
  },
});
