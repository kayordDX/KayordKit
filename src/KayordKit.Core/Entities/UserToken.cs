namespace KayordKit.Core.Entities;
public class UserToken
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime IssuedDate { get; set; }
    public DateTime ExpireDate { get; set; }
}
