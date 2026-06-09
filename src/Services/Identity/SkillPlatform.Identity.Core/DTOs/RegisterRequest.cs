using System.ComponentModel.DataAnnotations;

namespace SkillPlatform.Identity.Core.DTOs;

public class RegisterRequest
{
    [Required(ErrorMessage = "用户名不能为空")]
    [StringLength(50, MinimumLength = 2)]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "邮箱不能为空")]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "密码不能为空")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "密码至少6位")]
    public string Password { get; set; } = string.Empty;

    public string? Department { get; set; }
    public string? Title { get; set; }
}
