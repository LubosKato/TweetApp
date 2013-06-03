using System.ComponentModel.DataAnnotations;

namespace TweetApp.Models
{
    public class InputModel
    {
        [Required(ErrorMessage = "Missing input")]
        public string InputValues { get; set; }
    }
}