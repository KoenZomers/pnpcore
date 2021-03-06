using PnP.Core.Services;
using System.Linq;

namespace PnP.Core.Model.SharePoint
{
    /// <summary>
    /// Public interface to define a collection of RoleDefinition objects
    /// </summary>
    [ConcreteType(typeof(RoleDefinitionCollection))]
    public interface IRoleDefinitionCollection : IQueryable<IRoleDefinition>, IDataModelCollection<IRoleDefinition>
    {
    }
}