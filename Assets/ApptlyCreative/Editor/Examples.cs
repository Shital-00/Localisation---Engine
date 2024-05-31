using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace ApptlyCreative.Localisation
{
	public class Examples : MonoBehaviour
	{

		private bool on = false;

		void Start()
		{
			Click();
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

			//TIP: Make sure a language has been selected first or the key will throw an error.
		}

	}

}
