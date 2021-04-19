using System;
using System.ComponentModel.DataAnnotations;

public class ApplyModel
{
    [Required]
    [StringLength(16, ErrorMessage = "Full Name too long (128 character limit).")]
    public string FullName { get; set; }

    [Required]
    public RequestType Request { get; set; }

    [Range(1, 100000, ErrorMessage = "Donation value is invalid (1-100000).")]
    public int Donation { get; set; }
}

public enum RequestType
{
    Hug,
    Kiss
}