// <copyright file="DefaultToolPatches.cs" company="algernon (K. Algernon A. Sheppard)">
// Copyright (c) algernon (K. Algernon A. Sheppard). All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace EnlightenYourMouse
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;
    using AlgernonCommons;
    using HarmonyLib;
    using UnityEngine;

    /// <summary>
    /// Harmony patches to replace game mouse light code.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = "Harmony")]
    [HarmonyPatch(typeof(DefaultTool), nameof(DefaultTool.RenderGeometry))]
    public static class DefaultToolPatches
    {
        /// <summary>
        /// Harmony pre-emptive prefix for DefaultTool.EnableMouseLight to always disable default game light.
        /// </summary>
        /// <param name="__result">Original method result.</param>
        /// <returns>Always <c>false</c> (never execute original method).</returns>
        [HarmonyPatch("EnableMouseLight")]
        public static bool Prefix(out bool __result)
        {
            __result = false;
            return false;
        }

        /// <summary>
        /// Harmony transpiler to replace game mouse light code in DefaultTool.RenderGeometry with a call to our custom method.
        /// </summary>
        /// <param name="instructions">Original ILCode.</param>
        /// <returns>Modified ILCode.</returns>
        [HarmonyPatch(nameof(DefaultTool.RenderGeometry))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> RenderGeometryTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            // Instruction parsing.
            IEnumerator<CodeInstruction> instructionsEnumerator = instructions.GetEnumerator();
            CodeInstruction instruction;
            MethodInfo drawLight = AccessTools.Method(typeof(LightSystem), nameof(LightSystem.DrawLight), new Type[] { typeof(LightType), typeof(Vector3), typeof(Vector3), typeof(Vector3), typeof(Color), typeof(float), typeof(float), typeof(float), typeof(float), typeof(bool) });

            // Flag to indicate when we're omitting game code.
            bool isSkipping = false;

            // Iterate through all instructions in original method.
            while (instructionsEnumerator.MoveNext())
            {
                // Get next instruction and add it to output.
                instruction = instructionsEnumerator.Current;

                if (instruction.opcode == OpCodes.Call)
                {
                    if (instruction.operand.ToString().Equals("RenderManager get_instance()"))
                    {
                        Logging.Message("found call to ", instruction.operand.ToString());
                        yield return new CodeInstruction(OpCodes.Ldarg_1);
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(DefaultTool), "m_accuratePosition"));
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(DefaultTool), "m_toolController"));
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MouseLight), nameof(MouseLight.CustomMouseLight)));

                        // Now, skip (omit) code from original method replaced by our custom call - everything from here until callVirt Void Drawlight, inclusive.
                        isSkipping = true;
                    }
                }

                // Are we omitting code?
                if (isSkipping)
                {
                    // Yes - see if we can find the call to DrawLight that is the last instruction skipped.
                    if (instruction.Calls(drawLight))
                    {
                        Logging.Message("found LightSystem.DrawLight; stopping skipping");

                        // Stop skipping after this instruction.
                        isSkipping = false;
                    }
                }
                else
                {
                    // Not skipping - add this instruction to output.
                    yield return instruction;
                }
            }
        }
    }
}
