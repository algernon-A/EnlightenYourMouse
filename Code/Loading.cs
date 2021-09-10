using ICities;


namespace EnlightenYourMouse
{
    /// <summary>
    /// Main loading class: the mod runs from here.
    /// </summary>
    public class Loading : LoadingExtensionBase
    {
        /// <summary>
        /// Called by the game when level loading is complete.
        /// </summary>
        /// <param name="mode">Loading mode (e.g. game, editor, scenario, etc.)</param>
        public override void OnLevelLoaded(LoadMode mode)
        {
            // Apply Harmony patches.
            Patcher.PatchAll();

            // Set up options panel event handler (need to redo this now that options panel has been reset after loading into game).
            OptionsPanel.OptionsEventHook();
        }
    }
}