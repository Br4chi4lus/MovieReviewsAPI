<script setup lang="ts">
import type { RegisterDto } from '@/dtos/register.dto.ts';
import { ref } from 'vue';
import { isAxiosError } from 'axios';
import router from '@/router';
import api from '@/axios.ts';

const registerDto = ref<RegisterDto>({
  email: '',
  password: '',
  passwordConfirm: '',
  username: '',
  firstName: '',
  lastName: '',
  dateOfBirth: '',
});
const loading = ref<boolean>(false);
const error = ref<string | null>(null);
async function onSubmit() {
  try {
    loading.value = true;
    const response = await api.post<RegisterDto>('account/register', registerDto.value);
    router.push('/login');
  } catch (err) {
    console.log(err);
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
    }
  } finally {
    loading.value = false;
  }
}
</script>

<template>
  <div>
    <form class="form" @submit.prevent="onSubmit">
      <label for="email">Email address:</label>
      <input type="email" id="email" name="email" required v-model="registerDto.email" />
      <label for="password">Password:</label>
      <input
        type="password"
        id="password"
        name="password"
        required
        v-model="registerDto.password"
      />
      <label for="passwordConfirm">Confirm password:</label>
      <input
        type="password"
        id="passwordConfirm"
        name="passwordConfirm"
        required
        v-model="registerDto.passwordConfirm"
      />
      <label for="username">Username:</label>
      <input type="text" id="username" name="username" required v-model="registerDto.username" />
      <label for="firstName">First name:</label>
      <input type="text" id="firstName" name="firstName" required v-model="registerDto.firstName" />
      <label for="lastName">Last name:</label>
      <input type="text" id="lastName" name="lastName" v-model="registerDto.lastName" />
      <label for="dateOfBirth">Date of Birth:</label>
      <input
        type="date"
        id="dateOfBirth"
        name="dateOfBirth"
        required
        v-model="registerDto.dateOfBirth"
      />
      <button type="submit" :disabled="loading">Submit</button>
    </form>
    <div v-if="error">{{ error }}</div>
  </div>
</template>

<style scoped></style>
