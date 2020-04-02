//Author : Iqubal
//email : iqubal333@gmail.com

using UnityEngine;
using System.Collections.Generic;

namespace PnC.CasualGameKit
{
    /// <summary>
    /// UI Screen class.
    /// </summary>
    [System.Serializable]
    public class UIScreen
    {
        public string screenName;
        public GameObject screenGameObj;
    }

    //TODO : integrate tween for entry, exit and tick states with UI manager
    public interface IScreenTweener
    {
        //void enterTween
        //void exitTween
        void Tween();
    }

    /// <summary>
    /// UI manager for easy management of UI screens and transitions.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        ///<summary><list type of UI Screens </summary>
        public List<UIScreen> UIScreens;

        private GameObject lastScreen, openScreen, modalScreen;

        // <summary>Dictionary of screen name and respective gameobject </summary>
        Dictionary<string, GameObject> screensDictionary = new Dictionary<string, GameObject>();

        private void Awake()
        {
            //Fill the dictionary
            foreach (UIScreen item in UIScreens)
            {
                screensDictionary.Add(item.screenName, item.screenGameObj);
            }
            AwakeInit();
        }

        /// <summary>
        /// To be used instead of Awake in the derived class
        /// </summary>
        protected virtual void AwakeInit()
        {
        }

        /// <summary>
        /// Opens  a screen with the UIScreensList enum
        /// </summary>
        public void OpenScreen(UIScreensList screen)
        {
            OpenScreen(screen.ToString());
        }

        /// <summary>
        /// Opens a screen with the screen name
        /// </summary>
        public void OpenScreen(string screenName)
        {
            GameObject screen = null;
            if (screensDictionary.TryGetValue(screenName, out screen))
            {
                if (openScreen != null)
                {
                    openScreen.SetActive(false);
                    lastScreen = openScreen;
                }
                screen.SetActive(true);
                openScreen = screen;
            }
            else
            {
                Debug.LogError("Screen does not exist");
            }
        }

        /// <summary>
        /// Closes current screen with the screen name
        /// </summary>
        public void CloseCurrentScreen()
        {
                if (openScreen != null)
                {
                    openScreen.SetActive(false);
                    lastScreen = openScreen;
                }  
        }

        /// <summary>
        /// Go back to the previous screen
        /// </summary>
        public void Back()
        {
            if (lastScreen != null)
                lastScreen.SetActive(true);

            openScreen.SetActive(false);
            GameObject temp = lastScreen;
            lastScreen = openScreen;
            openScreen = temp;
        }

        /// <summary>
        /// Open a screen as a modal with UIScreensList enum
        /// </summary>
        public void OpenModal(UIScreensList screen)
        {
            OpenModal(screen.ToString());
        }

        /// <summary>
        /// Open a screen as a modal with screen name
        /// </summary>
        public void OpenModal(string screenName)
        {
            GameObject screen = null;
            if (screensDictionary.TryGetValue(screenName, out screen))
            {
                screen.SetActive(true);
                modalScreen = screen;
            }
            else
            {
                Debug.LogError("Screen does not exist");
            }
        }

        /// <summary>
        /// close the already open modal screena
        /// </summary>
        public void CloseModal()
        {
            if (modalScreen != null)
                modalScreen.SetActive(false);
        }

    }

}