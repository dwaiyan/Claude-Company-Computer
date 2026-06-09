using System.ComponentModel.DataAnnotations;

namespace SkillPlatform.Identity.Core.DTOs;

public class LoginRequest
{
    [Required(ErrorMessage = "邮箱不能为空")]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "密码不能为空")]
    public string Password { get; set; } = string.Empty;
}
