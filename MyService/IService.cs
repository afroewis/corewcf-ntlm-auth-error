using CoreWCF;
using System;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Authorization;

namespace MyService
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        [Authorize(Policy = "Deny")]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
    }

    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    [Authorize(Policy = "Deny")]
    public class Service(IHttpContextAccessor httpContextAccessor) : IService
    {
        [Authorize(Policy = "Deny")]
        public string GetData(int value)
        {
            // This should never be hit, because the policy should disallow any request.
            var user = httpContextAccessor.HttpContext.User;
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
