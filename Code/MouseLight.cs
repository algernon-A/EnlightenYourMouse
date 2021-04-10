using UnityEngine;
using ColossalFramework;


namespace EnlightenYourMouse
{
    /// <summary>
    /// Class to manage custom mouse cursor lighting.
    /// </summary>
    public static class MouseLight
    {
        // Mouse lighting parameters.
        public static float intensityMultiplier = 1f;
        public static float rangeMultiplier = 4f;
        private static float red = 1f;
        private static float green = 1f;
        private static float blue = 1f;


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
            // Based on game code.
            LightSystem lightSystem = Singleton<RenderManager>.instance.lightSystem;
            Vector3 a = m_accuratePosition - cameraInfo.m_position;
            float magnitude = a.magnitude;
            float range = Mathf.Sqrt(magnitude);

            // Multiply game intensity with our custom multiplier.
            float intensity = m_toolController.m_MouseLightIntensity.value * intensityMultiplier;

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