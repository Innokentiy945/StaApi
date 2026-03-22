using System.Text.Json;
using System.Text.Json.Serialization;

namespace StaApi.Models
{
    
    
    public class DictionaryMorphologyModel
    {
        public Guid Id { get; set; }                
        public string Wordform { get; set; }        
        public string Lemma { get; set; }           
        public string Msd { get; set; }             
        public string Type { get; set; }            
        public string Upos { get; set; }            
        public Features Features { get; set; }      
        public Morph Morph { get; set; }            
        public double Frequency { get; set; }       
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
}