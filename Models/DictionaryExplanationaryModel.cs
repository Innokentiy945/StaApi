using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace StaApi.Models;

[Table("DictionaryTable")]
public class DictionaryExplanationaryModel
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [Column("word")]
    [JsonProperty("word_lat")]
    public string Word { get; set; }

    [Column("definition")]
    [JsonProperty("definition")]
    public string Definition { get; set; }

    [Column("part_of_speech")]
    [JsonProperty("pos")]
    public string Pos { get; set; }
}