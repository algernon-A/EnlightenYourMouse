// <copyright file="ToolRayCast.cs" company="algernon (K. Algernon A. Sheppard)">
// Copyright (c) algernon (K. Algernon A. Sheppard). All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace EnlightenYourMouse
{
    /// <summary>
    /// Simple class to access ToolBase.RayCast protected static method.
    /// </summary>
    public class ToolRayCast : ToolBase
    {
        /// <summary>
        /// Access ToolBase.RayCast protected method.
        /// </summary>
        /// <param name="input">Input raycast.</param>
        /// <param name="output">Output raycast.</param>
        /// <returns>Raycast result.</returns>
        public static bool BaseRayCast(RaycastInput input, out RaycastOutput output) => RayCast(input, out output);
    }
}
