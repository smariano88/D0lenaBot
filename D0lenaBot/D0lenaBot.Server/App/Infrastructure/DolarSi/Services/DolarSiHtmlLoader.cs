using HtmlAgilityPack;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Infrastructure.DolarSi.Services
{
    internal interface IDolarSiHtmlLoader
    {
        Task<HtmlDocument> Load(string url);
    }

    internal class DolarSiHtmlLoader : IDolarSiHtmlLoader
    {
        public async Task<HtmlDocument> Load(string url)
        {
            var web = new HtmlWeb();
            return await web.LoadFromWebAsync(url);
        }
    }
}