// <copyright file="ModSettings.cs" company="algernon (K. Algernon A. Sheppard)">
// Copyright (c) algernon (K. Algernon A. Sheppard). All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace EnlightenYourMouse
{
    using System.IO;
    using System.Xml.Serialization;
    using AlgernonCommons.Keybinding;
    using AlgernonCommons.XML;
    using UnityEngine;

    /// <summary>
    /// Mod settings.
    /// </summary>
    [XmlRoot("EnlightenYourMouse")]
    public class ModSettings : SettingsXMLBase
    {
        // UUI hotkey.
        [XmlIgnore]
        private static readonly UnsavedInputKey UUIKey = new UnsavedInputKey(name: "Mouse light toggle", keyCode: KeyCode.M, control: false, shift: false, alt: true);

        // Settings file name.
        [XmlIgnore]
        private static readonly string SettingsFileName = "EYM-settings.xml";

        // User settings directory.
        [XmlIgnore]
        private static readonly string UserSettingsDir = ColossalFramework.IO.DataLocation.localApplicationData;

        // Full userdir settings file name.
        [XmlIgnore]
        private static readonly string SettingsFile = Path.Combine(UserSettingsDir, SettingsFileName);

        /// <summary>
        /// Gets or sets the mouse light intensity multiplier.
        /// </summary>
        public float XMLIntensity { get => MouseLight.IntensityMultiplier; set => MouseLight.IntensityMultiplier = value; }

        /// <summary>
        /// Gets or sets the mouse light range multiplier.
        /// </summary>
        public float XMLRange { get => MouseLight.RangeMultiplier; set => MouseLight.RangeMultiplier = value; }

        /// <summary>
        /// Gets or sets the mouse light custom color - red component.
        /// </summary>
        public float XMLRed { get => MouseLight.Red; set => MouseLight.Red = value; }

        /// <summary>
        /// Gets or sets the mouse light custom color - green component.
        /// </summary>
        public float XMLGreen { get => MouseLight.Green; set => MouseLight.Green = value; }

        /// <summary>
        /// Gets or sets the mouse light custom color - blue component.
        /// </summary>
        public float XMLBlue { get => MouseLight.Blue; set => MouseLight.Blue = value; }

        /// <summary>
        /// Gets or sets the toggle key.
        /// </summary>
        [XmlElement("ToggleKey")]
        public Keybinding XMLToggleKey
        {
            get => UUIKey.Keybinding;

            set => UUIKey.Keybinding = value;
        }

        /// <summary>
        /// Gets the current hotkey as a UUI UnsavedInputKey.
        /// </summary>
        [XmlIgnore]
        internal static UnsavedInputKey ToggleKey => UUIKey;

        /// <summary>
        /// Loads settings from file.
        /// </summary>
        internal static void Load()
        {
            // If no settings file in user settings directory, move any existing settings file from application directory to user settings directory.
            if (!File.Exists(SettingsFile) && File.Exists(SettingsFileName))
            {
                File.Move(SettingsFileName, SettingsFile);
            }

            XMLFileUtils.Load<ModSettings>(SettingsFile);
        }

        /// <summary>
        /// Saves settings to file.
        /// </summary>
        internal static void Save() => XMLFileUtils.Save<ModSettings>(SettingsFile);
    }
}
