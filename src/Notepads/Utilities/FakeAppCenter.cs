using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AppCenter
{
    namespace Analytics
    {
        public static class Analytics
        {
            public static void TrackEvent(string name, object obj = null)
            {
            }
        }
    }

    namespace Crashes
    {
        public class ErrorAttachmentLog
        {
            public static int AttachmentWithText(string message, string name)
            {
                return 0;
            }
        }

        public static class Crashes
        {
            public static void TrackError(Exception ex, object obj, int attachment = 0)
            {
            }
        }
    }

    public static class AppCenter
    {
        public static void Start(string appSecret, Type[] services)
        {
        }
    }
}
