using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FierceStukCloud.Abstractions
{
    public interface IStatusExistence
    {
        [JsonIgnore]
        bool OnServer { get; set; }

        [JsonIgnore]
        bool OnPC { get; set; }

        [JsonIgnore]
        bool OnPhone { get; set; }
    }
}
