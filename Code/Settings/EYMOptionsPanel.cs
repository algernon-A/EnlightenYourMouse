using System.Linq;
using UnityEngine;
using ColossalFramework.UI;
using AlgernonUtils;
using AlgernonTranslation;


namespace EnlightenYourMouse
{
    /// <summary>
    /// Garbage Bin Manager options panel.
    /// </summary>
    public class EYMOptionsPanel : UIPanel
    {
        // Layout constants.
        private const float Margin = 5f;


        /// <summary>
        /// Performs initial setup for the panel; we don't use Start() as that's not sufficiently reliable (race conditions), and is not needed with the dynamic create/destroy process.
        /// </summary>
        internal void Setup(float width, float height)
        {
            // Size and placement.
            this.autoSize = false;
            this.autoLayout = false;
            this.relativePosition = new Vector2(Margin, Margin);
            this.width = width - (Margin * 2f);
            this.height = height - (Margin * 2f);

            // Add title
            float currentY = TitleLabel(this, "EYM_NAM");

            // Add sliders.
            UISlider intensitySlider = UIControls.AddSliderWithValue(this, Translations.Translate("EYM_OPT_INT"), 1f, 3f, 0.1f, MouseLight.intensityMultiplier, (value) => MouseLight.intensityMultiplier = value);
            UISlider rangeSlider = UIControls.AddSliderWithValue(this, Translations.Translate("EYM_OPT_RNG"), 1f, 16f, 0.5f, MouseLight.rangeMultiplier, (value) => MouseLight.rangeMultiplier = value);
            UISlider redSlider = UIControls.AddSliderWithValue(this, Translations.Translate("EYM_OPT_RED"), 0f, 1f, 0.01f, MouseLight.Red, (value) => MouseLight.Red = value);
            UISlider greenSlider = UIControls.AddSliderWithValue(this, Translations.Translate("EYM_OPT_GRN"), 0f, 1f, 0.01f, MouseLight.Green, (value) => MouseLight.Green = value);
            UISlider blueSlider = UIControls.AddSliderWithValue(this, Translations.Translate("EYM_OPT_BLU"), 0f, 1f, 0.01f, MouseLight.Blue, (value) => MouseLight.Blue = value);

            // Set slider positions.
            currentY = SliderPosition(rangeSlider, currentY);
            currentY = SliderPosition(intensitySlider, currentY);

            UILabel colorLabel = UIControls.AddLabel(this, Margin, currentY, Translations.Translate("EYM_OPT_COL"), textScale: 1.3f);
            colorLabel.font = Resources.FindObjectsOfTypeAll<UIFont>().FirstOrDefault((UIFont f) => f.name == "OpenSans-Semibold");
            currentY += colorLabel.height + Margin;

            currentY = SliderPosition(redSlider, currentY);
            currentY = SliderPosition(greenSlider, currentY);
            currentY = SliderPosition(blueSlider, currentY);

            // Revert to defaults button.
            UIButton defaultsButton = UIControls.AddButton(this, Margin, currentY, Translations.Translate("EYM_OPT_DEF"), width = 300f);

            defaultsButton.eventClicked += (control, clickEvent) =>
            {
                intensitySlider.value = MouseLight.DefaultIntensity;
                rangeSlider.value = MouseLight.DefaultRange;
                redSlider.value = MouseLight.DefaultRed;
                greenSlider.value = MouseLight.DefaultGreen;
                blueSlider.value = MouseLight.DefaultBlue;
            };

            // Language dropdown.
            currentY += defaultsButton.height + (Margin * 2f);
            UIControls.OptionsSpacer(this, Margin, currentY, this.width - (Margin * 2f));
            currentY += 15f;
            UIDropDown translationDropDown = UIControls.AddPlainDropDown(this, Translations.Translate("TRN_CHOICE"), Translations.LanguageList, Translations.Index);
            translationDropDown.parent.relativePosition = new Vector2(Margin, currentY);

            // Event handler.
            translationDropDown.eventSelectedIndexChanged += (control, index) =>
            {
                Translations.Index = index;
                OptionsPanel.LocaleChanged();
            };
        }


        /// <summary>
        /// Sets the position of the specified slider.
        /// </summary>
        /// <param name="slider">Slider to position</param>
        /// <param name="yPos">Relative Y position at top of slider parent</param>
        /// <returns>New  relative Y position underneath slider parent</returns>
        private float SliderPosition(UISlider slider, float yPos)
        {
            slider.parent.relativePosition = new Vector2(Margin, yPos);
            return yPos + slider.parent.height;
        }


        /// <summary>
        /// Adds a title label across the top of the specified UIComponent.
        /// </summary>
        /// <param name="parent">Parent component</param>
        /// <param name="titleKey">Title translation key</param>
        /// <returns>Y position below title</returns>
        private float TitleLabel(UIComponent parent, string titleKey)
        {
            // Add title.
            UILabel titleLabel = UIControls.AddLabel(parent, 0f, Margin, Translations.Translate(titleKey), parent.width, 1.5f);
            titleLabel.textAlignment = UIHorizontalAlignment.Center;
            titleLabel.font = Resources.FindObjectsOfTypeAll<UIFont>().FirstOrDefault((UIFont f) => f.name == "OpenSans-Semibold");

            UIControls.OptionsSpacer(parent, Margin, titleLabel.height + (Margin * 2f), parent.width - (Margin * 2f));

            return Margin + titleLabel.height + Margin + 5f + Margin;
        }
    }
}