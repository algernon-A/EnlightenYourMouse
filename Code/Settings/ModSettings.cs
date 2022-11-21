// <copyright file="ModSettings.cs" company="algernon (K. Algernon A. Sheppard)">
// Copyright (c) algernon (K. Algernon A. Sheppard). All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace EnlightenYourMouse
{
    using System.IO;
    using System.Xml.Serialization;
    using AlgernonCommons.XML;

    /// <summary>
    /// Mod settings.
    /// </summary>
    [XmlRoot("EnlightenYourMouse")]
    public class ModSettings : SettingsXMLBase
    {
        // Settings file name.
        [XmlIgnore]
        internal static readonly string SettingsFileName = "EYM-settings.xml";

        // User settings directory.
        [XmlIgnore]
        private static readonly string UserSettingsDir = ColossalFramework.IO.DataLocation.localApplicationData;

        // Full userdir settings file name.
        [XmlIgnore]
        private static readonly string SettingsFile = Path.Combine(UserSettingsDir, SettingsFileName);

        /// <summary>
        /// Gets or sets the mouse light intensity multiplier.
        /// </summary>
        public float XMLIntensity { get => MouseLight.intensityMultiplier; set => MouseLight.intensityMultiplier = value; }

        /// <summary>
        /// Gets or sets the mouse light range multiplier.
        /// </summary>
        public float XMLRange { get => MouseLight.rangeMultiplier; set => MouseLight.rangeMultiplier = value; }

        /// <summary>
        /// Gets or sets the mouse light custom color - red component.
        /// </summary>
        public float XMLRed { get => MouseLight.Red; set => MouseLight.Red = value; }

        /// <summary>
        /// Gets or sets the mouse light custom color - red component.
        /// </summary>
        public float XMLGreen { get => MouseLight.Green; set => MouseLight.Green = value; }

        /// <summary>
        /// Gets or sets the mouse light custom color - red component.
        /// </summary>
        public float XMLBlue { get => MouseLight.Blue; set => MouseLight.Blue = value; }

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
