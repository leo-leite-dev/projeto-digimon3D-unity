public class DigimonSkillTriggerFormatter
{
    public string Format(string trigger)
    {
        if (string.IsNullOrWhiteSpace(trigger))
            return null;

        return trigger.Replace(" ", string.Empty);
    }
}
