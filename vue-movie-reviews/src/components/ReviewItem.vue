<script setup lang="ts">
defineProps<{
  author: string;
  content: string;
  rating: number;
}>();
</script>

<template>
  <div>
    <h3>{{ author + ' ' + rating }}</h3>
    <p>{{ content }}</p>
  </div>
</template>

<style scoped></style>
