using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class TimelineManager : MonoBehaviour
{

    [SerializeField] private List<TimeData> times;
    public PlayableDirector playableDirector;

    [SerializeField]
    private int timeOutLimit1 = 0;
    [SerializeField]
    private int timeOutLimit2 = 0;

    //public AudioClip otherClip;
    private System.DateTime lastInteractionTime;

    int count = 0;

    [SerializeField] private AudioManager audioManager;

    [SerializeField] private ChallengeManager challengManager;

    public void Puse()
    {
        playableDirector.Pause();
        TimeStart();
    }

    

    public void Playtime()
    {
      playableDirector.Resume();
       
    }

    public void QuetionPlay()
    {
        
        float time1 = times[challengManager.challangeIndex].quetionAudio;
        playableDirector.time = time1;
        playableDirector.Resume();
        count++;
    }

    public void NextQuetion()
    {
        if(count ==1)
        {
            count--;
        }
        float nexttime = times[challengManager.challangeIndex].nextquetion;
        playableDirector.time = nexttime;
        
        challengManager.challangeIndex++;
        challengManager.ObjectChange();
        
    }

    private void TimeStart()
    {
        lastInteractionTime = System.DateTime.Now;
        InvokeRepeating("Timer", 1.0f, 1.0f);

    }
    
   

    public  void Timer()
    {
         if (System.DateTime.Now.Subtract(lastInteractionTime).Seconds == timeOutLimit1)
         {
            if (playableDirector.state == PlayState.Paused)
            {
                if (count == 0)
                {
                    QuetionPlay();
                    playableDirector.Play();
                }
               
                
                Debug.Log("after 6 second");
               
            }
           
        }

        if (System.DateTime.Now.Subtract(lastInteractionTime).Seconds == timeOutLimit2)
        {

            if (playableDirector.state == PlayState.Paused)
            {
                playableDirector.Play();
                NextQuetion();
               

                RemoveTime();
                Debug.Log("after 10 second");

            }

        }

       
    }
   public void RemoveTime()
    {
        CancelInvoke("Timer");
        Debug.Log("Destroy");
    }

}



[System.Serializable]
public class TimeData
{
    public float quetionAudio;
    public float QuetionAudio { get => QuetionAudio; set => value = QuetionAudio; }

    public float nextquetion;
    public float Nextquetion { get => Nextquetion; set => value = Nextquetion; }
}