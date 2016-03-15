using Microsoft.Xrm.Sdk;
using NSubstitute;
using System;

namespace TestHarnessServices
{
    /// <summary>
    /// Service class inherited from CrmService that provides functions to mock CRM Plugin specific objects
    /// </summary>
    public class PluginService : CrmService
    {
        #region Private Variables

        /// <summary>
        /// Plugin Execution Context
        /// </summary>
        private IPluginExecutionContext _pluginExecutionContext;

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns a mocked version of the IPluginExecutionContext object, that will return the Target entity, primary entity Id, and current users Id
        /// </summary>
        public IPluginExecutionContext PluginExecutionContext
        {
            get
            {
                if (_pluginExecutionContext == null)
                {
                    if (this.CrmContextInfo.Entity == null)
                    {
                        throw new NullReferenceException("CrmContext is empty");
                    }
                    else
                    {
                        var parameterCollection = new ParameterCollection();
                        parameterCollection.Add("Target", base.CrmContextInfo.Entity);

                        var pluginExecutionContextMock = Substitute.For<IPluginExecutionContext>();
                        pluginExecutionContextMock.PrimaryEntityId.Returns(this.CrmContextInfo.EntityId);
                        pluginExecutionContextMock.UserId.Returns(this.CurrentUserId);
                        pluginExecutionContextMock.InitiatingUserId.Returns(this.CurrentUserId);
                        pluginExecutionContextMock.InputParameters.Returns(parameterCollection);

                        return pluginExecutionContextMock;
                    }
                }

                return _pluginExecutionContext;
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="connectionName"></param>
        public PluginService(string connectionName) : base(connectionName) { }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="crmContextInfo"></param>
        public PluginService(CrmContextInfo crmContextInfo, string connectionName) : base(crmContextInfo, connectionName) { }
        
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns a mocked version of the IServiceProvider object, including mocking the 
        /// calls to retrieve IPluginExecutionContext and IOrganizationServiceFactory objects
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IServiceProvider GetServiceProvider(Entity entity)
        {
            var serviceProviderMock = Substitute.For<IServiceProvider>();
            var pluginExecutionContext = this.PluginExecutionContext;
            var organizationServiceFactory = this.OrganizationServiceFactory;
            
            serviceProviderMock.GetService(typeof(IPluginExecutionContext)).Returns(pluginExecutionContext);
            serviceProviderMock.GetService(typeof(IOrganizationServiceFactory)).Returns(organizationServiceFactory);

            return serviceProviderMock;
        }
        #endregion
    }
}
