using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaApi.Models;

[Table("DictionaryTable")]
public class DictionaryModel
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Word { get; set; }

    [Required]
    public string Definition { get; set; }

    public string Pos { get; set; }
}