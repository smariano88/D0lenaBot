using System;
using System.Collections.Generic;
using System.Text;

namespace D0lenaBot.Server.App.Application.FetchDollarQuery
{
    public interface IFetchDollarQuery
    {
        void Fetch(DateTime date);
    }
}
