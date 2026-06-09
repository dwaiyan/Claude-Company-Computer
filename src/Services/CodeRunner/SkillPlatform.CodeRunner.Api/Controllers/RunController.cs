using System.Diagnostics;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace SkillPlatform.CodeRunner.Api.Controllers;

[ApiController]
[Route("api/code")]
public class RunController : ControllerBase
{
    private static readonly string WorkDir = Path.Combine(Path.GetTempPath(), "skill-code-runner");

    [HttpPost("run")]
    public async Task<IActionResult> Run([FromBody] CodeRunRequest request)
    {
        var runId = Guid.NewGuid().ToString("N")[..8];
        var dir = Path.Combine(WorkDir, runId);

        try
        {
            Directory.CreateDirectory(dir);

            // Create project
            var projXml = @"<Project Sdk=""Microsoft.NET.Sdk""><PropertyGroup><OutputType>Exe</OutputType><TargetFramework>net6.0</TargetFramework><ImplicitUsings>enable</ImplicitUsings></PropertyGroup></Project>";
            await System.IO.File.WriteAllTextAsync(Path.Combine(dir, "Runner.csproj"), projXml);
            await System.IO.File.WriteAllTextAsync(Path.Combine(dir, "Program.cs"), request.Code);

            // Compile
            var compileResult = await RunProcess("dotnet", $"build -nologo -v q \"{Path.Combine(dir, "Runner.csproj")}\"", 30000);
            if (compileResult.ExitCode != 0)
            {
                return Ok(new CodeRunResult
                {
                    Success = false,
                    Output = compileResult.Output,
                    Errors = compileResult.Errors,
                    CompilationSucceeded = false
                });
            }

            // Run
            var runResult = await RunProcess("dotnet", $"run --no-build --project \"{Path.Combine(dir, "Runner.csproj")}\"", request.TimeoutMs > 0 ? request.TimeoutMs : 5000);

            return Ok(new CodeRunResult
            {
                Success = runResult.ExitCode == 0,
                Output = runResult.Output,
                Errors = runResult.Errors,
                CompilationSucceeded = true,
                ExecutionTimeMs = runResult.ElapsedMs
            });
        }
        finally
        {
            try { Directory.Delete(dir, true); } catch { /* cleanup fail ok */ }
        }
    }

    [HttpGet("languages")]
    public IActionResult GetLanguages()
    {
        return Ok(new[]
        {
            new { id = "csharp", name = "C# (.NET 6)", template = "using System;\n\nConsole.WriteLine(\"Hello World!\");" }
        });
    }

    private static async Task<ProcessResult> RunProcess(string cmd, string args, int timeoutMs)
    {
        var sw = Stopwatch.StartNew();
        var psi = new ProcessStartInfo(cmd, args)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var proc = Process.Start(psi)!;
        var output = new List<string>();
        var errors = new List<string>();

        proc.OutputDataReceived += (_, e) => { if (e.Data != null) output.Add(e.Data); };
        proc.ErrorDataReceived += (_, e) => { if (e.Data != null) errors.Add(e.Data); };
        proc.BeginOutputReadLine();
        proc.BeginErrorReadLine();

        if (!proc.WaitForExit(timeoutMs))
        {
            proc.Kill(true);
            errors.Add("执行超时");
        }

        sw.Stop();
        return new ProcessResult
        {
            ExitCode = proc.HasExited ? proc.ExitCode : -1,
            Output = string.Join("\n", output),
            Errors = string.Join("\n", errors),
            ElapsedMs = (int)sw.ElapsedMilliseconds
        };
    }
}

public class CodeRunRequest
{
    [JsonPropertyName("code")] public string Code { get; set; } = string.Empty;
    [JsonPropertyName("timeoutMs")] public int TimeoutMs { get; set; } = 5000;
}

public class CodeRunResult
{
    [JsonPropertyName("success")] public bool Success { get; set; }
    [JsonPropertyName("output")] public string Output { get; set; } = string.Empty;
    [JsonPropertyName("errors")] public string Errors { get; set; } = string.Empty;
    [JsonPropertyName("compilationSucceeded")] public bool CompilationSucceeded { get; set; }
    [JsonPropertyName("executionTimeMs")] public int ExecutionTimeMs { get; set; }
}

public class ProcessResult
{
    public int ExitCode { get; set; }
    public string Output { get; set; } = string.Empty;
    public string Errors { get; set; } = string.Empty;
    public int ElapsedMs { get; set; }
}
