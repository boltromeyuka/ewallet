using System.ComponentModel.DataAnnotations;

namespace EWallet.data
{
    public class Named
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
