using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Audit.Net.JSONConverter
{
    class AuditJsonConverter : DefaultContractResolver
    {
        private readonly List<string> _namesOfVirtualPropsToKeep = new List<string>(new string[] { });

        public AuditJsonConverter() { }

        public AuditJsonConverter(IEnumerable<string> namesOfVirtualPropsToKeep)
        {
            _namesOfVirtualPropsToKeep = namesOfVirtualPropsToKeep.Select(x => x.ToLower()).ToList();
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);
            var propInfo = member as PropertyInfo;
            if (propInfo != null)
            {
                if (propInfo.GetMethod.IsVirtual &&
                    !propInfo.GetMethod.IsFinal &&
                    !_namesOfVirtualPropsToKeep.Contains(propInfo.Name.ToLower()))
                {
                    prop.ShouldSerialize = obj => false;
                }
            }
            return prop;
        }
    }
}
