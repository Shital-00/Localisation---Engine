using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ApptlyCreative.Localisation {

	public class LocalisationEditor : EditorWindow {

		public localisationData localisedData;
		bool showDebug = false;

		[MenuItem ("Window/Apptly Creative/Localisation Editor")]
		//[MenuItem ("Window/Apptly Creative/Timeline")]
		static void Init()
		{
			EditorWindow.GetWindow (typeof(LocalisationEditor),false,"Localisation",true).Show ();
			//EditorWindow.GetWindow(typeof(LocalisationEditor), false, "Timeline", true).Show();
		}

		private void OnGUI()
		{
			EditorGUILayout.Space();
			//GUIHelperACG.ShowHeaderLogo(EditorGUIUtility.LoadRequired("ApptlyCreative/inspector_header_apptlycreative.png") as Texture);
			GUIHelperACG.ShowHeaderLogo(EditorGUIUtility.LoadRequired("ApptlyCreative/infogen log.jpeg") as Texture);
			GUILayout.Label("Localisation Editor", EditorStyles.boldLabel);
			EditorGUILayout.Space();

			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			EditorGUILayout.LabelField("Load or create new lists of keys and values. Make sure each language you are using has the same set of keys and the values for each language are the localised items for that specific audience.",GUIHelperACG.guiMessageStyle);
			EditorGUILayout.EndVertical();
			EditorGUILayout.Space();

			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			GUILayout.Label("Add / Modify Language", EditorStyles.boldLabel);

			if (localisedData != null) 
			{
				SerializedObject serializedObject = new SerializedObject (this);
				SerializedProperty serializedProperty = serializedObject.FindProperty ("localisedData");
				EditorGUILayout.PropertyField (serializedProperty, true);
				serializedObject.ApplyModifiedProperties ();
			}

			EditorGUILayout.Space();

			GUILayout.BeginHorizontal();

			if (GUILayout.Button ("Create New Language")) 
			{
				CreateNewData ();
			}

			if (GUILayout.Button("Clear Values"))
			{
				ClearValues();
			}

			GUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
			EditorGUILayout.Space();

			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			GUILayout.Label("Data Management", EditorStyles.boldLabel);
			GUILayout.BeginHorizontal();

			if (localisedData != null) 
			{
				if (GUILayout.Button ("Save Language - JSON")) 
				{
					SaveGameDataJSON ();
				}
			}

			if (GUILayout.Button ("Load Language - JSON")) 
			{
				LoadGameDataJSON ();
			}

			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();

			if (localisedData != null) 
			{
				if (GUILayout.Button ("Save Language - Strings")) 
				{
					SaveGameDataStrings ();
				}
			}

			if (GUILayout.Button ("Load Language - Strings")) 
			{
				LoadGameDataStrings ();
			}

			GUILayout.EndHorizontal();
			EditorGUILayout.Space();

			showDebug = EditorGUILayout.Foldout(showDebug, "Debugging");

			if (showDebug) {

				if (GUILayout.Button ("Clear Language PlayerPref")) {
					ClearPlayerPref ();
				}

				EditorGUILayout.BeginVertical(EditorStyles.helpBox);
				EditorGUILayout.LabelField("While testing you may need to reset the selected language, you can do this by removing the PlayerPref. Note that this will only affect the language preferences.", GUIHelperACG.guiMessageStyle);
				EditorGUILayout.EndVertical();

			}

			EditorGUILayout.EndVertical();
			EditorGUILayout.Space();


		}

		private void LoadGameDataJSON()
		{
			string filePath = EditorUtility.OpenFilePanel ("Select localisation data file", Application.streamingAssetsPath, "json");

			if (!string.IsNullOrEmpty (filePath)) 
			{
				string dataAsJson = File.ReadAllText (filePath);

				localisedData = JsonUtility.FromJson<localisationData> (dataAsJson);
			}
		}

		private void SaveGameDataJSON()
		{
			string filePath = EditorUtility.SaveFilePanel ("Save localisation data file", Application.streamingAssetsPath, "", "json");

			if (!string.IsNullOrEmpty(filePath))
			{
				string dataAsJson = JsonUtility.ToJson(localisedData);
				File.WriteAllText (filePath, dataAsJson);
			}
		}

		private void LoadGameDataStrings()
		{
			string filePath = EditorUtility.OpenFilePanel ("Select localisation data file", Application.streamingAssetsPath, "strings");

			if (!string.IsNullOrEmpty(filePath))
			{
				StreamReader reader = File.OpenText(filePath);
				string line;
				List<localisationItem> localisedText = new List<localisationItem>();

				while ((line = reader.ReadLine()) != null) {
					if (line.StartsWith("\"")) {
						string[] items = line.Split('"');
						localisationItem item = new localisationItem();
						item.key = items[1];
						item.value = items[3];
						localisedText.Add (item); 
					}
				}

				localisedData = new localisationData ();
				localisedData.items = new localisationItem[localisedText.Count];
				localisedData.items = localisedText.ToArray();
			}
		}

		private void SaveGameDataStrings()
		{
			string filePath = EditorUtility.SaveFilePanel("Save localisation data file", Application.streamingAssetsPath, "", "strings");
			if (!string.IsNullOrEmpty(filePath))
			{
				using (StreamWriter file = new StreamWriter(filePath))
				{
					foreach (localisationItem item in localisedData.items)
					{
						file.WriteLine(string.Format("\"{0}\" = \"{1}\";",item.key, item.value));
					}
				}
			}

		}

		private void CreateNewData()
		{
			localisedData = new localisationData ();
		}

		private void ClearValues()
		{
			foreach (localisationItem item in localisedData.items)
			{
				item.value = "";
			}
		}

		private void ClearPlayerPref()
		{
			if (PlayerPrefs.HasKey("ACGLocalisationIndex"))
			{
				PlayerPrefs.DeleteKey("ACGLocalisationIndex");
			}
		}

	}

	[CustomPropertyDrawer(typeof(localisationItem))]
	public class LocalisationItemDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			var keyRect = new Rect(position.x + 30, position.y, 160, position.height);
			var labelRect = new Rect(position.x + 165, position.y, 60, position.height);
			var valueRect = new Rect(position.x + 205, position.y, 240, position.height);

			EditorGUI.LabelField(position, "Key", "");
			EditorGUI.PropertyField(keyRect, property.FindPropertyRelative("key"), GUIContent.none);
			EditorGUI.LabelField(labelRect, "Value", "");
			EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("value"), GUIContent.none);

			EditorGUI.EndProperty();
		}
	}


}