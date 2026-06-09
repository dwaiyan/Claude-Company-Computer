<script setup lang="ts">
import { onMounted } from 'vue';
import { useThemeStore } from '@/stores/theme';

const theme = useThemeStore();

onMounted(() => {
  document.documentElement.setAttribute('data-theme', theme.theme);
});
</script>

<template>
  <router-view />
</template>
