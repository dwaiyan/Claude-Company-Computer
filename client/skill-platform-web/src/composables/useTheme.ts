import { useThemeStore } from '@/stores/theme';

export function useTheme() {
  const store = useThemeStore();
  return {
    theme: store.theme,
    isDark: () => store.theme === 'dark',
    toggle: () => store.toggle(),
    setTheme: (t: 'light' | 'dark') => store.setTheme(t),
  };
}
