<script setup lang="ts">
const props = defineProps<{
    todo: string
}>();
</script>

<template>
  <li>
    <span>{{ todo }}</span>
    <button>X</button>
  </li>
</template>
