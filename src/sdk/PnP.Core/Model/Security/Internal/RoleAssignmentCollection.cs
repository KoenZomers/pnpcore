using PnP.Core.QueryModel;
using PnP.Core.Services;
using System.Threading.Tasks;
using System.Linq;

namespace PnP.Core.Model.Security
{
    internal partial class RoleAssignmentCollection : QueryableDataModelCollection<IRoleAssignment>, IRoleAssignmentCollection
    {
        public RoleAssignmentCollection(PnPContext context, IDataModelParent parent, string memberName = null) : base(context, parent, memberName)
        {
            PnPContext = context;
            Parent = parent;
        }
    }
}
