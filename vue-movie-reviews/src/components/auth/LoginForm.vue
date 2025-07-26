<script setup lang="ts">
import { isAxiosError } from 'axios';
import type { LoginDto } from '@/dtos/login.dto.ts';
import { ref } from 'vue';
import router from '@/router';
import api from '@/axios.ts';
import { isAuthenticated, userIsModOrAdmin } from '@/components/auth/auth.ts'
import { AuthService } from '@/services/auth.service.ts'

const loginDto = ref<LoginDto>({ email: '', password: '' });
const error = ref<string | null>(null);
const loading = ref<boolean>(false);

async function onSubmit() {
  if (!loginDto.value.email || !loginDto.value.password) {
    error.value = 'Email and password are required';
    return;
  }
  loading.value = true;
  try {
    const response = await api.post<string>('account/login', loginDto.value);
    localStorage.setItem('token', response.data);
    isAuthenticated.value = !AuthService.verifyTokenIsExpired();
    userIsModOrAdmin.value = AuthService.isModOrAdmin();
    router.push('/movies');
  } catch (err) {
    if (isAxiosError(err)) {
      if (err.response) {
        const data = err.response.data;
        if (data.message) {
          error.value = data.message;
        } else if (data.errors) {
          const flatErrors = Object.values(data.errors).flat();
          error.value = flatErrors.join(', ');
        } else {
          error.value = 'Unexpected error occurred';
        }
      } else {
        error.value = 'Unexpected error occurred';
      }
    } else {
      error.value = (err as Error).message;
    }
  }
  loading.value = false;
}
</script>

<template>
  <div>
    <form class="form" @submit.prevent="onSubmit">
      <label for="email">Email:</label>
      <input type="email" name="email" id="email" v-model="loginDto.email" />
      <label for="password">Password:</label>
      <input type="password" name="password" id="password" v-model="loginDto.password" />
      <button type="submit" :disabled="loading">Login</button>
      <div v-if="error" class="error">{{ error }}</div>
    </form>
  </div>
</template>

<style scoped>
.error {
  color: red;
}
</style>
