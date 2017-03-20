namespace bUtility
{
    public interface IResponse: IExceptionContainer
    {
        object Data { get; set; }
    }
}
