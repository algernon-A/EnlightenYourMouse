// <copyright file="ModSettings.cs" company="algernon (K. Algernon A. Sheppard)">
// Copyright (c) algernon (K. Algernon A. Sheppard). All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace EnlightenYourMouse
{
    using System;
    using System.IO;
    using System.Xml.Serialization;
    using AlgernonCommons;
    using AlgernonTranslation;

    [XmlRoot("EnlightenYourMouse")]
    public class ModSettings
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

        // Language.
        [XmlElement("Language")]
        public string Language
        {
            get => Translations.Language;

            set => Translations.Language = value;
        }

        // Custom mouse light settings.

        /// <summary>
        /// Mouse light intensity multiplier.
        /// </summary>
        public float XMLIntensity { get => MouseLight.intensityMultiplier; set => MouseLight.intensityMultiplier = value; }


        /// <summary>
        /// Mouse light range multiplier.
        /// </summary>
        public float XMLRange { get => MouseLight.rangeMultiplier; set => MouseLight.rangeMultiplier = value; }


        /// <summary>
        /// Mouse light custom color - red component.
        /// </summary>
        public float XMLRed { get => MouseLight.Red; set => MouseLight.Red = value; }


        /// <summary>
        /// Mouse light custom color - red component.
        /// </summary>
        public float XMLGreen { get => MouseLight.Green; set => MouseLight.Green = value; }


        /// <summary>
        /// Mouse light custom color - red component.
        /// </summary>
        public float XMLBlue { get => MouseLight.Blue; set => MouseLight.Blue = value; }


        /// <summary>
        /// Load settings from XML file.
        /// </summary>
        internal static void Load()
        {
            try
            {
                // Attempt to read new settings file (in user settings directory).
                string fileName = SettingsFile;
                if (!File.Exists(fileName))
                {
                    // No settings file in user directory; use application directory instead.
                    fileName = SettingsFileName;

                    if (!File.Exists(fileName))
                    {
                        Logging.Message("no settings file found");
                        return;
                    }
                }

                // Read settings file.
                using (StreamReader reader = new StreamReader(fileName))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(ModSettings));
                    if (!(xmlSerializer.Deserialize(reader) is ModSettings settingsFile))
                    {
                        Logging.Error("couldn't deserialize settings file");
                    }
                }
            }
            catch (Exception e)
            {
                Logging.LogException(e, "exception reading XML settings file");
            }
        }


        /// <summary>
        /// Save settings to XML file.
        /// </summary>
        internal static void Save()
        {
            try
            {
                // Pretty straightforward..
                using (StreamWriter writer = new StreamWriter(SettingsFile))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(ModSettings));
                    xmlSerializer.Serialize(writer, new ModSettings());
                }

                // Cleaning up after ourselves - delete any old config file in the application direcotry.
                if (File.Exists(SettingsFileName))
                {
                    File.Delete(SettingsFileName);
                }
            }
            catch (Exception e)
            {
                Logging.LogException(e, "exception saving XML settings file");
            }
        }
    }
}
