using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ApptlyCreative.Localisation
{

	public class LocaliseObject : MonoBehaviour
	{

		[Header("Localisation")]
		public string key;
		[Header("Affected Components")]
		public bool textUI;
		public bool buttonUI;
		public bool imageUI;
		public bool texture2D;
		public bool audioUI;
		[Header("Modifiers")]
		public bool useButtonToSetLocale;
		public int localisionIndex;
		public bool goToNextSceneOnClick;
		public string nextSceneName;
		private bool on = false;
		public bool gameScene;
		

		void Start()
		{
			setLocalisedObject();

		}

		public void setLocalisedObject()
		{
			if (textUI == true)
			{
				Text comp = GetComponent<Text>();
				comp.text = LocalisationManager.instance.GetLocalisedValue(key);
			}
			if (buttonUI == true && useButtonToSetLocale == false)
			{
				Text comp = transform.GetChild(0).GetComponent<Text>();
				comp.text = LocalisationManager.instance.GetLocalisedValue(key);
			}
			if (buttonUI == true && useButtonToSetLocale == true)
			{
				Text comp = transform.GetChild(0).GetComponent<Text>();
				comp.text = LocalisationManager.instance.availableLanguages[localisionIndex].name;
				Button btn = GetComponent<Button>();
				btn.onClick.AddListener(() => LocalisationManager.instance.LoadLocalisedText(LocalisationManager.instance.availableLanguages[localisionIndex].fileName));
				btn.onClick.AddListener(() => LocalisationManager.instance.saveDefault(localisionIndex));

				if (goToNextSceneOnClick)
				{
					if (PlayerPrefs.HasKey("ACGLocalisationIndex"))
					{
						LocalisationManager.instance.loadDefault();
						SceneManager.LoadScene(nextSceneName);
					}
					else
					{
						btn.onClick.AddListener(() => SceneManager.LoadScene(nextSceneName));
					}
				}
			}
			if (imageUI == true)
			{
				Image comp = GetComponent<Image>();
				comp.sprite = (Sprite)Resources.Load(LocalisationManager.instance.GetLocalisedValue(key));
			}
			if (texture2D == true)
			{
				Texture2D comp = GetComponent<Texture2D>();
				comp = (Texture2D)Resources.Load(LocalisationManager.instance.GetLocalisedValue(key));
			}
            if (buttonUI == true && gameScene == true && useButtonToSetLocale == false)
            {
				if(goToNextSceneOnClick)
                {
					if(localisionIndex == 0 || localisionIndex == 1)
                    {
						Button btn = GetComponent<Button>();
						btn.onClick.AddListener(() =>
						{
							//SaveDataManager.SaveEnglishData();
							SceneManager.LoadScene(nextSceneName);
							//SaveDataTester.SaveSpanish();
							//GetComponent<AudioClipSwitcher>().enabled = false;
						});
					}
					else if(localisionIndex == 2)
                    {
						//for chinese
                    }
					else if(localisionIndex == 3)
                    {
						Button btn = GetComponent<Button>();
						btn.onClick.AddListener(() =>
						{
							//SaveDataManager.SaveSpanishData();
							SceneManager.LoadScene(nextSceneName);
							
							//GetComponent<AudioClipSwitcher>().enabled = true;
						});
					}
                    
					
				}
            }
			




        }
		public void Click()
		{
			//2. Get a reference to the component you wish to modify
			Text comp = transform.GetChild(0).GetComponent<Text>();

			if (on)
			{
				on = false;
				//3. Access the localisation manager instance, call 'GetLocalisedValue' and insert your key
				comp.text = LocalisationManager.instance.GetLocalisedValue("1_MainMenu_SwitchOff");
			}
			else
			{
				on = true;
				comp.text = LocalisationManager.instance.GetLocalisedValue("1_MainMenu_SwitchON");
			}
		}
        public void GoToNextScene(int id)
        {
		   SwitchLanguage.langIndex = id;
		   SceneManager.LoadScene(nextSceneName);	
        }
       
    }

	#if UNITY_EDITOR
	[CustomEditor(typeof(LocaliseObject))]
	public class LocalisationObjectEditor : Editor {
		public override void OnInspectorGUI() {

			ShowHeaderLogo(EditorGUIUtility.LoadRequired("ApptlyCreative/infogen log.jpeg") as Texture);

			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			EditorGUILayout.LabelField("Insert the key and tick on the relevant components on this game object you would like to be affected.",guiMessageStyle);
			EditorGUILayout.EndVertical();
			EditorGUILayout.Space();

			EditorGUILayout.BeginVertical(EditorStyles.helpBox);

			DrawDefaultInspector();

			EditorGUILayout.EndVertical();
			EditorGUILayout.Space();

		}

		void ShowHeaderLogo(Texture tex) {
			var rect = GUILayoutUtility.GetRect(0f, 0f);
			rect.width = tex.width;
			rect.height = tex.height;
			GUILayout.Space(rect.height);
			GUI.DrawTexture(rect, tex);

			var e = Event.current;
			if (e.type != EventType.MouseUp) {
				return;
			}
			if (!rect.Contains(e.mousePosition)) {
				return;
			}
		}

		GUIStyle guiMessageStyle{
			get{
				var messageStyle = new GUIStyle (GUI.skin.label);
				messageStyle.wordWrap = true;

				return messageStyle;
			}
		}
	}
	#endif

}
