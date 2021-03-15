namespace Account.Models
{
    using System.ComponentModel.DataAnnotations;

    public class AccountBalance
    {
        [Required]
        public int Balance { get; set; }
    }
}
