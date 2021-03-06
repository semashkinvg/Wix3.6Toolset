//-------------------------------------------------------------------------------------------------
// <copyright file="RegistryBase.cs" company="Microsoft">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
//    
//    The use and distribution terms for this software are covered by the
//    Common Public License 1.0 (http://opensource.org/licenses/cpl1.0.php)
//    which can be found in the file CPL.TXT at the root of this distribution.
//    By using this software in any fashion, you are agreeing to be bound by
//    the terms of this license.
//    
//    You must not remove this notice, or any other, from this software.
// </copyright>
// 
// <summary>
// Contains the RegistryBase class.
// </summary>
//-------------------------------------------------------------------------------------------------

namespace Microsoft.Tools.WindowsInstallerXml.WixBuild
{
    using System;
    using System.Security;
    using System.Security.AccessControl;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using Microsoft.Win32;

    /// <summary>
    /// Abstract base class for defining an MSBuild task for working with the registry.
    /// </summary>
    public abstract class RegistryBase : Task
    {
        // =========================================================================================
        // Member Variables
        // =========================================================================================

        private RegistryHive hive;
        private string key;
        private string name;

        // =========================================================================================
        // Constructors
        // =========================================================================================

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryBase"/> class.
        /// </summary>
        protected RegistryBase()
        {
        }

        // =========================================================================================
        // Properties
        // =========================================================================================

        /// <summary>
        /// Gets or sets the registry hive to search for <see cref="Key"/>.
        /// </summary>
        /// <value>One of the <see cref="RegistryHive"/> values.</value>
        [Required]
        public string Hive
        {
            get { return this.hive.ToString(); }
            set { this.hive = (RegistryHive)Enum.Parse(typeof(RegistryHive), value); }
        }

        /// <summary>
        /// Gets or set the registry key to read, including the path.
        /// </summary>
        [Required]
        public string Key
        {
            get { return this.key; }
            set { this.key = value; }
        }

        /// <summary>
        /// Gets or sets the name of the registry entry to read. Null or blank values indicate the default key.
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// Gets the <see cref="RegistryKey"/> associated with <see cref="Hive"/>.
        /// </summary>
        protected RegistryKey HiveRegistryKey
        {
            get
            {
                switch (this.hive)
                {
                    case RegistryHive.ClassesRoot:
                        return Registry.ClassesRoot;

                    case RegistryHive.CurrentUser:
                        return Registry.CurrentUser;

                    case RegistryHive.LocalMachine:
                        return Registry.LocalMachine;

                    case RegistryHive.Users:
                        return Registry.Users;

                    case RegistryHive.CurrentConfig:
                    case RegistryHive.DynData:
                    case RegistryHive.PerformanceData:
                    default:
                        this.Log.LogError("Registry hive {0} not found or not supported.", this.Hive);
                        return null;
                }
            }
        }
    }
}
