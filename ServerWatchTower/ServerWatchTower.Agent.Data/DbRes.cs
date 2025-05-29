//-----------------------------------------------------------------------
// <copyright file="DbRes.cs" company="Cosys SRL.">
//     Copyright (c) 2012, 2024 Cosys SRL. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ServerWatchTower.Agent.Data
{
    using System;
    using System.Diagnostics;
    using System.Resources;
    using System.Threading;
    using System.Globalization;

    /// <summary>
    /// Static class providing the localized error message strings for the database operations.
    /// </summary>
    internal static class DbRes
    {
        /// <summary>
        /// The cached <see cref="ResourceManager"/> instance used by this class.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceManager _resMgr;

        /// <summary>
        /// Gets the cached <see cref="System.Resources.ResourceManager"/> instance used by this class.
        /// </summary>
        public static ResourceManager ResourceManager
        {
            [DebuggerStepThrough]
            get
            {
                if (_resMgr == null)
                {
                    var tmp = new ResourceManager(typeof(DbRes));
                    Interlocked.CompareExchange(ref _resMgr, tmp, null);
                }

                return _resMgr;
            }
        }

        /// <summary>
        /// Gets the value of the string resource localized for the 
        /// <see cref="CultureInfo.CurrentUICulture"/> culture.
        /// </summary>
        /// <param name="resourceName">The name of the resource to return.</param>
        /// <returns>The value of the loaded resource.</returns>
        [DebuggerStepThrough]
        public static string GetString(string resourceName)
        {
            return ResourceManager.GetString(resourceName);
        }

        /// <summary>
        /// Gets the value of the string resource with the specified identifier for
        /// the <see cref="CultureInfo.CurrentUICulture"/> culture.
        /// </summary>
        /// <param name="resourceId">The identifier of the resource.</param>
        /// <returns>The value of the loaded resource, or <c>null</c> when not found.</returns>
        [DebuggerStepThrough]
        public static string GetString(int resourceId)
        {
            return ResourceManager.GetString(resourceId.ToString(CultureInfo.InvariantCulture));
        }


        /// <summary>
        /// Gets the value of the string resource localized for a 
        /// specific culture.
        /// </summary>
        /// <param name="culture">The culture to which the resource should be returned.</param>
        /// <param name="resourceName">The name of the resource to return.</param>
        /// <returns>The value of the loaded resource.</returns>
        [DebuggerStepThrough]
        public static string GetString(string resourceName, CultureInfo culture)
        {
            return ResourceManager.GetString(resourceName, culture ?? CultureInfo.CurrentUICulture);
        }
    }
}
