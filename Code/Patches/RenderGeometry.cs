using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using ColossalFramework;
using HarmonyLib;
using AlgernonUtils;


namespace EnlightenYourMouse
{
	/// <summary>
	/// Harmony transpiler to replace game mouse light code with custom method.
	/// </summary>
	[HarmonyPatch(typeof(DefaultTool), nameof(DefaultTool.RenderGeometry))]
	public static class MouseLightPatch
	{
		/// <summary>
		/// Harmony transpiler to replace game mouse light code in DefaultTool.RenderGeometry with a call to our custom method.
		/// </summary>
		/// <param name="original">Original method reference</param>
		/// <param name="instructions">Original method ILCode</param>
		/// <param name="generator">ILCode generator</param>
		/// <returns>Patched ILCode</returns>
		public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions, ILGenerator generator)
		{
			// Instruction parsing.
			IEnumerator<CodeInstruction> instructionsEnumerator = instructions.GetEnumerator();
			CodeInstruction instruction;

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
					if (instruction.opcode == OpCodes.Callvirt && instruction.operand.ToString().StartsWith("Void DrawLight"))
					{
						Logging.Message("found Callvirt with operand ", instruction.operand.ToString(), "; stopping skipping");

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


	/// <summary>
	/// Harmony prefix to extend mouse lighting to selected tools that don't have it (NetTool, MoveItTool).
	/// </summary>
	[HarmonyPatch]
	public static class NewMouseLight
	{
		/// <summary>
		/// Determines the target method for the Prefix patch.
		/// </summary>
		/// <returns>RenderGeometry methods of NetTool and MoveItTool (if available)</returns>
		public static IEnumerable<MethodBase>TargetMethods()
        {
			// NetTool.
			yield return typeof(NetTool).GetMethod(nameof(NetTool.RenderGeometry));

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
		/// <param name="cameraInfo">Current camera reference</param>
		public static void Prefix(RenderManager.CameraInfo cameraInfo)
		{
			// If we're not in a special infomode, get raycast target point and apply mouse light.
			if (Singleton<InfoManager>.instance.CurrentMode == InfoManager.InfoMode.None && ToolRayCast.BaseRayCast(new ToolBase.RaycastInput(Camera.main.ScreenPointToRay(Input.mousePosition), Camera.main.farClipPlane), out ToolBase.RaycastOutput output))
			{
				MouseLight.DrawMouseLight(cameraInfo, output.m_hitPos);
			}
		}
	}


	/// <summary>
	/// Simple class to access ToolBase.RayCast protected static method.
	/// </summary>
	public class ToolRayCast : ToolBase
	{
		/// <summary>
		/// Access ToolBase.RayCast protected method.
		/// </summary>
		/// <param name="input">Input raycast</param>
		/// <param name="output">Output raycast</param>
		/// <returns></returns>
		public static bool BaseRayCast(RaycastInput input, out RaycastOutput output) => RayCast(input, out output);
	}
}
