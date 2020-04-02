//Author : Iqubal
//email : iqubal333@gmail.com

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace PnC.CasualGameKit
{
    [CustomEditor(typeof(ObjectPooler), true)]
    public class ObjectPoolerEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ObjectPooler objectPooler = (ObjectPooler)target;

            EditorGUILayout.LabelField("");
            EditorGUILayout.HelpBox("After assigning Items, Hit this button to get an enum of poolable items created. Makes method calls easy.\n ** NOTE : Make sure the name is a valid enum name **\n" +
                                    "Example -  objectPooler.GetPooledObject(ObjectPoolItems.leftoverBlock, true);", MessageType.Info);

            if (GUILayout.Button("Generate ObjectPoolItems Enum"))
            {
                GenerateObjectPoolerItemsEnum();
            }
        }

        /// <summary>
        /// Creates the ObjectPoolItems enum at the ObjectPooler's location
        /// </summary>
        public void GenerateObjectPoolerItemsEnum()
        {
            //Get the list of Object Pool Items
            ObjectPooler objectPooler = (ObjectPooler)target;
            List<PoolItem> itemsToPool = objectPooler.itemsToPool;

            //Get the script's path
            MonoScript thisScript = MonoScript.FromMonoBehaviour(objectPooler);
            string ScriptFilePath = AssetDatabase.GetAssetPath(thisScript);

            //Create a path for the enum file
            string enumName = "ObjectPoolItems";
            string filePathAndName = ScriptFilePath.Replace(thisScript.name + ".cs", "") + "/" + enumName + ".cs";

            //Wrire the enum at above path
            using (StreamWriter streamWriter = new StreamWriter(filePathAndName))
            {
                streamWriter.WriteLine("public enum " + enumName);
                streamWriter.WriteLine("{");
                for (int i = 0; i < itemsToPool.Count; i++)
                {
                    streamWriter.WriteLine("\t" + itemsToPool[i].itemName + ",");
                }
                streamWriter.WriteLine("}");
            }
            AssetDatabase.Refresh();

            Debug.Log("ObjectPoolItems  enum created/updated at " + ScriptFilePath);

        }
    }
}
