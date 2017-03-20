namespace bUtility
{
    public class SystemRequest<T> : Request<T> where T : class, IUserIDProvider, new()
    {
        public override string UserID
        {
            get { return Payload?.UserID; }
            set
            {
                if (Payload == null)
                {
                    Payload = new T();
                }
                Payload.UserID = value;
            }
        }
    }
}
