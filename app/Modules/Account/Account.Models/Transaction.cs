namespace Account.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    public class Transaction
    {
        [Required]
        [JsonPropertyName("account_id")]
        public Guid AccountId { get; set; }

        [Required]
        public int? Amount { get; set; }
    }
}
