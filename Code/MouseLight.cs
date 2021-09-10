using UnityEngine;
using ColossalFramework;


namespace EnlightenYourMouse
{
    /// <summary>
    /// Class to manage custom mouse cursor lighting.
    /// </summary>
    public static class MouseLight
    {
        // Default values.
        public const float DefaultIntensity = 1.5f;
        public const float DefaultRange = 8f;
        public const float DefaultRed = 1f;
        public const float DefaultGreen = 1f;
        public const float DefaultBlue = 1f;

        // Mouse lighting parameters.
        public static float intensityMultiplier = DefaultIntensity;
        public static float rangeMultiplier = DefaultRange;
        private static float red = DefaultRed;
        private static float green = DefaultGreen;
        private static float blue = DefaultBlue;
        private static float toolIntensity = 1f;


        /// <summary>
        /// Custom mouse light color - red component.
        /// </summary>
		public static float Red
        {
            get => red;

            set => red = Mathf.Clamp(value, 0f, 1f);
        }


        /// <summary>
        /// Custom mouse light color - green component.
        /// </summary>
		public static float Green
        {
            get => green;

            set => green = Mathf.Clamp(value, 0f, 1f);
        }


        /// <summary>
        /// Custom mouse light color - blue component.
        /// </summary>
        public static float Blue
        {
            get => blue;

            set => blue = Mathf.Clamp(value, 0f, 1f);
        }


        /// <summary>
        /// Custom method to replace game code for drawing the mouse cursor light.
        /// </summary>
        /// <param name="cameraInfo">Current camera</param>
        /// <param name="m_accuratePosition">Current tool accurate position</param>
        /// <param name="m_toolController">Current tool controller reference</param>
        public static void CustomMouseLight(RenderManager.CameraInfo cameraInfo, Vector3 m_accuratePosition, ToolController m_toolController)
        {
            // Extract intensity from current tool controller and record it.
            toolIntensity = m_toolController.m_MouseLightIntensity.value;

            // Render light.
            DrawMouseLight(cameraInfo, m_accuratePosition);
        }


        /// <summary>
        /// Custom method to replace game code for drawing the mouse cursor light.
        /// </summary>
        /// <param name="cameraInfo">Current camera</param>
        /// <param name="m_accuratePosition">Current tool accurate position</param>
        public static void DrawMouseLight(RenderManager.CameraInfo cameraInfo, Vector3 m_accuratePosition)
        {
            // Based on game code.
            LightSystem lightSystem = Singleton<RenderManager>.instance.lightSystem;
            Vector3 a = m_accuratePosition - cameraInfo.m_position;
            float magnitude = a.magnitude;
            float range = Mathf.Sqrt(magnitude);

            // Multiply game intensity with our custom multiplier.
            float intensity = toolIntensity * intensityMultiplier;

            // Ditto for range.
            range *= 1f + intensity * rangeMultiplier;

            // Back to game code.
            intensity += intensity * intensity * intensity * 2f;
            intensity *= MathUtils.SmoothStep(0.9f, 0.1f, lightSystem.DayLightIntensity);
            Vector3 direction = a * (1f / Mathf.Max(1f, magnitude));
            Vector3 position = m_accuratePosition - direction * (range * 0.2f);
            if (intensity > 0.001f)
            {
                // Replace game color (Color.white) with our own custom color.
                lightSystem.DrawLight(LightType.Spot, position, direction, Vector3.zero, new Color(red, green, blue), intensity, range, 90f, 1f, volume: false);
            }
        }
    }
}