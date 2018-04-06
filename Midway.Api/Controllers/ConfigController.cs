using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Midway.Api.DataConnector;
using Midway.Api.Models;

namespace Midway.Api.Controllers
{
    public class ConfigController : Controller
    {
        private readonly IDataProvider _dataProvider;

        public ConfigController(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public ActionResult Post(NameValueRequest request)
        {
            var newModel = new NameValueModel
            {
                Name = request.Name,
                Value = request.Value,
                GroupName = request.GroupName,
                Environment = request.Environment
            };

            _dataProvider.Create(newModel);

            return Created("", "");
        }

        public async Task<ActionResult> Get(string environment)
        {
            var result = _dataProvider.GetEnvironment(environment);

            return Ok(result);
        }

        public async Task<ActionResult> Get()
        {
            var result = _dataProvider.GetAll();

            return Ok(result);
        }
    }
}