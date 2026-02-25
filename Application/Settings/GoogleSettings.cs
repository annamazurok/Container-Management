using System.ComponentModel.DataAnnotations;

namespace Application.Settings;

public class GoogleSettings
{
    public const string SectionName = "Google";

    [Required(ErrorMessage = "Google ClientId is required")]
    public string ClientId { get; set; } = string.Empty;
}