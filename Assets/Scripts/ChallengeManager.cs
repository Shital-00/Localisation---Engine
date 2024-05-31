using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;


public class ChallengeManager : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private TimelineManager timelineManager;
    [SerializeField] private Effect[] effect;

    [SerializeField] private List<ChallangeData> Challangedata;

    //public AudioClip otherClip;

  
    public int challangeIndex ;
   // private LanguageSwitcher languageSwitcher;
    public TextMeshProUGUI score;
    private int index;
    private int attempt = 1;

    private void Awake()
    {
        // index = SwitchLanguage.langIndex;
        //if (index == 0 || index == 1)
        //{
        // timelineManager.gameObject.GetComponent<TimelineManager>().enabled = true;
        //timelineManager.gameObject.GetComponent<AudioClipSwitcher>().enabled = false;
        //}
        //else if (index == 3)
        //{
        //timelineManager.enabled = false;
        //timelineManager.gameObject.GetComponent<AudioClipSwitcher>().enabled = true;
        timelineManager.gameObject.GetComponent<LOcalisationController>().enabled = true;
        
        //languageSwitcher = GetComponent<LanguageSwitcher>();
        //(ctrtimelineManager.gameObject.GetComponent<Aud>
      
       // }
    }

    private void Start()
    {
        ObjectChange();

    }
   

    public  void OptionSelect(int answer)
    {
        bool refval = Challangedata[challangeIndex].OptionsDataRef[answer].IsCorrect;
        Debug.Log("sprite clicked of index" + answer);
        if (refval)
        {
            if (timelineManager.playableDirector.state == PlayState.Paused)
            {
                Debug.Log("1");
                timelineManager.Playtime();
                effect[answer].ChangeScale();
                score.text = "Score :" + ((attempt++) * 20).ToString();
                timelineManager.RemoveTime();

            }

        }
        else
        {
            if (timelineManager.playableDirector.state == PlayState.Paused)
            {

                Debug.Log("2");

                audioManager.WrongAnswerAudio();
                effect[answer].Rotate();
                score.text = "Score :" + ((attempt--) * 20).ToString();

            }
        }

    }

    public void ObjectChange()
    {
        for (int j = 0; j < Challangedata[challangeIndex].optionsDataRef.Count; ++j)
        {
            Challangedata[challangeIndex].OptionsDataRef[j].textmeshproref.text = Challangedata[challangeIndex].OptionsDataRef[j].texts;

        }
        for (int i = 0; i < Challangedata[challangeIndex].QuetionDataRe.Count; ++i)
        {
            Challangedata[challangeIndex].QuetionDataRe[i].textmeshproref.text = Challangedata[challangeIndex].QuetionDataRe[i].text;
        }

    }
}


[System.Serializable]
public class ChallangeData
{
    public  List<OptionsData> optionsDataRef;
    public List<OptionsQuetion> QuetionDataRe;
    public List<OptionsData> OptionsDataRef { get => optionsDataRef; set => value = optionsDataRef; }
    public List<OptionsQuetion> QuetionDataRef { get => QuetionDataRe; set => value = QuetionDataRe; }
}
[System.Serializable]
public class OptionsData
{
    public GameObject OptionRef;
    public bool isCorrect;
    public string texts;
    public TMP_Text textmeshproref;
    public bool IsCorrect { get => isCorrect; set => value = isCorrect; }
    public TMP_Text Textmeshproref { get => textmeshproref; set => value = textmeshproref; }

    
}

[System.Serializable]
public class OptionsQuetion
{
    public GameObject Quetion;
    public  string text;
    public  TMP_Text textmeshproref;
    public string Text { get => text; set => value= text; }
    public TMP_Text Textmeshpro { get => textmeshproref; set => value = textmeshproref; }

}


