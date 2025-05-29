//-----------------------------------------------------------------------
// <copyright file="AgentConfiguration.generated.cs" company="Cosys SRL.">
//     Copyright (c) 2012, 2025 Cosys SRL. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ServerWatchTower.Agent.Model
{
    using System;
	using System.Collections.Specialized;
    using System.ComponentModel.Composition;
    using Cosys.SilverLib.Model;

    /// <summary>
    /// Class which provides access to the configuration settings of the Agent module.
    /// </summary>
    /// <remarks>
    /// <para>The configuration setting of a module are read-only, and they are loaded from the
	/// <c>dtConfig</c> table at application startup.</para>
    /// </remarks>
    [Export, Export("ServerWatchTower.Agent.AgentConfiguration"), PartCreationPolicy(CreationPolicy.Shared)]
    public partial class AgentConfiguration : ModuleConfigurationBase
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentConfiguration"/> class with
        /// the <see cref="SilverConfiguration"/> instance which provides the underlying data.
        /// </summary>
        /// <param name="config">The configuration store which provides the data for the new instance.</param>
        [ImportingConstructor]
        public AgentConfiguration(SilverConfiguration config) : base(config)
        {
			// loading the properties from the store
        }

		#endregion
    }
}

