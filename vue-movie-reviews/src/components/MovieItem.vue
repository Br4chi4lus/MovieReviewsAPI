<script setup lang="ts">
defineProps<{
  title: string;
  director: string;
  dateOfPremiere: string;
}>();
</script>

<template>
  <div>
    <h2>
      {{ title }}
    </h2>
    <div>
      <h3>Director: {{ director }}</h3>
      <h3>Premiere: {{ dateOfPremiere }}</h3>
    </div>
  </div>
</template>

<style scoped></style>
