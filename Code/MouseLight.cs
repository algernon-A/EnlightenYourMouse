// <copyright file="MouseLight.cs" company="algernon (K. Algernon A. Sheppard)">
// Copyright (c) algernon (K. Algernon A. Sheppard). All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace EnlightenYourMouse
{
    using AlgernonCommons.Keybinding;
    using ColossalFramework;
    using UnifiedUI.Helpers;
    using UnityEngine;

    /// <summary>
    /// Class to manage custom mouse cursor lighting.
    /// </summary>
    public static class MouseLight
    {
        /// <summary>
        /// Default mouse light intensity.
        /// </summary>
        public const float DefaultIntensity = 1.5f;

        /// <summary>
        /// Default mouse light range.
        /// </summary>
        public const float DefaultRange = 8f;

        /// <summary>
        /// Default mouse color - red component.
        /// </summary>
        public const float DefaultRed = 1f;

        /// <summary>
        /// Default mouse color - green component.
        /// </summary>
        public const float DefaultGreen = 1f;

        /// <summary>
        /// Default mouse color - blue component.
        /// </summary>
        public const float DefaultBlue = 1f;

        // Mouse lighting parameters.
        private static float s_intensityMultiplier = DefaultIntensity;
        private static float s_rangeMultiplier = DefaultRange;
        private static float s_red = DefaultRed;
        private static float s_green = DefaultGreen;
        private static float s_blue = DefaultBlue;
        private static float s_toolIntensity = 1f;

        // Light toggle.
        private static bool s_enabled = true;

        /// <summary>
        /// Gets or sets the mouse light intensity multiplier.
        /// </summary>
        public static float IntensityMultiplier { get => s_intensityMultiplier; set => s_intensityMultiplier = value; }

        /// <summary>
        /// Gets or sets the mouse light range multiplier.
        /// </summary>
        public static float RangeMultiplier { get => s_rangeMultiplier; set => s_rangeMultiplier = value; }

        /// <summary>
        /// Gets or sets the mouse light custom color - red component.
        /// </summary>
        public static float Red
        {
            get => s_red;

            set => s_red = Mathf.Clamp(value, 0f, 1f);
        }

        /// <summary>
        /// Gets or sets the mouse light custom color - green component.
        /// </summary>
        public static float Green
        {
            get => s_green;

            set => s_green = Mathf.Clamp(value, 0f, 1f);
        }

        /// <summary>
        /// Gets or sets the mouse light custom color - blue component.
        /// </summary>
        public static float Blue
        {
            get => s_blue;

            set => s_blue = Mathf.Clamp(value, 0f, 1f);
        }

        /// <summary>
        /// Custom method to replace game code for drawing the mouse cursor light.
        /// </summary>
        /// <param name="cameraInfo">Current camera.</param>
        /// <param name="m_accuratePosition">Current tool accurate position.</param>
        /// <param name="m_toolController">Current tool controller reference.</param>
        internal static void CustomMouseLight(RenderManager.CameraInfo cameraInfo, Vector3 m_accuratePosition, ToolController m_toolController)
        {
            // Extract intensity from current tool controller and record it.
            s_toolIntensity = m_toolController.m_MouseLightIntensity.value;

            // Render light.
            DrawMouseLight(cameraInfo, m_accuratePosition);
        }

        /// <summary>
        /// Custom method to replace game code for drawing the mouse cursor light.
        /// </summary>
        /// <param name="cameraInfo">Current camera.</param>
        /// <param name="m_accuratePosition">Current tool accurate position.</param>
        internal static void DrawMouseLight(RenderManager.CameraInfo cameraInfo, Vector3 m_accuratePosition)
        {
            // Don't do anything if not enabled.
            if (!s_enabled)
            {
                return;
            }

            // Based on game code.
            LightSystem lightSystem = Singleton<RenderManager>.instance.lightSystem;
            Vector3 a = m_accuratePosition - cameraInfo.m_position;
            float magnitude = a.magnitude;
            float range = Mathf.Sqrt(magnitude);

            // Multiply game intensity with our custom multiplier.
            float intensity = s_toolIntensity * s_intensityMultiplier;

            // Ditto for range.
            range *= 1f + (intensity * s_rangeMultiplier);

            // Back to game code.
            intensity += intensity * intensity * intensity * 2f;
            intensity *= MathUtils.SmoothStep(0.9f, 0.1f, lightSystem.DayLightIntensity);
            Vector3 direction = a * (1f / Mathf.Max(1f, magnitude));
            Vector3 position = m_accuratePosition - (direction * (range * 0.2f));
            if (intensity > 0.001f)
            {
                // Replace game color (Color.white) with our own custom color.
                lightSystem.DrawLight(LightType.Spot, position, direction, Vector3.zero, new Color(s_red, s_green, s_blue), intensity, range, 90f, 1f, volume: false);
            }
        }

        /// <summary>
        /// Adds the UUI button.
        /// </summary>
        internal static void AddUUIButton()
        {
            UUICustomButton uuiButton = UUIHelpers.RegisterCustomButton(
                name: (Mod.Instance as Mod)?.Name,
                groupName: null, // default group
                tooltip: (Mod.Instance as Mod)?.Name,
                icon: UUIHelpers.LoadTexture(UUIHelpers.GetFullPath<Mod>("Resources", "EYM-UUI.png")),
                onToggle: (value) => s_enabled = value,
                hotkeys: new UUIHotKeys { ActivationKey = ModSettings.ToggleKey });

            // Set initial state.
            uuiButton.IsPressed = s_enabled;
        }
    }
}