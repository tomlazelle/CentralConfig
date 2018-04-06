using Midway.Api.Models;
using System.Collections.Generic;

namespace Midway.Api.DataConnector
{
    public interface IDataProvider
    {
        bool Create(NameValueModel nameValueModel);
        IEnumerable<NameValueRequest> GetEnvironment(string environment);
        IEnumerable<NameValueRequest> GetAll();
    }
}