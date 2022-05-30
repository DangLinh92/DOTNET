using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Authorization
{
    public static class Operations
    {
        public static OperationAuthorizationRequirement Create =
            new OperationAuthorizationRequirement { Name = nameof(Create) };
        public static OperationAuthorizationRequirement Read =
            new OperationAuthorizationRequirement { Name = nameof(Read) };
        public static OperationAuthorizationRequirement Update =
            new OperationAuthorizationRequirement { Name = nameof(Update) };
        public static OperationAuthorizationRequirement Delete =
            new OperationAuthorizationRequirement { Name = nameof(Delete) };
        public static OperationAuthorizationRequirement Import =
           new OperationAuthorizationRequirement { Name = nameof(Import) };
        public static OperationAuthorizationRequirement Export =
          new OperationAuthorizationRequirement { Name = nameof(Export) };

        public static OperationAuthorizationRequirement ApproveL1 =
          new OperationAuthorizationRequirement { Name = nameof(ApproveL1) };

        public static OperationAuthorizationRequirement ApproveL2 =
          new OperationAuthorizationRequirement { Name = nameof(ApproveL2) };

        public static OperationAuthorizationRequirement ApproveL3 =
          new OperationAuthorizationRequirement { Name = nameof(ApproveL3) };
    }
}
