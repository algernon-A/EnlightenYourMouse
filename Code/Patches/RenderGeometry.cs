using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
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
}
