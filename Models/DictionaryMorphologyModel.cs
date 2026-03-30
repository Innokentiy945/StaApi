using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace StaApi.Models;

[Table("DictionaryMorphologyTable")]
public class DictionaryMorphologyModel
{
    [Column("id")]
    public Guid Id { get; set; }

    [Column("wordform")]
    public string Wordform { get; set; }

    [Column("lemma")]
    public string Lemma { get; set; }

    [Column("msd")]
    public string Msd { get; set; }

    [Column("type")]
    public string Type { get; set; }

    [Column("upos")]
    public string Upos { get; set; }

    [Column("features")]
    public string Features { get; set; }

    [Column("morph")]
    public string Morph { get; set; }

    [Column("frequency")]
    public double Frequency { get; set; }

    [Column("per_million")]
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
