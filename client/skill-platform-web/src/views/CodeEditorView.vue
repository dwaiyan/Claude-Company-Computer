<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { codeRunnerApi } from '@/api/coderunner';
import type { LanguageInfo } from '@/api/coderunner';

const code = ref(`using System;\n\nConsole.WriteLine("Hello .NET!");\nConsole.WriteLine($"Sum: {Add(3, 5)}");\n\nstatic int Add(int a, int b) => a + b;`);
const output = ref('');
const errors = ref('');
const running = ref(false);
const execTime = ref(0);
const languages = ref<LanguageInfo[]>([]);

onMounted(async () => {
  languages.value = await codeRunnerApi.getLanguages();
});

async function runCode() {
  running.value = true;
  output.value = '';
  errors.value = '';
  try {
    const result = await codeRunnerApi.run({ code: code.value });
    output.value = result.output;
    errors.value = result.errors;
    execTime.value = result.executionTimeMs;
  } catch (e: any) {
    errors.value = e.message || '执行失败';
  } finally {
    running.value = false;
  }
}
</script>

<template>
  <div class="page-container">
    <header class="page-header">
      <h1>C# 在线代码运行</h1>
      <p>编写 .NET 6 代码，在线编译运行</p>
    </header>

    <div class="editor-section">
      <textarea v-model="code" class="code-editor" spellcheck="false" rows="14"></textarea>
      <button @click="runCode" :disabled="running" class="btn-run">
        {{ running ? '运行中...' : '▶ 运行' }}
      </button>
    </div>

    <div v-if="output || errors" class="output-section">
      <div v-if="errors" class="errors"><pre>{{ errors }}</pre></div>
      <div v-if="output" class="output">
        <div class="output-header">输出 <span class="time">{{ execTime }}ms</span></div>
        <pre>{{ output }}</pre>
      </div>
    </div>
  </div>
</template>

<style scoped>
.page-container { max-width: 960px; margin: 0 auto; padding: 32px 24px; }
.page-header h1 { font-size: 24px; color: #1a73e8; }
.page-header p { color: #888; margin-top: 4px; }
.editor-section { margin: 20px 0; }
.code-editor { width: 100%; padding: 16px; font-family: 'Consolas', 'Courier New', monospace; font-size: 14px; border: 1px solid #d9d9d9; border-radius: 8px; background: #1e1e2e; color: #cdd6f4; resize: vertical; }
.btn-run { margin-top: 12px; padding: 10px 32px; background: #1a73e8; color: #fff; border: none; border-radius: 8px; font-size: 15px; cursor: pointer; }
.btn-run:disabled { background: #93b8f0; cursor: not-allowed; }
.output-section { margin-top: 20px; }
.errors pre { background: #fce8e6; color: #d93025; padding: 14px; border-radius: 8px; font-size: 13px; white-space: pre-wrap; }
.output { background: #fff; border-radius: 8px; overflow: hidden; box-shadow: 0 1px 6px rgba(0,0,0,0.06); }
.output-header { padding: 10px 16px; background: #f0f5ff; font-size: 13px; font-weight: 500; color: #1a73e8; }
.time { float: right; color: #888; font-weight: 400; }
.output pre { padding: 14px 16px; font-family: 'Consolas', monospace; font-size: 14px; white-space: pre-wrap; margin: 0; }
</style>
