using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TracingDemoApplication.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            HttpClient client = new HttpClient();
            var requestHeaders = string.Join(",", Request.Headers.ToList());
            AddHeaderIfNotNull(client, Model.REQUEST_ID, Request.Headers[Model.REQUEST_ID].ToString());
            AddHeaderIfNotNull(client, Model.B3_TRACE_ID, Request.Headers[Model.B3_TRACE_ID].ToString());
            AddHeaderIfNotNull(client, Model.B3_SPAN_ID, Request.Headers[Model.B3_SPAN_ID].ToString());
            AddHeaderIfNotNull(client, Model.B3_PARENT_SPAN_ID, Request.Headers[Model.B3_PARENT_SPAN_ID].ToString());
            AddHeaderIfNotNull(client, Model.B3_SAMPLED, Request.Headers[Model.B3_SAMPLED].ToString());
            AddHeaderIfNotNull(client, Model.B3_FLAGS, Request.Headers[Model.B3_FLAGS].ToString());
            AddHeaderIfNotNull(client, Model.OT_SPAN_CONTEXT, Request.Headers[Model.OT_SPAN_CONTEXT].ToString());
            
            var dummystring = await client.GetStringAsync(@"https://dummy-service/api/v1/dummy");
            return $"Response : {dummystring}; DemoController Headers : {requestHeaders}";
        }
        private void AddHeaderIfNotNull(HttpClient client, string headerName, string headerValue)
        {
            if (!string.IsNullOrWhiteSpace(headerValue))
                client.DefaultRequestHeaders.TryAddWithoutValidation(headerName, headerValue);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    internal static class Model
    {

        // ------------- Tracing specific headers ---------------------

        /// <summary>
        /// The request identifier
        /// </summary>
        public const string REQUEST_ID = "x-request-id";

        /// <summary>
        /// The b3 trace identifier
        /// </summary>
        public const string B3_TRACE_ID = "x-b3-traceid";

        /// <summary>
        /// The b3 span identifier
        /// </summary>
        public const string B3_SPAN_ID = "x-b3-spanid";

        /// <summary>
        /// The b3 parent span identifier
        /// </summary>
        public const string B3_PARENT_SPAN_ID = "x-b3-parentspanid";

        /// <summary>
        /// The b3 sampled
        /// </summary>
        public const string B3_SAMPLED = "x-b3-sampled";

        /// <summary>
        /// The b3 flags
        /// </summary>
        public const string B3_FLAGS = "x-b3-flags";

        /// <summary>
        /// The ot span context
        /// </summary>
        public const string OT_SPAN_CONTEXT = "x-ot-span-context";
    }
}
