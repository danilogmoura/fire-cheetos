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
        public const string DownloadLink = null;
    }

    public class Main : MelonMod
    {
        public static bool feature1Enabled;
        public static bool feature2Enabled;
        public static bool feature3Enabled;
        public static bool feature4Enabled;
        public static bool feature5Enabled;

        private List<Button> buttonsUpgrade;
        private object clickCoroutine; // Armazena a corrotina em execução

        private bool showMenu; // Variável para ativar/desativar o menu

        public override void OnInitializeMelon()
        {
            SpeedHackDetector.StopDetection();

            var harmony = new HarmonyLib.Harmony("br.com.danilogmoura.ieh");
            harmony.PatchAll();
            MelonLogger.Msg("Fire Cheetos Inicializado!");
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
                    button.onClick.Invoke(); // Simula um clique no botão
                    MelonDebug.Msg($"Botão {buttonName} pressionado!");
                }
                else
                {
                    MelonLogger.Error($"O objeto {buttonName} não tem um componente Button!");
                }
            }
            else
            {
                MelonLogger.Error($"Botão {buttonName} não encontrado!");
            }
        }

        private void StartClicking()
        {
            clickCoroutine = MelonCoroutines.Start(ClickLoop());
            MelonDebug.Msg("Auto-click iniciado!");
        }

        private void StopClicking()
        {
            if (clickCoroutine != null) MelonCoroutines.Stop(clickCoroutine);
            buttonsUpgrade = null;
            MelonDebug.Msg("Auto-click parado!");
        }

        private IEnumerator ClickLoop()
        {
            while (feature2Enabled)
            {
                yield return new WaitForSeconds(0.5f); // Tempo entre cliques (ajuste conforme necessário)
                ClickAllUpgradeButtons();
                yield return new WaitForSeconds(0.5f); // Tempo entre cliques (ajuste conforme necessário)
            }
        }

        private void ClickAllUpgradeButtons()
        {
            var root = GameObject.Find("menusRoot/menuCanvasParent/SafeArea/menuCanvas/menus");
            if (root == null)
            {
                MelonDebug.Error("menusRoot não encontrado!");
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
                        MelonDebug.Msg($"Botão {button.gameObject.name} clicado!");
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
                MelonDebug.Error("Erro ao desativar o SpeedHackDetector: " + e);
            }

            feature5Enabled = !feature5Enabled;
            Time.timeScale = feature5Enabled ? 2f : 1f;
            MelonDebug.Msg($"Speed Hack: {feature5Enabled}");
        }

        public override void OnGUI()
        {
            if (!showMenu) return;

            const float menuX = 180;
            const float menuY = 80;
            const float width = 200;
            const float height = 190;

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
                    feature5Enabled ? "Speed Hack: ON" : "Speed Hack: OFF"))
                ToggleSpeedHack();

            if (GUI.Button(new Rect(buttonX, menuY + 150, buttonWidth, buttonHeight),
                    feature3Enabled ? "Weakened Enemies: ON" : "Weakened Enemies: OFF"))
            {
                feature3Enabled = !feature3Enabled;
                MelonDebug.Msg($"Weakened Enemies: {feature3Enabled}");
            }
        }
    }
}