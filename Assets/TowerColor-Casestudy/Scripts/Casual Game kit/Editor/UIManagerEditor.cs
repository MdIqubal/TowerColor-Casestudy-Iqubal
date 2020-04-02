using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace PnC.CasualGameKit
{
    [CustomEditor(typeof(UIManager), true)]
    public class UIManagerEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            UIManager uiManager = (UIManager)target;

            EditorGUILayout.LabelField("");  
            EditorGUILayout.HelpBox("After assigning screens, Hit this button to get an enum of screen names created/updated. Makes method calls easy.\n ** NOTE : Make sure the name is a valid enum name **\n" +
                                    " Example -  OpenScreen(UIScreensList.HUD);", MessageType.Info);

            if (GUILayout.Button("Generate UIScreens Enum"))
            {
                GenerateUIScreensEnum();
            }
        }

        /// <summary>
        /// Creates the audioClips enum at the uiManager's location
        /// </summary>
        public void GenerateUIScreensEnum()
        {
            //Get the list of screens
            UIManager uiManager = (UIManager)target;
            List<UIScreen> screens = uiManager.UIScreens;

            //Get the script's path
            MonoScript thisScript = MonoScript.FromMonoBehaviour(uiManager);
            string ScriptFilePath = AssetDatabase.GetAssetPath(thisScript);

            //Create a path for the enum file
            string enumName = "UIScreensList";
            string filePathAndName = ScriptFilePath.Replace(thisScript.name + ".cs", "") + "/" + enumName + ".cs";

            //Wrire the enum at above path
            using (StreamWriter streamWriter = new StreamWriter(filePathAndName))
            {
                streamWriter.WriteLine("public enum " + enumName);
                streamWriter.WriteLine("{");
                for (int i = 0; i < screens.Count; i++)
                {
                    streamWriter.WriteLine("\t" + screens[i].screenName + ",");
                }
                streamWriter.WriteLine("}");
            }
            AssetDatabase.Refresh();

            Debug.Log("UIScreens  enum created/updated at " + ScriptFilePath);

        }
    }
}