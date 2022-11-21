// <copyright file="ModUtils.cs" company="algernon (K. Algernon A. Sheppard)">
// Copyright (c) algernon (K. Algernon A. Sheppard). All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace EnlightenYourMouse
{
    using System;
    using System.Reflection;
    using AlgernonCommons;
    using ColossalFramework.Plugins;

    /// <summary>
    /// Class that manages interactions with other mods, including compatibility and functionality checks.
    /// </summary>
    internal static class ModUtils
    {
        /// <summary>
        /// Uses reflection to find the MoveItTool.RenderGeometry method of Move It.
        /// </summary>
        /// <returns>MethodBase reference to MoveItTool.RenderGeometry if successful, null if unsuccessful.</returns>
        internal static MethodBase MoveItReflection()
        {
            Logging.KeyMessage("Attempting to find Move It");

            // Iterate through each loaded plugin assembly.
            foreach (PluginManager.PluginInfo plugin in PluginManager.instance.GetPluginsInfo())
            {
                foreach (Assembly assembly in plugin.GetAssemblies())
                {
                    if (assembly.GetName().Name.Equals("MoveIt") && plugin.isEnabled)
                    {
                        Logging.KeyMessage("Found Move It");

                        // Found MoveIt.dll that's part of an enabled plugin; try to get its MoveItTool class.
                        Type moveItTools = assembly.GetType("MoveIt.MoveItTool");

                        if (moveItTools != null)
                        {
                            // Try to get LockBuildingLevel method.
                            MethodBase moveItRenderGeometry = moveItTools.GetMethod("RenderGeometry", BindingFlags.Public | BindingFlags.Instance);
                            if (moveItRenderGeometry != null)
                            {
                                // Success!
                                Logging.KeyMessage("found MoveItTool.RenderGeometry");

                                // At this point, we're done; return.
                                return moveItRenderGeometry;
                            }
                        }
                    }
                }
            }

            // If we got here, we were unsuccessful.
            Logging.KeyMessage("MoveItTool.RenderGeometry not found");
            return null;
        }
    }
}
