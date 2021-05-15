using System;
using System.Linq;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Align.Patches
{
    public static class MenuPatches
    {
        public static void AddSceneLoadedHandler()
        {
            SceneManager.add_sceneLoaded((Action<Scene, LoadSceneMode>) OnSceneLoaded);
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            switch (scene.name)
            {
                case "MainMenu":
                    MainMenuPatches.RepositionObjects();
                    break;
                case "MMOnline":
                    MmOnlinePatches.RepositionObjects();
                    break;
                case "FindAGame":
                    FindAGamePatches.RepositionObjects();
                    break;
            }
        }

        private static class MainMenuPatches
        {
            public static void RepositionObjects()
            {
                RepositionLocalButton();
            }

            private static void RepositionLocalButton()
            {
                var localButton = GameObject.Find("PlayLocalButton");
                if (!localButton) return;
                
                var position = localButton.transform.localPosition;
                position.x = -1.025f;
                localButton.transform.localPosition = position;
            }
        }

        private static class MmOnlinePatches
        {
            public static void RepositionObjects()
            {
                RepositionBackButton();
            }
            
            private static void RepositionBackButton()
            {
                var backButton = GameObject.Find("BackButton");
                if (!backButton) return;

                var position = backButton.transform.localPosition;
                position.x = -4.733334f;
                position.y = -2.65f;
                backButton.transform.localPosition = position;
            }
        }

        private static class CreateGameMenuPatches
        {
            [HarmonyPatch(typeof(CreateGameOptions), nameof(CreateGameOptions.Show))]
            public static class TextAlignmentPatch
            {
                public static void Postfix()
                {
                    var createGameOptions = UnityEngine.Object.FindObjectOfType<CreateGameOptions>();
                    if (!createGameOptions) return;
                    
                    RepositionImpostorsText(createGameOptions);
                    FixAirshipButtonHitBox(createGameOptions);
                }
            }

            private static void RepositionImpostorsText(CreateGameOptions createGameOptions)
            {
                var impostorsText = createGameOptions.Content.GetComponentsInChildren<TextMeshPro>()
                    .FirstOrDefault(textMeshPro => textMeshPro.gameObject.transform.parent.name == "Impostors");
                if (!impostorsText) return;
                    
                var position = impostorsText.rectTransform.localPosition;
                position.x = -0.499f;
                impostorsText.rectTransform.localPosition = position;
            }

            private static void FixAirshipButtonHitBox(CreateGameOptions createGameOptions)
            {
                var mapIcon = createGameOptions.Content.transform.FindChild("Map")?.FindChild("4")
                    ?.FindChild("MapIcon4");
                if (!mapIcon) return;

                var boxCollider = mapIcon.transform.parent.GetComponent<BoxCollider2D>();
                var size = boxCollider.size;
                size.x = 2.1f;
                boxCollider.size = size;
            }
        }

        private static class FindAGamePatches
        {
            public static void RepositionObjects()
            {
                RepositionImpostorFilter();
            }

            private static void RepositionImpostorFilter()
            {
                var impostorFilter = GameObject.Find("Impostors");
                if (!impostorFilter) return;

                var textMeshPro = impostorFilter.GetComponentInChildren<TextMeshPro>();
                var position = textMeshPro.rectTransform.localPosition;
                position.x = -1.38f;
                textMeshPro.rectTransform.localPosition = position;
            }
        }
    }
}