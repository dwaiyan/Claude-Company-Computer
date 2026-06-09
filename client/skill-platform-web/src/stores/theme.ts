import { defineStore } from 'pinia';
import { ref } from 'vue';

export type Theme = 'light' | 'dark';

export const useThemeStore = defineStore('theme', () => {
  const theme = ref<Theme>(getInitialTheme());
  const systemDark = ref(window.matchMedia('(prefers-color-scheme: dark)').matches);

  function getInitialTheme(): Theme {
    const saved = localStorage.getItem('theme-preference');
    if (saved === 'dark' || saved === 'light') return saved;
    return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
  }

  function applyTheme(t: Theme) {
    theme.value = t;
    document.documentElement.setAttribute('data-theme', t);
  }

  function toggle() {
    applyTheme(theme.value === 'dark' ? 'light' : 'dark');
    localStorage.setItem('theme-preference', theme.value);
  }

  function setTheme(t: Theme) {
    applyTheme(t);
    localStorage.setItem('theme-preference', t);
  }

  const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
  mediaQuery.addEventListener('change', (e) => {
    systemDark.value = e.matches;
    if (!localStorage.getItem('theme-preference')) {
      applyTheme(e.matches ? 'dark' : 'light');
    }
  });

  return { theme, systemDark, toggle, setTheme };
});
