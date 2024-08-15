using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public sealed class ExcelFile
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public byte[] Data { get; set; } = null!;

        public ExcelFile(Guid id, string name, byte[] data)
        {
            Id = id;
            Name = name;
            Data = data;
        }
    }
}
