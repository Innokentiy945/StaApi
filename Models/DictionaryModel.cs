using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaApi.Models;

[Table("DictionaryTable")]
public class DictionaryModel
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [Column("word")]
    public string Word { get; set; }

    [Required]
    [Column("definition")]
    public string Definition { get; set; }

    [Column("part_of_speech")]
    public string Pos { get; set; }
}