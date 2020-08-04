using System;
using System.Collections.Generic;
using System.Text;

namespace MyApplic
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {
        public readonly string ServiceName;

        public string DisplayName { get; set; }
        public string Description { get; set; }

        public ServiceAttribute(string serviceName)
        {
            ServiceName = serviceName;
        }
    }
}
