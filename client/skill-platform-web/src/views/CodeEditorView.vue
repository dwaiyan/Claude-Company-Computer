<script setup lang="ts">
import { ref, onMounted } from 'vue'; import { codeRunnerApi } from '@/api/coderunner';
import type { LanguageInfo } from '@/api/coderunner'; import ThemeToggle from '@/components/ThemeToggle.vue';

const code = ref(`using System;\n\nConsole.WriteLine("// Laboratory Precision //");\nConsole.WriteLine($"Execution: {DateTime.Now:HH:mm:ss}");`);
const output = ref(''); const errors = ref(''); const running = ref(false);
const execTime = ref(0); const languages = ref<LanguageInfo[]>([]);

onMounted(async () => { languages.value = await codeRunnerApi.getLanguages(); });

async function runCode() {
  running.value = true; output.value = ''; errors.value = '';
  try { const r = await codeRunnerApi.run({ code: code.value }); output.value = r.output; errors.value = r.errors; execTime.value = r.executionTimeMs; }
  catch (e: any) { errors.value = e.message || '执行失败'; } finally { running.value = false; }
}
</script>

<template>
  <div class="dashboard">
    <header class="app-header"><div class="header-left"><h1>代码运行</h1><nav class="nav-links"><router-link to="/">←</router-link></nav></div><div class="header-right"><ThemeToggle /></div></header>
    <div class="page-container">
      <div class="page-header"><h1>C# 在线运行</h1><p>编写 .NET 6 代码，在线编译执行</p></div>

      <div style="margin:20px 0;">
        <textarea v-model="code" style="width:100%;padding:20px;font-family:var(--font-display);font-size:13px;line-height:1.7;background:var(--bg-elevated);color:var(--text-primary);border:1px solid var(--border-default);border-radius:var(--radius-lg);resize:vertical;" rows="12" spellcheck="false"></textarea>
        <button @click="runCode" :disabled="running" class="btn-primary" style="margin-top:12px;width:auto;">
          {{ running ? '▹ 编译中...' : '▸ 运行' }}
        </button>
      </div>

      <div v-if="errors" style="background:rgba(220,38,38,0.08);border:1px solid rgba(220,38,38,0.2);border-radius:var(--radius-md);padding:16px;font-family:var(--font-display);font-size:12px;color:var(--color-danger);white-space:pre-wrap;margin-bottom:16px;">{{ errors }}</div>
      <div v-if="output" class="card" style="padding:0;overflow:hidden;">
        <div style="padding:10px 16px;background:var(--bg-muted);font-family:var(--font-display);font-size:11px;letter-spacing:0.04em;color:var(--text-secondary);">OUTPUT <span style="float:right;color:var(--text-muted);">{{ execTime }}ms</span></div>
        <pre style="padding:16px;font-family:var(--font-display);font-size:13px;line-height:1.6;white-space:pre-wrap;margin:0;">{{ output }}</pre>
      </div>
    </div>
  </div>
</template>
