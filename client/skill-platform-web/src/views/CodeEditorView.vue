<script setup lang="ts">
import { ref, onMounted } from 'vue'; import { codeRunnerApi } from '@/api/coderunner';
import type { LanguageInfo } from '@/api/coderunner'; import ThemeToggle from '@/components/ThemeToggle.vue';

const code = ref('using System;\n\nConsole.WriteLine("Hello, SkillPlatform!");\nConsole.WriteLine($"执行时间: {DateTime.Now:HH:mm:ss}");');
const result = ref(''); const errors = ref('');
const running = ref(false); const languages = ref<LanguageInfo[]>([]);

onMounted(async () => { try { languages.value = await codeRunnerApi.getLanguages(); } catch { /* */ } });

async function run() {
  running.value = true; result.value = ''; errors.value = '';
  try { const r = await codeRunnerApi.run({ code: code.value }); result.value = r.output; errors.value = r.errors; }
  catch (e: any) { errors.value = e.message || '执行错误'; }
  finally { running.value = false; }
}
</script>

<template>
  <div class="dashboard">
    <header class="app-header"><div class="header-left"><h1>C# 在线运行</h1><nav class="nav-links"><router-link to="/">← 返回</router-link></nav></div><div class="header-right"><ThemeToggle /></div></header>
    <div class="page-container" style="max-width:900px;">
      <div style="margin-bottom:24px;">
        <textarea v-model="code" spellcheck="false" style="width:100%;min-height:260px;padding:24px;background:var(--bg-muted);border:1px solid var(--border-subtle);border-radius:var(--radius-md);font-family:var(--font-mono);font-size:14px;font-weight:300;color:var(--text-primary);resize:vertical;line-height:1.7;tab-size:2;"></textarea>
      </div>
      <button @click="run" :disabled="running" class="btn-primary" style="width:auto;padding:10px 40px;margin-bottom:28px;">
        {{ running ? '执行中...' : '▶ 运行' }}
      </button>
      <div v-if="result" style="padding:24px;background:var(--bg-muted);border:1px solid var(--border-subtle);border-radius:var(--radius-md);font-family:var(--font-mono);font-size:13px;font-weight:300;white-space:pre-wrap;line-height:1.7;color:var(--text-primary);">{{ result }}</div>
      <div v-if="errors" style="padding:24px;background:rgba(194,65,12,0.04);border-left:2px solid var(--color-danger);border-radius:0 var(--radius-sm) var(--radius-sm) 0;font-family:var(--font-mono);font-size:13px;font-weight:300;white-space:pre-wrap;line-height:1.7;color:var(--color-danger);margin-top:12px;">{{ errors }}</div>
    </div>
  </div>
</template>
