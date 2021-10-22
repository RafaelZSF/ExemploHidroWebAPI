using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HidroWebAPI.Models.Responses.Http
{
    public class BadRequestResponse
    {
        public Dictionary<string, List<string>> Errors { get; set; }
    }
}
