<script setup lang="ts">
import router from '@/router';
import { isAuthenticated, userIsModOrAdmin } from '@/components/auth/auth.ts';

function onClick() {
  localStorage.removeItem('token');
  isAuthenticated.value = false;
  userIsModOrAdmin.value = false;
  router.push('/login');
}
</script>

<template>
  <button @click="onClick">Logout</button>
</template>

<style scoped>
button {
  border: none;
  outline: none;
  background: none;
  cursor: pointer;
  color: hsla(160, 100%, 37%, 1);
}
</style>
