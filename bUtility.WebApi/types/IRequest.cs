namespace bUtility
{
    public interface IRequest : IUserIDProvider
    {
        RequestHeader Header { get; set; }
        object Data { get; }
        string UserID { get; }
    }
}