import { defineStore } from 'pinia';
import type { MovieDto } from '@/dtos/movie.dto.ts';

export const useMoviesStore = defineStore('movies', {
  state: () => ({
    movies: [] as MovieDto[],
    lastUpdated: null as Date | null,
  }),
  actions: {
    setMovies(movies: MovieDto[]) {
      this.movies = movies;
      this.lastUpdated = new Date();
    },
    clear() {
      this.movies = [] as MovieDto[];
      this.lastUpdated = null as Date | null;
    },
    shouldUpdate() {
      if (!this.lastUpdated) return true;
      const now = new Date();
      const diff = now.getTime() - this.lastUpdated.getTime();

      return diff > 5 * 60 * 1000;
    },
    findMovieById(id: number) {
      return this.movies.find((movie) => movie.id === id);
    },
  },
});
