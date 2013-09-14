using System;
using System.Windows.Navigation;

namespace Hydrate.Common
{
    class AssociationUriMapper : UriMapperBase
    {
        private string tempUri;

        public override Uri MapUri(Uri uri)
        {
            tempUri = System.Net.HttpUtility.UrlDecode(uri.ToString());

            if (tempUri.Contains("hydrate:newRecord"))
            {
                return new Uri("/MainPage.xaml?newRecord=true", UriKind.Relative);
            }

            return uri;
        }
    }
}