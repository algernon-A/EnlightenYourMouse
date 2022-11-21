// <copyright file="RenderGeometryPatches.cs" company="algernon (K. Algernon A. Sheppard)">
// Copyright (c) algernon (K. Algernon A. Sheppard). All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace EnlightenYourMouse
{
    using System.Collections.Generic;
    using System.Reflection;
    using ColossalFramework;
    using HarmonyLib;
    using UnityEngine;

    /// <summary>
    /// Harmony prefix to extend mouse lighting to selected tools that don't have it.
    /// Currently supported vanilla tools: NetTool, BulldozeTool, ZoneTool.
    /// Currently supported mod tools: MoveItTool.
    /// </summary>
    [HarmonyPatch]
    public static class RenderGeometryPatches
    {
        /// <summary>
        /// Determines the target method for the Prefix patch.
        /// </summary>
        /// <returns>RenderGeometry methods of NetTool and MoveItTool (if available).</returns>
        public static IEnumerable<MethodBase> TargetMethods()
        {
            // NetTool.
            yield return typeof(NetTool).GetMethod(nameof(NetTool.RenderGeometry));

            // BulldozeTool.
            yield return typeof(BulldozeTool).GetMethod(nameof(BulldozeTool.RenderGeometry));

            // ZoneTool.
            yield return typeof(ZoneTool).GetMethod(nameof(ZoneTool.RenderGeometry));

            // MoveItTool.
            MethodBase moveItRenderGeometry = ModUtils.MoveItReflection();
            if (moveItRenderGeometry != null)
            {
                yield return moveItRenderGeometry;
            }
        }

        /// <summary>
        /// Harmony Prefix patch for tool RenderGeometry method to add custom mouse lighting.
        /// </summary>
        /// <param name="cameraInfo">Current camera reference.</param>
        public static void Prefix(RenderManager.CameraInfo cameraInfo)
        {
            // If we're not in a special infomode, get raycast target point and apply mouse light.
            if (Singleton<InfoManager>.instance.CurrentMode == InfoManager.InfoMode.None && ToolRayCast.BaseRayCast(new ToolBase.RaycastInput(Camera.main.ScreenPointToRay(Input.mousePosition), Camera.main.farClipPlane), out ToolBase.RaycastOutput output))
            {
                MouseLight.DrawMouseLight(cameraInfo, output.m_hitPos);
            }
        }
    }
}
