using Audit.Net.JSONConverter;
using Newtonsoft.Json;
using System;
using System.Data.Entity.Core.Objects;

namespace Audit.Net.Models
{
    public class AuditModel
    {
        [JsonIgnore()]
        public virtual string AuditJson
        {
            get
            {
                try
                {
                    var json = JsonConvert.SerializeObject(
                        this,
                        Formatting.None,
                        new JsonSerializerSettings
                        {
                            MissingMemberHandling = MissingMemberHandling.Ignore,
                            ContractResolver = new AuditJsonConverter(),
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        }
                    );

                    return json;

                }
                catch (Exception exce)
                {
                    return exce.Message;
                }
            }
        }

        [JsonIgnore()]
        public virtual string EntityType
        {
            get
            {
                return ObjectContext.GetObjectType(GetType()).Name;
            }
        }
        [JsonIgnore()]
        public virtual long Identifier
        {
            get
            {
                return GetHashCode();
            }
        }
        [JsonIgnore()]
        public virtual bool CanAudit
        {
            get
            {
                return true;
            }
        }
    }
}
