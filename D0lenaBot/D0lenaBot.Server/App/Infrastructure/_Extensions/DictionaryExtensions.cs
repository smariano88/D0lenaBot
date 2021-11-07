using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D0lenaBot.Server.App.Infrastructure.Extensions
{
    public static class DictionaryExtensions
    {
        public static string ToQueryStringArgs(this Dictionary<string, string> values)
        {
            var formatedArg = values.Select(kvp => string.Format("{0}={1}", HttpUtility.UrlEncode(kvp.Key), (kvp.Value)));
            return string.Join("&", formatedArg);
        }
    }
}
