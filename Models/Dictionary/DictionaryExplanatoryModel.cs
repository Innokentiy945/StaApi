using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace StaApi.Models.Dictionary;

[Table("MFSW")]
public class DictionaryExplanatoryModel
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
    
    [Column("translation_ru")]
    [JsonProperty("translation_ru")]
    public string TranslationRu { get; set; }
    
    [Column("translation_en")]
    [JsonProperty("translation_en")]
    public string TranslationEn { get; set; }
}