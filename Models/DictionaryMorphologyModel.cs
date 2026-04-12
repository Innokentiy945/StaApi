using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace StaApi.Models;

[Table("DictionaryMorphologyTable")]
[Index(nameof(Lemma))]
public class DictionaryMorphologyModel
{
    [Key]
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("word")]
    public string? Wordform { get; set; }

    [JsonPropertyName("lemma")]
    public string? Lemma { get; set; }

    [JsonPropertyName("tag")]
    public string? Msd { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("pos")]
    public string? Upos { get; set; }

    [JsonPropertyName("features_type")]
    public string? Features_Type { get; set; }

    [JsonPropertyName("features_degree")]
    public string? Features_Degree { get; set; }

    [JsonPropertyName("features_gender")]
    public string? Features_Gender { get; set; }

    [JsonPropertyName("features_number")]
    public string? Features_Number { get; set; }

    [JsonPropertyName("features_case")]
    public string? Features_Case { get; set; }

    [JsonPropertyName("features_definiteness")]
    public string? Features_Definiteness { get; set; }

    [JsonPropertyName("morph_case")]
    public string? Morph_Case { get; set; }

    [JsonPropertyName("morph_definite")]
    public string? Morph_Definite { get; set; }

    [JsonPropertyName("morph_degree")]
    public string? Morph_Degree { get; set; }

    [JsonPropertyName("morph_gender")]
    public string? Morph_Gender { get; set; }

    [JsonPropertyName("morph_number")]
    public string? Morph_Number { get; set; }

    [JsonPropertyName("frequency")]
    public double? Frequency { get; set; }

    [JsonPropertyName("per_million")]
    public double? PerMillion { get; set; }
    
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
}