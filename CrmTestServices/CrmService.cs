using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using NSubstitute;
using System;

namespace TestHarnessServices
{
    /// <summary>
    /// Base class for CRM services
    /// </summary>
    public abstract class CrmService
    {
        #region Properties
        #region Private Variables

        /// <summary>
        /// Crm Connection Name
        /// </summary>
        private string _crmConnectionName;

        /// <summary>
        /// Organization Service
        /// </summary>
        private IOrganizationService _organizationService;

        /// <summary>
        /// Tracing Service
        /// </summary>
        private ITracingService _tracingService;

        /// <summary>
        /// Organization Service Factory
        /// </summary>
        private IOrganizationServiceFactory _organizationServiceFactory;

        /// <summary>
        /// Current Users Id
        /// </summary>
        private Guid? _currentUserId;

        #endregion

        #region Protected Properties

        /// <summary>
        /// Crm Context Info
        /// </summary>
        protected CrmContextInfo CrmContextInfo { get; set; }
        
        #endregion

        #region Public Properties

        /// <summary>
        /// Organization Service property
        /// </summary>
        public IOrganizationService OrganizationService
        {
            get
            {
                if (_organizationService == null)
                {
                    var crmConnection = new CrmConnection(_crmConnectionName);
                    _organizationService = new OrganizationService(crmConnection);
                }

                return _organizationService;
            }
        }

        /// <summary>
        /// Tracing Service property
        /// </summary>
        public ITracingService TracingService
        {
            get
            {
                if (_tracingService == null)
                {
                    _tracingService = Substitute.For<ITracingService>();
                }

                return _tracingService;
            }
        }

        /// <summary>
        /// Returns a mocked IOrganizationServiceFactory object, and mocks the CreateOrganizationService method
        /// </summary>
        public IOrganizationServiceFactory OrganizationServiceFactory
        {
            get
            {
                if (_organizationServiceFactory == null)
                {
                    var organizationServiceFactoryMock = Substitute.For<IOrganizationServiceFactory>();
                    organizationServiceFactoryMock.CreateOrganizationService(Arg.Any<Guid>()).Returns(this.OrganizationService);

                    _organizationServiceFactory = organizationServiceFactoryMock;
                }

                return _organizationServiceFactory;
            }
        }

        /// <summary>
        /// Returns the User Id for the system user used to log into CRM
        /// </summary>
        public Guid CurrentUserId
        {
            get
            {
                if (_currentUserId == null)
                {
                    var whoAmIResponse = (WhoAmIResponse)this.OrganizationService.Execute(new WhoAmIRequest());

                    _currentUserId = whoAmIResponse.UserId; 
                }

                return _currentUserId.Value;
            }            
        }

        #endregion 
        #endregion

        #region Constructor

        public CrmService(string connectionName)
        {
            _crmConnectionName = connectionName;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="crmContextInfo"></param>
        public CrmService(CrmContextInfo crmContextInfo, string connectionName)
        {
            this.CrmContextInfo = crmContextInfo;
            _crmConnectionName = connectionName;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Set CrmContextInfo after service object has been initialised via parameterless constructor
        /// </summary>
        /// <param name="crmContextInfo"></param>
        public void SetCrmContextInfo(CrmContextInfo crmContextInfo)
        {
            this.CrmContextInfo = crmContextInfo;
        }

        /// <summary>
        /// Resets any member variables so that service can be re-used (required if you add extra mocking to these objects)
        /// </summary>
        public void Reset()
        {
            this._organizationService = null;
            this._organizationServiceFactory = null;
            this._currentUserId = null;
            this._tracingService = null;
            this.CrmContextInfo = null;          
        }
        
        #endregion
    }
}
