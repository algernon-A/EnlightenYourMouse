using System.Xml.Serialization;
using AlgernonTranslation;


namespace EnlightenYourMouse
{
    [XmlRoot("EnlightenYourMouse")]
    public class EYMSettingsFile
    {
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
        public float Intensity { get => MouseLight.intensityMultiplier; set => MouseLight.intensityMultiplier = value; }


        /// <summary>
        /// Mouse light range multiplier.
        /// </summary>
        public float Range { get => MouseLight.rangeMultiplier; set => MouseLight.rangeMultiplier = value; }


        /// <summary>
        /// Mouse light custom color - red component.
        /// </summary>
        public float Red { get => MouseLight.Red; set => MouseLight.Red = value; }


        /// <summary>
        /// Mouse light custom color - red component.
        /// </summary>
        public float Green { get => MouseLight.Green; set => MouseLight.Green = value; }


        /// <summary>
        /// Mouse light custom color - red component.
        /// </summary>
        public float Blue { get => MouseLight.Blue; set => MouseLight.Blue = value; }
    }
}
