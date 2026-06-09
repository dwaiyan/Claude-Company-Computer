import { createRouter, createWebHistory } from 'vue-router';
import { useAuthStore } from '@/stores/auth';

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      name: 'dashboard',
      component: () => import('@/views/DashboardView.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('@/views/LoginView.vue'),
      meta: { guest: true },
    },
    {
      path: '/register',
      name: 'register',
      component: () => import('@/views/RegisterView.vue'),
      meta: { guest: true },
    },
    {
      path: '/tech-tree',
      name: 'tech-tree',
      component: () => import('@/views/TechTreeView.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/tech-tree/:nodeId',
      name: 'node-detail',
      component: () => import('@/views/NodeDetailView.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/learning',
      name: 'learning',
      component: () => import('@/views/LearningView.vue'),
      meta: { requiresAuth: true },
    },
  ],
});

router.beforeEach((to, _from, next) => {
  const auth = useAuthStore();
  if (to.meta.requiresAuth && !auth.isLoggedIn) {
    next('/login');
  } else if (to.meta.guest && auth.isLoggedIn) {
    next('/');
  } else {
    next();
  }
});

export default router;
