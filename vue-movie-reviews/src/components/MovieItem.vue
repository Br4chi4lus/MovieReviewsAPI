<script setup lang="ts">
const props = defineProps<{
  title: string;
  director: string;
  dateOfPremiere: string;
}>();
</script>

<template>
  <div>
    <h2>
      {{ props.title }}
    </h2>
    <div>
      <h3>Director: {{ props.director }}</h3>
      <h3>Premiere: {{ props.dateOfPremiere }}</h3>
    </div>
  </div>
</template>

<style scoped></style>
