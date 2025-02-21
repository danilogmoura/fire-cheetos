using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Il2CppCodeStage.AntiCheat.Detectors;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace FireCheetos
{
    public static class BuildInfo
    {
        public const string Name = "Fire Cheetos";
        public const string Description = "";
        public const string Author = "danilogmoura";
        public const string Company = null;
        public const string Version = "1.0.0";
    }

    public class Main : MelonMod
    {
        public static bool feature1Enabled;
        public static bool feature2Enabled;
        public static bool feature3Enabled;
        public static bool feature4Enabled;
        public static bool feature5Enabled;
        public static bool feature6Enabled;

        private List<Button> buttonsUpgrade;
        private object clickCoroutine;

        private bool showMenu;

        public override void OnLateInitializeMelon()
        {
            InjectionDetector.StopDetection();
            ObscuredCheatingDetector.StopDetection();
            SpeedHackDetector.StopDetection();
            TimeCheatingDetector.StopDetection();
            WallHackDetector.StopDetection();

            var harmony = new HarmonyLib.Harmony("br.com.danilogmoura.ieh");
            harmony.PatchAll();
            MelonDebug.Msg("Fire Cheetos Initialized!");
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F1)) showMenu = !showMenu;
        }

        private void PressButton(string buttonName)
        {
            var buttonObject = GameObject.Find(buttonName);
            if (buttonObject != null)
            {
                var button = buttonObject.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.Invoke();
                    MelonDebug.Msg($"Button {buttonName} pressed!");
                }
                else
                {
                    MelonDebug.Error($"The object {buttonName} does not have a Button component!");
                }
            }
            else
            {
                MelonDebug.Error($"Button {buttonName} not found!");
            }
        }

        private void StartClicking()
        {
            clickCoroutine = MelonCoroutines.Start(ClickLoop());
            MelonDebug.Msg("Auto-click started!");
        }

        private void StopClicking()
        {
            if (clickCoroutine != null) MelonCoroutines.Stop(clickCoroutine);
            buttonsUpgrade = null;
            MelonDebug.Msg("Auto-click stopped!");
        }

        private IEnumerator ClickLoop()
        {
            while (feature2Enabled)
            {
                yield return new WaitForSeconds(0.5f);
                ClickAllUpgradeButtons();
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void ClickAllUpgradeButtons()
        {
            var root = GameObject.Find("menusRoot/menuCanvasParent/SafeArea/menuCanvas/menus");
            if (root == null)
            {
                MelonDebug.Error("menusRoot not found!");
                return;
            }

            if (buttonsUpgrade == null)
            {
                buttonsUpgrade = new List<Button>();

                var buttons = root.GetComponentsInChildren<Button>();

                foreach (var button in buttons)
                    if (button.gameObject.name == "lvlUpButton" || button.gameObject.name == "buyButton")
                    {
                        buttonsUpgrade.Add(button);
                        button.onClick.Invoke(); // Simula um clique no botão
                        MelonDebug.Msg($"Button {button.gameObject.name} clicked!");
                    }
            }

            buttonsUpgrade.Where(button => button.interactable)
                .ToList()
                .ForEach(button => button.onClick.Invoke());
        }

        private void ToggleSpeedHack()
        {
            try
            {
                SpeedHackDetector.StopDetection();
            }
            catch (Exception e)
            {
                MelonDebug.Error("Error desabling the SpeedHackDetector: " + e);
            }

            feature5Enabled = !feature5Enabled;
            Time.timeScale = feature5Enabled ? 3f : 1f;
            MelonDebug.Msg($"Speed Hack: {feature5Enabled}");
        }

        public override void OnGUI()
        {
            if (!showMenu) return;

            const float menuX = 180;
            const float menuY = 80;
            const float width = 200;
            const float height = 220;

            const float buttonX = menuX + 10;
            const float buttonWidth = 180;
            const float buttonHeight = 30;

            var menuRect = new Rect(menuX, menuY, width, height);
            GUI.Box(menuRect, "");

            if (GUI.Button(new Rect(buttonX, menuY + 10, buttonWidth, buttonHeight),
                    feature1Enabled ? "Auto Attack: ON" : "Auto Attack: OFF"))
            {
                feature1Enabled = !feature1Enabled;
                MelonDebug.Msg($"Auto Attack: {feature1Enabled}");
            }

            if (GUI.Button(new Rect(buttonX, menuY + 45, buttonWidth, buttonHeight),
                    feature2Enabled ? "Auto Upgrade: ON" : "Auto Upgrade: OFF"))
            {
                feature2Enabled = !feature2Enabled;
                if (feature2Enabled)
                {
                    StartClicking();
                    PressButton("upgradesButton");
                }
                else
                {
                    PressButton("upgradesButton");
                    StopClicking();
                }

                MelonDebug.Msg($"Auto Upgrade: {feature2Enabled}");
            }

            if (GUI.Button(new Rect(buttonX, menuY + 80, buttonWidth, buttonHeight),
                    feature4Enabled ? "Epic Prestige: ON" : "Epic Prestige: OFF"))
            {
                feature4Enabled = !feature4Enabled;
                MelonDebug.Msg($"Epic Prestige: {feature4Enabled}");
            }

            if (GUI.Button(new Rect(buttonX, menuY + 115, buttonWidth, buttonHeight),
                    feature6Enabled ? "XP Booster: ON" : "XP Booster: OFF"))
            {
                feature6Enabled = !feature6Enabled;
                MelonDebug.Msg($"XP Booster: {feature6Enabled}");
            }

            if (GUI.Button(new Rect(buttonX, menuY + 150, buttonWidth, buttonHeight),
                    feature5Enabled ? "Speed Hack: ON" : "Speed Hack: OFF"))
                ToggleSpeedHack();

            if (GUI.Button(new Rect(buttonX, menuY + 185, buttonWidth, buttonHeight),
                    feature3Enabled ? "Weakened Enemies: ON" : "Weakened Enemies: OFF"))
            {
                feature3Enabled = !feature3Enabled;
                MelonDebug.Msg($"Weakened Enemies: {feature3Enabled}");
            }
        }
    }
}