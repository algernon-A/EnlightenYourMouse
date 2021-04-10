using ICities;
using ColossalFramework.UI;
using CitiesHarmony.API;
using AlgernonUtils;
using AlgernonTranslation;


namespace EnlightenYourMouse
{
    /// <summary>
    /// The base mod class for instantiation by the game.
    /// </summary>
    public class EnlightenYourMouseMod : IUserMod
    {
        public string Name => ModName;
        public string Description => Translations.Translate("EYM_DESC");

        internal static string ModName => "EYM: Enlighten Your Mouse";


        /// <summary>
        /// Called by the game when the mod is enabled.
        /// </summary>
        public void OnEnabled()
        {
            // Apply Harmony patches via Cities Harmony.
            // Called here instead of OnCreated to allow the auto-downloader to do its work prior to launch.
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
            
            // Load the settings file.
            SettingsUtils.LoadSettings();
        }


        /// <summary>
        /// Called by the game when the mod is disabled.
        /// </summary>
        public void OnDisabled()
        {
            // Unapply Harmony patches via Cities Harmony.
            if (HarmonyHelper.IsHarmonyInstalled)
            {
                Patcher.UnpatchAll();
            }
        }


        /// <summary>
        /// Called by the game when the mod options panel is setup.
        /// </summary>
        public void OnSettingsUI(UIHelperBase helper)
        {
            // Language drop down.
            UIDropDown languageDropDown = (UIDropDown)helper.AddDropdown(Translations.Translate("TRN_CHOICE"), Translations.LanguageList, Translations.Index, (index) => { Translations.Index = index; SettingsUtils.SaveSettings(); });
            languageDropDown.autoSize = false;
            languageDropDown.width = 270f;

            helper.AddSlider(Translations.Translate("EYM_OPT_INT"), 1f, 3f, 0.1f, MouseLight.intensityMultiplier, (value) => { MouseLight.intensityMultiplier = value; SettingsUtils.SaveSettings(); } );
            helper.AddSlider(Translations.Translate("EYM_OPT_RNG"), 1f, 16f, 0.5f, MouseLight.rangeMultiplier, (value) => { MouseLight.rangeMultiplier = value; SettingsUtils.SaveSettings(); } );
            helper.AddSlider(Translations.Translate("EYM_OPT_RED"), 0f, 1f, 0.01f, MouseLight.Red, (value) => { MouseLight.Red = value; SettingsUtils.SaveSettings(); } );
            helper.AddSlider(Translations.Translate("EYM_OPT_GRN"), 0f, 1f, 0.01f, MouseLight.Green, (value) => { MouseLight.Green = value; SettingsUtils.SaveSettings(); } );
            helper.AddSlider(Translations.Translate("EYM_OPT_BLU"), 0f, 1f, 0.01f, MouseLight.Blue, (value) => { MouseLight.Blue = value; SettingsUtils.SaveSettings(); } );
        }
    }
}
