<script setup lang="ts">
import { useTheme } from '@/composables/useTheme';

const { theme, toggle } = useTheme();
</script>

<template>
  <button
    class="theme-toggle"
    :title="theme === 'dark' ? '切换浅色模式' : '切换深色模式'"
    @click="toggle"
    :aria-label="theme === 'dark' ? 'Switch to light mode' : 'Switch to dark mode'"
  >
    <span class="toggle-track">
      <span class="toggle-icon">{{ theme === 'dark' ? '◉' : '○' }}</span>
    </span>
  </button>
</template>

<style scoped>
.theme-toggle {
  background: none;
  border: none;
  cursor: pointer;
  padding: 4px;
  border-radius: var(--radius-md);
  transition: background var(--transition-fast);
}
.theme-toggle:hover { background: var(--bg-muted); }

.toggle-track {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 32px;
  height: 32px;
  border-radius: 50%;
  border: 1px solid var(--border-default);
  font-size: 14px;
  transition: all var(--transition-base);
  color: var(--text-secondary);
}

.toggle-icon {
  transition: transform var(--transition-base);
  line-height: 1;
}

.theme-toggle:hover .toggle-track {
  border-color: var(--color-primary);
  color: var(--color-primary);
  box-shadow: 0 0 8px var(--color-primary-glow);
}
</style>
