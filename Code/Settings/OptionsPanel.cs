﻿// <copyright file="OptionsPanel.cs" company="algernon (K. Algernon A. Sheppard)">
// Copyright (c) algernon (K. Algernon A. Sheppard). All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace EnlightenYourMouse
{
    using System.Linq;
    using AlgernonCommons.Keybinding;
    using AlgernonCommons.Translation;
    using AlgernonCommons.UI;
    using ColossalFramework.UI;
    using UnityEngine;

    /// <summary>
    /// Garbage Bin Manager options panel.
    /// </summary>
    public class OptionsPanel : OptionsPanelBase
    {
        // Layout constants.
        private const float Margin = 5f;

        /// <summary>
        /// Performs on-demand panel setup.
        /// </summary>
        protected override void Setup()
        {
            // Add title
            UILabels.AddLabel(this, 0f, Margin, Translations.Translate("EYM_NAM"), OptionsPanelManager<OptionsPanel>.PanelWidth, 1.5f, UIHorizontalAlignment.Center);
            float currentY = 30f;

            // Language dropdown.
            UISpacers.AddOptionsSpacer(this, Margin, currentY, OptionsPanelManager<OptionsPanel>.PanelWidth - (Margin * 2f));
            currentY += 15f;
            UIDropDown translationDropDown = UIDropDowns.AddPlainDropDown(this, Margin, currentY, Translations.Translate("LANGUAGE_CHOICE"), Translations.LanguageList, Translations.Index);

            // Event handler.
            translationDropDown.eventSelectedIndexChanged += (control, index) =>
            {
                Translations.Index = index;
                OptionsPanelManager<OptionsPanel>.LocaleChanged();
            };

            currentY += translationDropDown.parent.height + Margin;
            UISpacers.AddOptionsSpacer(this, Margin, currentY, OptionsPanelManager<OptionsPanel>.PanelWidth - (Margin * 2f));
            currentY += 15f;

            // Effect sliders.
            UISlider intensitySlider = UISliders.AddPlainSliderWithValue(this, Margin, currentY, Translations.Translate("EYM_OPT_INT"), 1f, 3f, 0.1f, MouseLight.IntensityMultiplier);
            intensitySlider.eventValueChanged += (c, value) => MouseLight.IntensityMultiplier = value;
            currentY += intensitySlider.parent.height;

            UISlider rangeSlider = UISliders.AddPlainSliderWithValue(this, Margin, currentY, Translations.Translate("EYM_OPT_RNG"), 1f, 16f, 0.5f, MouseLight.RangeMultiplier);
            rangeSlider.eventValueChanged += (c, value) => MouseLight.RangeMultiplier = value;
            currentY += rangeSlider.parent.height;

            UISpacers.AddOptionsSpacer(this, Margin, currentY, OptionsPanelManager<OptionsPanel>.PanelWidth - (Margin * 2f));
            currentY += 15f;

            // Color sliders.
            UILabel colorLabel = UILabels.AddLabel(this, Margin, currentY, Translations.Translate("EYM_OPT_COL"), textScale: 1.3f);
            colorLabel.font = Resources.FindObjectsOfTypeAll<UIFont>().FirstOrDefault((UIFont f) => f.name == "OpenSans-Semibold");
            currentY += colorLabel.height + Margin;

            UISlider redSlider = UISliders.AddPlainSliderWithValue(this, Margin, currentY, Translations.Translate("EYM_OPT_RED"), 0f, 1f, 0.01f, MouseLight.Red);
            redSlider.eventValueChanged += (c, value) => MouseLight.Red = value;
            currentY += redSlider.parent.height;

            UISlider greenSlider = UISliders.AddPlainSliderWithValue(this, Margin, currentY, Translations.Translate("EYM_OPT_GRN"), 0f, 1f, 0.01f, MouseLight.Green);
            greenSlider.eventValueChanged += (c, value) => MouseLight.Green = value;
            currentY += greenSlider.parent.height;

            UISlider blueSlider = UISliders.AddPlainSliderWithValue(this, Margin, currentY, Translations.Translate("EYM_OPT_BLU"), 0f, 1f, 0.01f, MouseLight.Blue);
            blueSlider.eventValueChanged += (c, value) => MouseLight.Blue = value;
            currentY += blueSlider.parent.height;

            // Revert to defaults button.
            UIButton defaultsButton = UIButtons.AddButton(this, Margin, currentY, Translations.Translate("EYM_OPT_DEF"), width: 300f, scale: 1.1f);
            currentY += defaultsButton.height;

            defaultsButton.eventClicked += (control, clickEvent) =>
            {
                intensitySlider.value = MouseLight.DefaultIntensity;
                rangeSlider.value = MouseLight.DefaultRange;
                redSlider.value = MouseLight.DefaultRed;
                greenSlider.value = MouseLight.DefaultGreen;
                blueSlider.value = MouseLight.DefaultBlue;
            };

            currentY += 30f;
            UISpacers.AddOptionsSpacer(this, Margin, currentY, OptionsPanelManager<OptionsPanel>.PanelWidth - (Margin * 2f));
            currentY += 15f;

            // Hotkey control.
            OptionsKeymapping anarchyKeyMapping = OptionsKeymapping.AddKeymapping(this, Margin, currentY, Translations.Translate("KEY_TOGGLE"), ModSettings.ToggleKey.Keybinding);
        }
    }
}