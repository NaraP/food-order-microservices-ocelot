namespace Notification.API.Entities;

public class NotificationLog
{
    public int      Id        { get; set; }
    public string   EventType { get; set; } = string.Empty;
    public string   Email     { get; set; } = string.Empty;
    public string   Subject   { get; set; } = string.Empty;
    public string   Body      { get; set; } = string.Empty;
    public bool     Sent      { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid?    RelatedId { get; set; }
}
