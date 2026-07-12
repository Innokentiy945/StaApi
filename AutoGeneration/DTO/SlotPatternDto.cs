namespace StaApi.AutoGeneration.DTO;

public class SlotPatternDto
{
    public string Code { get; set; } = default!;

    public string[] Slots { get; set; } = [];

    public Dictionary<string, SlotRuleDto> Rules { get; set; } = new();

    public string Template { get; set; } = default!;
}