using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.ViewModels.Helpers
{
    public class UriHelper
    {
        public static bool TryCreateWebUri(string uriString, out Uri uri)
        {
            if (!Uri.TryCreate(uriString.Trim(), UriKind.Absolute, out uri))
            {
                return false;
            }

            if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            {
                return false;
            }

            return true;
        }
    }
}
