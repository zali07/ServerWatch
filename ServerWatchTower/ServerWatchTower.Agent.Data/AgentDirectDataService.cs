namespace ServerWatchTower.Agent.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;
    using ServerWatchTower.Agent.Model;
    using Cosys.SilverLib.Data;
    using Cosys.SilverLib.Model;

    /// <summary>
    /// The <see cref="IAgentDataService"/> implementation which connects directly to the database.
    /// </summary>
    [Export(typeof(IAgentDataService))]
    internal class AgentDirectDataService : IAgentDataService
    {
        #region Private fields

        private readonly IConnectionStringProvider connectionProvider;

        private readonly ISessionProvider sessionProvider;

        private readonly ICultureProvider cultureProvider;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentDirectDataService"/> class.
        /// </summary>
        /// <param name="connectionProvider">The connection string provider of the application.</param>
        /// <param name="sessionProvider">The session provider of the application.</param>
        /// <param name="cultureProvider">The culture provider of the application.</param>
        [ImportingConstructor]
        public AgentDirectDataService(IConnectionStringProvider connectionProvider, ISessionProvider sessionProvider, ICultureProvider cultureProvider)
        {
            this.connectionProvider = connectionProvider;
            this.sessionProvider = sessionProvider;
            this.cultureProvider = cultureProvider;
        }



        #region Background task starting methods

        /// <summary>
        /// Starts a new <see cref="Task{TResult}"/> for running a method on the <see cref="AgentDataLayer"/> in the background.
        /// </summary>
        /// <typeparam name="TResult">The type of the result the data layer method will return.</typeparam>
        /// <param name="dataLayerMethodCall">The delegate which will call the data layer method.</param>
        /// <returns>A <see cref="Task{TResult}"/> with its <see cref="Task{TResult}.Result"/> being the data
        /// returned by the <paramref name="dataLayerMethodCall"/> delegate.</returns>
        private Task<TResult> RunOnDataLayerAsync<TResult>(Func<AgentDataLayer, TResult> dataLayerMethodCall)
        {
            return TaskEx.Run(
                () =>
                {
                    this.cultureProvider.SetCultureForThread();

                    using (var dl = new AgentDataLayer(this.connectionProvider))
                    {
                        return dl.RunWithErrorHandling(dataLayerMethodCall);
                    }
                });
        }

        /// <summary>
        /// Starts a new <see cref="Task{TResult}"/> for running a method on the <see cref="AgentDataLayer"/> in the background.
        /// </summary>
        /// <param name="dataLayerMethodCall">The delegate which will call the data layer method.</param>
        /// <returns>A <see cref="Task"/> which will run the <paramref name="dataLayerMethodCall"/> delegate.</returns>
        private Task RunOnDataLayerAsync(Action<AgentDataLayer> dataLayerMethodCall)
        {
            return TaskEx.Run(
                () =>
                {
                    this.cultureProvider.SetCultureForThread();

                    using (var dl = new AgentDataLayer(this.connectionProvider))
                    {
                        dl.RunWithErrorHandling(dataLayerMethodCall);
                    }
                });
        }

        /// <summary>
        /// Starts a new <see cref="Task{TResult}"/> for running a method on the <see cref="AgentDataLayer"/> in the background.
        /// </summary>
        /// <typeparam name="TResult">The type of the result the data layer method will return.</typeparam>
        /// <param name="dataLayerMethodCall">The delegate which will call the data layer method.</param>
        /// <returns>A <see cref="Task{TResult}"/> with its <see cref="Task{TResult}.Result"/> being the data
        /// returned by the <paramref name="dataLayerMethodCall"/> delegate.</returns>
        private Task<TResult> RunOnAuthorizedDataLayerAsync<TResult>(Func<AgentDataLayer, TResult> dataLayerMethodCall)
        {
            return TaskEx.Run(
                () =>
                {
                    this.cultureProvider.SetCultureForThread();

                    using (var dl = new AgentDataLayer(this.connectionProvider, this.sessionProvider))
                    {
                        return dl.RunWithErrorHandling(dataLayerMethodCall);

                    }
                });
        }

        /// <summary>
        /// Starts a new <see cref="Task{TResult}"/> for running a method on the <see cref="AgentDataLayer"/> in the background.
        /// </summary>
        /// <param name="dataLayerMethodCall">The delegate which will call the data layer method.</param>
        /// <returns>A <see cref="Task"/> which will run the <paramref name="dataLayerMethodCall"/> delegate.</returns>
        private Task RunOnAuthorizedDataLayerAsync(Action<AgentDataLayer> dataLayerMethodCall)
        {
            return TaskEx.Run(
                () =>
                {
                    this.cultureProvider.SetCultureForThread();

                    using (var dl = new AgentDataLayer(this.connectionProvider, this.sessionProvider))
                    {
                        dl.RunWithErrorHandling(dataLayerMethodCall);
                    }
                });
        }

        #endregion
    }
}
