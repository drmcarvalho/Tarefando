namespace Tarefando.Api.Database.Enums
{
    public enum ETaskType
    {
        Urgent = 0, // High priority for ordering
        Normal = 1,
        TeamAlignment = 2,        
        Training = 3,
        Administrative = 4
    }

    public static class ETaskTypeExtensions
    {
        public static string ToFriendlyString(this ETaskType taskType) => taskType switch
        {
            ETaskType.Urgent => "urgent",
            ETaskType.Normal => "normal",
            ETaskType.TeamAlignment => "teamAlignment",
            ETaskType.Training => "training",
            ETaskType.Administrative => "administrative",
            _ => "Unknown"
        };
    }
}
