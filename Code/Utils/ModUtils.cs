using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using ICities;
using ColossalFramework.Plugins;
using EnlightenYourMouse;


namespace AlgernonUtils
{
    /// <summary>
    /// Class that manages interactions with other mods, including compatibility and functionality checks.
    /// </summary>
    internal static class ModUtils
    {
        /// <summary>
        /// Returns the filepath of the current mod assembly.
        /// </summary>
        /// <returns>Mod assembly filepath</returns>
        internal static string GetAssemblyPath()
        {
            // Get list of currently active plugins.
            IEnumerable<PluginManager.PluginInfo> plugins = PluginManager.instance.GetPluginsInfo();

            // Iterate through list.
            foreach (PluginManager.PluginInfo plugin in plugins)
            {
                try
                {
                    // Get all (if any) mod instances from this plugin.
                    IUserMod[] mods = plugin.GetInstances<IUserMod>();

                    // Check to see if the primary instance is this mod.
                    if (mods.FirstOrDefault() is EnlightenYourMouseMod)
                    {
                        // Found it! Return path.
                        return plugin.modPath;
                    }
                }
                catch
                {
                    // Don't care.
                }
            }

            // If we got here, then we didn't find the assembly.
            Logging.Message("assembly path not found");
            throw new FileNotFoundException(EnlightenYourMouseMod.ModName + ": assembly path not found!");
        }


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
