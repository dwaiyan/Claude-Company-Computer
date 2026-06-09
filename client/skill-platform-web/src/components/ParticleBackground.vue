<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue';
import { useTheme } from '@/composables/useTheme';

const canvas = ref<HTMLCanvasElement | null>(null);
const { isDark } = useTheme();
let animId = 0;
let mouseX = 0, mouseY = 0;

interface Particle { x: number; y: number; vx: number; vy: number; r: number; alpha: number; }

onMounted(() => {
  const c = canvas.value!;
  const ctx = c.getContext('2d')!;
  const resize = () => { c.width = c.parentElement!.clientWidth; c.height = c.parentElement!.clientHeight; };
  resize(); window.addEventListener('resize', resize);

  const particles: Particle[] = Array.from({ length: 60 }, () => ({
    x: Math.random() * c.width,
    y: Math.random() * c.height,
    vx: (Math.random() - 0.5) * 0.4,
    vy: (Math.random() - 0.5) * 0.4,
    r: Math.random() * 2 + 0.5,
    alpha: Math.random() * 0.5 + 0.1,
  }));

  window.addEventListener('mousemove', (e) => {
    const rect = c.getBoundingClientRect();
    mouseX = e.clientX - rect.left; mouseY = e.clientY - rect.top;
  });

  function draw() {
    ctx.clearRect(0, 0, c.width, c.height);
    if (isDark()) { animId = requestAnimationFrame(draw); return; }

    for (const p of particles) {
      p.x += p.vx; p.y += p.vy;
      if (p.x < 0 || p.x > c.width) p.vx *= -1;
      if (p.y < 0 || p.y > c.height) p.vy *= -1;

      const dx = mouseX - p.x, dy = mouseY - p.y;
      const dist = Math.sqrt(dx * dx + dy * dy);
      if (dist < 150) { p.vx += dx * 0.0001; p.vy += dy * 0.0001; }

      ctx.beginPath();
      ctx.arc(p.x, p.y, p.r, 0, Math.PI * 2);
      ctx.fillStyle = `rgba(26, 86, 219, ${p.alpha})`;
      ctx.fill();
    }
    animId = requestAnimationFrame(draw);
  }
  draw();
});

onUnmounted(() => { cancelAnimationFrame(animId); });
</script>

<template>
  <canvas ref="canvas" style="position:absolute;inset:0;z-index:0;opacity:var(--particle-opacity);pointer-events:none;"></canvas>
</template>
