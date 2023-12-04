<script setup lang="ts">
const props = defineProps(["todo"]);
</script>

<template>
  <li>
    <span>{{ todo }}</span>
    <button>X</button>
  </li>
</template>
