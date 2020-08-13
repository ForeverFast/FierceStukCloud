using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FierceStukCloud.Abstractions
{
    public interface IStatusExistence
    {  
        string UserLogin { get; set; }  
        bool OnServer { get; set; }
        bool OnDevice { get; set; }
    }
}
