import { createRouter, createWebHistory } from 'vue-router';
import { AuthService } from '@/services/auth.service.ts'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/movies',
      name: 'movies',
      component: () => import('../views/MoviesView.vue'),
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('../views/LoginView.vue'),
      beforeEnter: (to, from, next) => {
        if (!AuthService.verifyTokenIsExpired()){
          next({ name: 'movies' });
        }
        else {
          next();
        }
      },
    },
    {
      path: '/register',
      name: 'register',
      component: () => import('../views/RegisterView.vue'),
      beforeEnter: (to, from, next) => {
        if (!AuthService.verifyTokenIsExpired()){
          next({ name: 'movies' });
        }
        else {
          next();
        }
      },
    },
    {
      path: '/movies/:id',
      name: 'movieDetails',
      component: () => import('../views/MovieDetailView.vue'),
    },
    {
      path: '/movies/create',
      name: 'createMovie',
      component: () => import('../views/CreateMovieView.vue'),
      beforeEnter: (to, from, next) =>{
        if (AuthService.verifyTokenIsExpired()){
          next({ name: 'login' });
        }
        else if (!AuthService.isModOrAdmin()) {
          next({name: 'movies'});
        }
        else {
          next();
        }
      }
    }
  ],
});

export default router;
