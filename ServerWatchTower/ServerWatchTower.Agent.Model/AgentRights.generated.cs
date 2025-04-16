//-----------------------------------------------------------------------
// <copyright file="AgentRights.generated.cs" company="Cosys SRL.">
//     Copyright (c) 2012, 2025 Cosys SRL. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ServerWatchTower.Agent.Model
{
    using System;
	using System.Collections.Specialized;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using Cosys.SilverLib.Model;

    /// <summary>
    /// Class through which the access rights of the current user to the Agent module can be verified.
    /// </summary>
    /// <remarks>
    /// <para>The rights of the user are loaded during startup, and will not be updated during
	/// the execution of the application.</para>
    /// </remarks>
    [Export("ServerWatchTower.Agent.AgentRights"), PartCreationPolicy(CreationPolicy.Shared)]
    public partial class AgentRights
    {
		#region Right constants

		#endregion

        #region Private fields

        /// <summary>
		/// The session provider through which the user rights can be verified.
		/// </summary>
		private ISessionProvider _sessionProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentRights"/> class.
        /// </summary>
        public AgentRights()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentRights"/> class with
        /// the <see cref="ISessionProvider"/> instance of the application.
        /// </summary>
        /// <param name="sessionProvider">The session provider through which the rights of the user can be checked.</param>
        public AgentRights(ISessionProvider sessionProvider)
        {
			// checking the rights of the user
            this.SessionProvider = sessionProvider;
        }

        #endregion

        #region Imports

        /// <summary>
        /// Gets or sets the session provider through which the rights of the user can be verified.
        /// </summary>
        [Import(typeof(ISessionProvider))]
        public ISessionProvider SessionProvider
        {
            get => this._sessionProvider;
            set
			{
			    if (!object.ReferenceEquals(this._sessionProvider, value))
				{
				    this._sessionProvider = value;

					var sessionInfo = value?.SessionInfo;

					if (sessionInfo == null)
					{
					    // the user doesn't have any rights
					}
					else
					{
						// checking and persisting the rights of the user
					}
				}
			}
        }

		#endregion
    }
}

