namespace LoginSystem.Extensions
{
    public class SessionExpirationManager
    {
        private static SessionExpirationManager _instance;
        private static readonly object _lock = new object();

        private SessionExpirationManager() { }

        public static SessionExpirationManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new SessionExpirationManager();
                    }
                    return _instance;
                }
            }
        }

        public bool IsSessionExpired(HttpContext context)
        {
            var session = context.Session;
            var sessionExpired = session.GetString("_SessionIsExpired");
            return !string.IsNullOrEmpty(sessionExpired) && sessionExpired.Equals("true");
        }

        public void SetSessionExpired(HttpContext context, bool expired)
        {
            var session = context.Session;
            session.SetString("_SessionIsExpired", expired ? "true" : "false");
        }

    }

}