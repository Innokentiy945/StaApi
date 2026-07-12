using System.Runtime.Serialization;

namespace StaApi.AutoGeneration.Models;

public enum UserLevel
{
    A1,
    A2,
    B1,
    B2,
    C1,
    C2
}

public enum AuthProvider
{
    Local,
    Google,
    Github,
    Apple
}

public enum TopicType
{
    [EnumMember(Value = "grammar")]
    Grammar,

    [EnumMember(Value = "vocabulary")]
    Vocabulary,

    [EnumMember(Value = "speaking")]
    Speaking,

    [EnumMember(Value = "listening")]
    Listening,

    [EnumMember(Value = "mixed")]
    Mixed
}

public enum DifficultyLevel
{
    Easy,
    Medium,
    Hard
}

public enum ExerciseSourceType
{
    Manual,
    Generated
}


public enum AnswerType
{
    Primary = 0,
    Alternative = 1,
    Distractor = 2
}