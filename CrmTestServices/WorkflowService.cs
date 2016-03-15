using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using NSubstitute;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHarnessServices
{
    /// <summary>
    /// Service class inherited from CrmService that provides functions to mock CRM Workflow specific objects
    /// </summary>
    public class WorkflowService : CrmService
    {
        #region Private Variables

        /// <summary>
        /// Workflow Context
        /// </summary>
        private IWorkflowContext _workflowContext;

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns a mocked version of the Workflow Context by mocking the current users ID, entity ID and entity name
        /// </summary>
        public IWorkflowContext WorkflowContext
        {
            get
            {
                if (_workflowContext == null)
                {
                    if (this.CrmContextInfo == null)
                    {
                        throw new NullReferenceException("CrmContextInfo is empty");
                    }
                    else
                    {
                        // setup WorkflowContext mock, return change of tenancy record Id
                        var workflowContextMock = Substitute.For<IWorkflowContext>();
                        workflowContextMock.UserId.Returns(this.CurrentUserId);
                        workflowContextMock.PrimaryEntityId.Returns(this.CrmContextInfo.EntityId);
                        workflowContextMock.PrimaryEntityName.Returns(this.CrmContextInfo.EntityName);

                        _workflowContext = workflowContextMock;
                    }
                }

                return _workflowContext;
            }
        }

        #endregion
        
        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public WorkflowService(string connectionName) : base(connectionName)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="crmContextInfo"></param>
        public WorkflowService(CrmContextInfo crmContextInfo, string connectionName) : base(crmContextInfo, connectionName) { }

        #endregion

        #region Public Methods
        /// <summary>
        /// Returns a mocked version of the Workflow Invoker, which allows us to call a workflow class directly.  
        /// This includes mocking the Tracing Service, Workflow Context and Organisation Service Factory
        /// </summary>
        /// <param name="workflow"></param>
        /// <returns></returns>
        public WorkflowInvoker GetWorkflowInvoker(CodeActivity workflow)
        {
            if (this.CrmContextInfo == null)
            {
                throw new NullReferenceException("CrmContextInfo is null");
            }
            else
            {
                WorkflowInvoker invoker = new WorkflowInvoker(workflow);
                // pass in our mocked extension classes to the workflow context
                invoker.Extensions.Add<ITracingService>(() => this.TracingService);
                invoker.Extensions.Add<IWorkflowContext>(() => this.WorkflowContext);
                invoker.Extensions.Add<IOrganizationServiceFactory>(() => this.OrganizationServiceFactory);

                return invoker; 
            }
        }
        #endregion        
    }
}
