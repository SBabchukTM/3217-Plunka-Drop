using System;
using System.Collections.Generic;
using UI.Windows;
using UnityEngine;

namespace UI
{
    public class WindowManager
    {
        private const string PrefabsFilePath = "Windows/";

        private static readonly Dictionary<Type, string> PrefabsDictionary = new Dictionary<Type, string>()
        {
            {typeof(LoadingWindow), "LoadingWindow"},

            {typeof(YouWinWindow),"YouWinWindow"},
            {typeof(YouLoseWindow),"YouLoseWindow"},
            {typeof(PausedWindow),"PausedWindow"},
            {typeof(BallDescriptionWindow), "BallDescriptionWindow" },

            {typeof(MenuWindow), "MenuWindow"},
            {typeof(ProfileWindow), "ProfileWindow"},
            {typeof(SelectPictureWindow), "SelectPictureWindow"},
            {typeof(SettingsWindow), "SettingsWindow"},
            {typeof(LeaderboardWindow), "LeaderboardWindow/LeaderboardWindow"},
            {typeof(SelectLevelWindow), "SelectLevelWindow/SelectLevelWindow" },

            {typeof(HowToPlayWindow), "HowToPlayWindow" },
            {typeof(PrivacyPolicyWindow), "PrivacyPolicyWindow"},
            {typeof(TermsOfUseWindow), "TermsOfUseWindow"}


        };

        public static T ShowWindow<T>() where T : Window
        {
            var go = GetPrefabByType<T>();
            if (go == null)
            {
                Debug.LogError("Show window - object not found");
                return null;
            }

            return GameObject.Instantiate(go, GUIHolder);
        }

        public static void ShowWindowByType(Type type)
        {
            var method = typeof(WindowManager).GetMethod("ShowWindow", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            var genericMethod = method.MakeGenericMethod(type);
            genericMethod.Invoke(null, null);
        }


        private static T GetPrefabByType<T>() where T : Window
        {
            var prefabName = PrefabsDictionary[typeof(T)];
            if (string.IsNullOrEmpty(prefabName))
            {
                Debug.LogError("cant find prefab type of " + typeof(T) + "Do you added it in PrefabsDictionary?");
            }

            var path = PrefabsFilePath + PrefabsDictionary[typeof(T)];
            var window = Resources.Load<T>(path);
            if (window == null)
            {
                Debug.LogError("Cant find prefab at path " + path);
            }

            return window;
        }

        public static Transform GUIHolder
        {
            get { return ServiceLocator.Current.Get<GUIHolder>().transform; }
        }
    }
}