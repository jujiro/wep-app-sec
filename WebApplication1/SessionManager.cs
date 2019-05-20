using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace WebFormAndWebApiLab
{
    public static class SessionManager
    {
        static object _lck = new object();
        private static string _sessionsFile = HttpContext.Current.Server.MapPath(@"~/App_data/sessions.txt");
        private static ConcurrentBag<SessionUser> _sessions = new ConcurrentBag<SessionUser>();
        public static void LoadPastSessions()
        {
            if (!File.Exists(_sessionsFile)) return;
            var fs = new FileStream(_sessionsFile, FileMode.Open);
            try
            {
                fs.Lock(0, 0);
                string fileContents;
                using (StreamReader reader = new StreamReader(fs))
                {
                    fileContents = reader.ReadToEnd();
                }
                var savedSessions = JsonConvert.DeserializeObject<ConcurrentBag<SessionUser>>(fileContents);
                // Clear all existing sessions in memory
                Interlocked.Exchange<ConcurrentBag<SessionUser>>(ref _sessions, new ConcurrentBag<WebFormAndWebApiLab.SessionUser>());
                // Skip loading of the retried sessions from the file.
                foreach (var sess in savedSessions)
                    if (sess.ExpiresOn > DateTime.UtcNow) _sessions.Add(sess);
                fs.Close();
                File.Delete(_sessionsFile);
            }
            catch { }
            finally
            {
                fs.Close();
                fs.Dispose();
            }
        }
        public static void SaveSessionsToFile()
        {
            try
            {
                if (File.Exists(_sessionsFile)) File.Delete(_sessionsFile);
                var fs = new FileStream(_sessionsFile, FileMode.CreateNew);
                string fileContents = JsonConvert.SerializeObject(_sessions);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(fileContents);
                }
                fs.Close();
            }
            catch { }
        }
        public static ConcurrentBag<SessionUser> Sessions
        {
            get { return _sessions; }
            set { _sessions = value; }
        }
        public static SessionUser CreateSession(string name)
        {
            var sess = new SessionUser(name);
            //
            sess.Token = Guid.NewGuid();
            
            // Both token and blah should match        
            sess.ExpiresOn = DateTime.UtcNow.AddMinutes(5);
            _sessions.Add(sess);
            return sess;
        }
        public static void KillSession(Guid token)
        {
            lock (_lck)
            {
                Interlocked.Exchange<ConcurrentBag<SessionUser>>(ref _sessions, new ConcurrentBag<WebFormAndWebApiLab.SessionUser>(_sessions.Where(zz => (zz.Token != token))));
            }
        }
        public static SessionUser FindSession(Guid token)
        {
            return _sessions.FirstOrDefault(zz => (zz.Token == token && zz.ExpiresOn > DateTime.UtcNow));
        }
    }
}
