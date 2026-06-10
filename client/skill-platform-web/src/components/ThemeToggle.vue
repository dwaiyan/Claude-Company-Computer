<script setup lang="ts">
import { useTheme } from '@/composables/useTheme';

const { theme, toggle } = useTheme();
</script>

<template>
  <button
    class="zen-toggle"
    :title="theme === 'dark' ? '墨 → 紙' : '紙 → 墨'"
    @click="toggle"
    :aria-label="theme === 'dark' ? 'Switch to light mode' : 'Switch to dark mode'"
  >
    <span class="zen-toggle-icon">{{ theme === 'dark' ? '墨' : '紙' }}</span>
  </button>
</template>

<style scoped>
.zen-toggle {
  background: none;
  border: none;
  cursor: pointer;
  padding: 4px;
  border-radius: var(--radius-sm);
  transition: background var(--transition-fast);
  font-family: var(--font-display);
}
.zen-toggle:hover { background: var(--bg-muted); }

.zen-toggle-icon {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 32px;
  height: 32px;
  font-size: 15px;
  color: var(--text-secondary);
  transition: color var(--transition-base);
  line-height: 1;
}

.zen-toggle:hover .zen-toggle-icon {
  color: var(--color-accent);
}
</style>
