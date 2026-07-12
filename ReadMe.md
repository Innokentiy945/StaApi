# Exercise Generation & Validation Engine

A deterministic exercise generation and validation engine built around **DataJson as the single source of truth**.

The architecture eliminates runtime database dependencies for generated exercises by embedding all required validation data directly into the generated exercise payload.

---

# Features

- ✅ DataJson as the single source of truth
- ✅ Deterministic exercise generation
- ✅ Stateless grammar engine
- ✅ Pure validation layer
- ✅ No runtime database lookups
- ✅ Dictionary-driven subject resolution
- ✅ Slot-based pattern system
- ✅ Case-insensitive JSON deserialization
- ✅ Frontend/backend responsibility separation

---

# Architecture

## Single Source of Truth

Each generated exercise is completely self-contained inside `DataJson`.

Example:

```json
{
  "sentence": "_____ istovarim",
  "missingSlot": "VERB",
  "task": "fill_in_blank",
  "answer": "radim",
  "xpReward": 10,
  "hint": "...",
  "explanation": "..."
}
```

No additional runtime lookup is required.

---

# Generated Exercise Contract

```csharp
public class GeneratedExerciseDto
{
    public Guid ExerciseInstanceId { get; init; }
    public ulong SubtopicId { get; init; }
    public uint XpReward { get; init; }
    public string DataJson { get; init; } = string.Empty;
}
```

---

# Generation Pipeline

```text
Pattern
    ↓
Resolve SUBJECT
    ↓
Resolve VERB
    ↓
Apply Grammar Rule
    ↓
Build Sentence
    ↓
Serialize DataJson
    ↓
Return DTO
```

The pipeline is deterministic and produces a fully self-contained exercise.

---

# Validation Flow

## Submit Request

```csharp
public class SubmitAnswerRequestDto
{
    public Guid ExerciseInstanceId { get; init; }
    public ulong SubtopicId { get; init; }
    public string DataJson { get; init; } = string.Empty;
    public string Answer { get; init; } = string.Empty;
    public uint TimeSpentSeconds { get; init; }
}
```

## Submit Response

```csharp
public class SubmitAnswerResponseDto
{
    public bool IsCorrect { get; set; }
    public decimal ScorePercent { get; set; }
    public uint XpEarned { get; set; }
    public string? Explanation { get; set; }
}
```

## Runtime Flow

```text
SubmitAnswerRequestDto
        ↓
Deserialize(DataJson)
(case-insensitive)
        ↓
Validator.Validate(data, userAnswer)
        ↓
Compare expected answer
        ↓
Return validation result
```

---

# JSON Deserialization

The project uses **System.Text.Json**.

By default, deserialization is case-sensitive.

To ensure reliable mapping, deserialization is performed with:

```csharp
var data = JsonSerializer.Deserialize<ExerciseDataDto>(
    requestDto.DataJson,
    new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    });
```

Without this option, properties such as `answer` may fail to map to `Answer`, resulting in empty values and false validation failures.

---

# Validator

The validator intentionally has a single responsibility.

## Responsibilities

- Extract the expected answer from `DataJson`
- Compare it with the user input
- Return the validation result

## Out of Scope

- Grammar rules
- Verb conjugation
- Dictionary lookup
- Database access
- Business logic

Example implementation:

```csharp
public bool Validate(ExerciseDataDto data, string userAnswer)
{
    return string.Equals(
        data.Answer?.Trim(),
        userAnswer?.Trim(),
        StringComparison.OrdinalIgnoreCase
    );
}
```

---

# Grammar Engine

The grammar engine is fully isolated from validation.

Properties:

- Stateless
- Deterministic
- No database dependency
- No JSON dependency
- Pure rule transformations

---

# Subject Model

The subject resolution system is:

- Dictionary-driven
- Free of hardcoded pronouns

---

# Pattern System

The pattern engine is based on:

- Slot-based templates
- Deterministic generation
- POS mapping

---

# Frontend Recommendation

The frontend should:

- Parse `DataJson`
- Render the exercise
- Submit the user's answer separately
- Avoid duplicating backend validation logic

---

# Design Principles

- **DataJson** is the only runtime source of truth.
- Validation is deterministic.
- The validator performs only answer comparison.
- Grammar generation is isolated.
- Generated exercises do not require database lookups.
- Components have clearly separated responsibilities.

---

# License

This project is licensed under the **MIT License**.

See the `LICENSE` file for details.

---

# Disclaimer

This project is provided **AS IS**, without warranty of any kind, express or implied, including but not limited to warranties of merchantability, fitness for a particular purpose, and noninfringement.

The software may contain bugs, limitations, or incomplete functionality.

The authors and copyright holders shall not be liable for any claim, damages, or other liability arising from the use of this software.

By using this project, you acknowledge that it is provided solely under the terms of the MIT License and that you assume all risks associated with its use.