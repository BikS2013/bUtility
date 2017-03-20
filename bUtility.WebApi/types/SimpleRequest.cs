namespace bUtility
{
    public class SimpleRequest<T> : Request<T> where T : class
    {
        public override string UserID { get; set; }
    }
}
