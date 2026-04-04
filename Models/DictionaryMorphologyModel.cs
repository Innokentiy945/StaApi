using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace StaApi.Models;

[Table("DictionaryMorphologyTable")]
public class DictionaryMorphologyModel
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("word")]
    public string Wordform { get; set; }

    [JsonPropertyName("lemma")]
    public string Lemma { get; set; }

    [JsonPropertyName("tag")]
    public string Msd { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("pos")]
    public string Upos { get; set; }

    [JsonPropertyName("features")]
    public Features Features { get; set; }

    [JsonPropertyName("morph")]
    public Morph Morph { get; set; }

    [JsonPropertyName("frequency")]
    public double Frequency { get; set; }

    [JsonPropertyName("per_million")]
    public double PerMillion { get; set; }
}

public class Features
{
    public string Type { get; set; }
    public string Degree { get; set; }
    public string Gender { get; set; }
    public string Number { get; set; }
    public string Case { get; set; }
    public string Definiteness { get; set; }
}

public class Morph
{
    public string Case { get; set; }
    public string Definite { get; set; }
    public string Degree { get; set; }
    public string Gender { get; set; }
    public string Number { get; set; }
}
