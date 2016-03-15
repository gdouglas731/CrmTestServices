using Microsoft.Xrm.Sdk;
using System;

namespace TestHarnessServices
{
    /// <summary>
    /// Stores relevant information needed to mock CRM plugin and workflow objects.
    /// </summary>
    public class CrmContextInfo
    {
        /// <summary>
        /// Constructor for workflow
        /// </summary>
        /// <param name="primaryEntityId"></param>
        /// <param name="entityName"></param>
        public CrmContextInfo(Guid primaryEntityId, string entityName)
        {
            this.Entity = new Entity(entityName)
            {
                Id = primaryEntityId
            };
        }

        /// <summary>
        /// Constructor for plugin
        /// </summary>
        /// <param name="entity"></param>
        public CrmContextInfo(Entity entity)
        {
            this.Entity = entity;
        }

        //public CrmContextInfo()
        //{
        //}
        
        /// <summary>
        /// Primary Entity Id
        /// </summary>
        public Guid EntityId {
            get
            {
                if (this.Entity != null)
                {
                    return this.Entity.Id;
                }
                else
                {
                    throw new InvalidOperationException("Entity is null");
                }
            }
        }

        /// <summary>
        /// Entity Name
        /// </summary>
        public string EntityName
        {
            get
            {
                if (this.Entity != null)
                {
                    return this.Entity.LogicalName;
                }
                else
                {
                    throw new InvalidOperationException("Entity is null");
                }
            }
        }
        public Entity Entity { get; set; }
    }
}
