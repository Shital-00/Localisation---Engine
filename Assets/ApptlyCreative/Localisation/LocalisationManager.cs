using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

namespace ApptlyCreative.Localisation {

	public class LocalisationManager : MonoBehaviour {

		public static LocalisationManager instance;

		[Header("Languages")]
		public List<languageItem> availableLanguages;

		private Dictionary<string, string> localisedText;
		private string missingTextString = "<String Missing>";


		void Awake () 
		{
			if (instance == null) {
				instance = this;
			} else if (instance != this)
			{
				Destroy (gameObject);
			}

			DontDestroyOnLoad (gameObject);
		}

		public void LoadLocalisedText(string fileName)
		{
			if (fileName.EndsWith(".strings"))
			{
				LoadLocalisedTextViaStrings(fileName);
				return;
			}
			if (fileName.EndsWith(".json"))
			{
				LoadLocalisedTextViaJSON(fileName);
				return;
			}
		}

		public void	LoadLocalisedTextViaJSON(string fileName)
		{
			localisedText = new Dictionary<string, string> ();
			string filePath = Path.Combine (Application.streamingAssetsPath, fileName);

			if (File.Exists (filePath)) {
				string dataAsJson = File.ReadAllText (filePath);
				localisationData loadedData = JsonUtility.FromJson<localisationData> (dataAsJson);

				for (int i = 0; i < loadedData.items.Length; i++) 
				{
					localisedText.Add (loadedData.items [i].key, loadedData.items [i].value);   
				}

				Debug.Log ("Data loaded, dictionary contains: " + localisedText.Count + " entries");
			} else 
			{
				Debug.LogError ("Cannot find file!");
			}
		}

		public void LoadLocalisedTextViaStrings(string fileName)
		{
			localisedText = new Dictionary<string, string> ();
			string filePath = Path.Combine (Application.streamingAssetsPath, fileName);

			if (File.Exists (filePath)) {

				StreamReader reader = File.OpenText(filePath);
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					if (line.StartsWith("\""))
					{
						string[] items = line.Split('"');
						localisedText.Add(items[1], items[3]);
					}
				}

				Debug.Log ("Data loaded, dictionary contains: " + localisedText.Count + " entries");
			} else 
			{
				Debug.LogError ("Cannot find file!");
			}
		}

		public string GetLocalisedValue(string key)
		{

			if (localisedText.Count == 0 || localisedText == null)
			{
				loadDefault();
			}

			string result = missingTextString;
			if (localisedText.ContainsKey (key)) 
			{
				result = localisedText [key];
			}

			return result;

		}

		public void loadDefault()
		{
			if (PlayerPrefs.HasKey("ACGLocalisationIndex"))
			{
				LoadLocalisedText(availableLanguages[PlayerPrefs.GetInt("ACGLocalisationIndex")].fileName);
			} else
			{
				LoadLocalisedText(availableLanguages[0].fileName);
			}
		}

		public void saveDefault(int pref)
		{
			PlayerPrefs.SetInt("ACGLocalisationIndex",pref);
		}
		


	}

	[System.Serializable]
	public class localisationData
	{
		public localisationItem[] items;
	}

	[System.Serializable]
	public class localisationItem 
	{
		public string key;
		public string value;
	}

	[System.Serializable]
	public class languageItem
	{
		public string name;
		public string fileName;
	}

}
