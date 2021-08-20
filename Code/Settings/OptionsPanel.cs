﻿using System;
using UnityEngine;
using ColossalFramework.UI;
using ColossalFramework.Globalization;
using AlgernonUtils;


namespace EnlightenYourMouse
{
    /// <summary>
    /// Class to handle the mod settings options panel.
    /// </summary>
    internal static class OptionsPanel
    {
        // Parent UI panel reference.
        internal static UIScrollablePanel optionsPanel;
        private static UIPanel gameOptionsPanel;
        private static EYMOptionsPanel _panel;
        internal static EYMOptionsPanel Panel => _panel;

        // Instance reference.
        private static GameObject optionsGameObject;
        internal static bool IsOpen => optionsGameObject != null;


        /// <summary>
        /// Attaches an event hook to options panel visibility, to create/destroy our options panel as appropriate.
        /// Destroying when not visible saves UI overhead and performance impacts, especially with so many UITextFields.
        /// </summary>
        internal static void OptionsEventHook()
        {
            // Get options panel instance.
            gameOptionsPanel = UIView.library.Get<UIPanel>("OptionsPanel");

            if (gameOptionsPanel == null)
            {
                Logging.Error("couldn't find OptionsPanel");
            }
            else
            {
                // Simple event hook to create/destroy GameObject based on appropriate visibility.
                gameOptionsPanel.eventVisibilityChanged += (control, isVisible) =>
                {
                    // Create/destroy based on whether or not we're now visible.
                    if (isVisible)
                    {
                        Create();
                    }
                    else
                    {
                        Close();
                    }
                };

                // Recreate panel on system locale change.
                LocaleManager.eventLocaleChanged += LocaleChanged;
            }
        }


        /// <summary>
        /// Refreshes the options panel (destroys and rebuilds) on a locale change when the options panel is open.
        /// </summary>
        private static void LocaleChanged()
        {
            if (gameOptionsPanel != null && gameOptionsPanel.isVisible)
            {
                Close();
                Create();
            }
        }


        /// <summary>
        /// Creates the panel object in-game and displays it.
        /// </summary>
        private static void Create()
        {
            try
            {
                // We're now visible - create our gameobject, and give it a unique name for easy finding with ModTools.
                optionsGameObject = new GameObject("EYMOptionsPanel");

                // Attach to game options panel.
                optionsGameObject.transform.parent = optionsPanel.transform;

                _panel = optionsGameObject.AddComponent<EYMOptionsPanel>();

                // Set up and show panel.
                Panel.Setup(optionsPanel.width, optionsPanel.height);
            }
            catch (Exception e)
            {
                Logging.LogException(e, "exception creating options panel");
            }
        }


        /// <summary>
        /// Closes the panel by destroying the object (removing any ongoing UI overhead).
        /// </summary>
        private static void Close()
        {
            // Save settings first.
            SettingsUtils.SaveSettings();

            // Enforce C# garbage collection by setting to null.
            if (_panel != null)
            {
                GameObject.Destroy(_panel);
                _panel = null;
            }

            // We're no longer visible - destroy our game object.
            if (optionsGameObject != null)
            {
                GameObject.Destroy(optionsGameObject);
                optionsGameObject = null;
            }
        }
    }
}