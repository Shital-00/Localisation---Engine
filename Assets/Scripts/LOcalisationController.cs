using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class LOcalisationController : MonoBehaviour
{
    public TimelineAsset timelineAsset;
    public int index;
    //private LanguageSwitcher languageSwitcher;
    [System.Serializable]
    public class SpanishAudioClipInfo
    {
        public AudioClip audioClip;
        public string audioName;
        public double startTime;
        public double endTime;
    }
    [System.Serializable]
    public class EnglishAudioClipInfo
    {
        public AudioClip audioClip;
        public string audioName;
        public double startTime;
        public double endTime;
    }
    //For removing Audio clips
    [System.Serializable]
    public class TimeOfPlacedAudio
    {
        public double startTime;
        public double endTime;
    }
    [System.Serializable]
    public class EnglishTimeOfSignals
    {
        public double time;
        public string signalName;
    }
    [System.Serializable]
    public class SpanishTimeOfSignals
    {
        public double time;
        public string signalName;
    }

    public SpanishAudioClipInfo[] spanishaudioClipInfos; // Array of  spanish audio clips to switch between
    public EnglishAudioClipInfo[] englishaudioClipInfos;
    public TimeOfPlacedAudio[] timeranges;
    public EnglishTimeOfSignals[] signalTimes;    // Times of English signals emitter
    public SpanishTimeOfSignals[] spanishSignalTimes; //Times of Spanish signals emitter
    private PlayableDirector director;
    private bool clipFound;
    private bool EngSignalFound;
    private bool SpaSignalFound;
    [HideInInspector]
    public string[] langArray = new string[] { "English", "English-US", "Spanish" };
    
   
    void Start()
    {
        index = SwitchLanguage.langIndex;
        director = GetComponent<PlayableDirector>();
        director.playableAsset = timelineAsset;
        clipFound = CheckAudioClipInRange();
        EngSignalFound = CheckSignalsIspresent(3.12);
        SpaSignalFound = CheckSignalsIspresent(3.4);
        
        if (index == 0 || index == 1)
        {
           
            if (clipFound)
            {
                foreach (var duration in timeranges)
                {
                    RemoveAudioClipsByTheirTime(duration.startTime, duration.endTime);
                }
                foreach (var audioClipInfos in englishaudioClipInfos)
                {
                    AssignEnglishAudioClip(audioClipInfos.audioClip, audioClipInfos.startTime, audioClipInfos.endTime);
                }

            }
            else
            {
                foreach (var audioClipInfos in englishaudioClipInfos)
                {
                    AssignEnglishAudioClip(audioClipInfos.audioClip, audioClipInfos.startTime, audioClipInfos.endTime);
                }
            }


            if (SpaSignalFound)
            {
                RemoveSignals();
                foreach (var signal in signalTimes)
                {
                    //AddSignalEvent(signal.time, signal.signalName);
                    AddSignalEvent(signal.time, signal.signalName);
                }
            }
            else if (EngSignalFound)
            {
                PlayTimeline();
            }
            else
            {
                foreach (var signal in signalTimes)
                {
                    //AddSignalEvent(signal.time, signal.signalName);
                    AddSignalEvent(signal.time, signal.signalName);
                }
            }
        }
        else if (index == 3)
        {
           
            if (clipFound)
            {
                foreach (var duration in timeranges)
                {
                    RemoveAudioClipsByTheirTime(duration.startTime, duration.endTime);
                }
                foreach (var audioClipInfos in spanishaudioClipInfos)
                {
                    AssignSpanishAudioClip(audioClipInfos.audioClip, audioClipInfos.startTime, audioClipInfos.endTime);
                }
            }
            else
            {
                foreach (var audioClipInfos in spanishaudioClipInfos)
                {
                    AssignSpanishAudioClip(audioClipInfos.audioClip, audioClipInfos.startTime, audioClipInfos.endTime);
                }
            }


            if (EngSignalFound)
            {
                PlayTimeline();
                //RemoveSignals();
                //foreach (var signal in spanishSignalTimes)
                //{
                //    AddSignalEvent(signal.time, signal.signalName);
                //}
            }
            else if (SpaSignalFound)
            {
                //PlayTimeline();

            }
            else
            {
                // PlacedSpanishSignals();
                foreach (var signal in spanishSignalTimes)
                {
                    AddSignalEvent(signal.time, signal.signalName);
                }
            }


        }

        PlayTimeline();
    }
   
    //private void Update()
    //{
    //   if (Input.GetKeyDown(KeyCode.Space))
    //   {
    //       languageSwitcher.SaveDataToJson();
    //   }
        
    //}
    private AudioTrack FindAudioTrack(TimelineAsset timeline)
    {
        foreach (var track in timeline.GetOutputTracks())
        {
            if (track is AudioTrack audioTrack)
            {
                return audioTrack;
            }
        }
        return null;
    }
   

    public void AssignSpanishAudioClip(AudioClip audioClip, double startTime, double endTime)
    {
        var audioTrack = FindAudioTrack(timelineAsset);
        TimelineClip clip = audioTrack.CreateDefaultClip();
        var audioPlayableAsset = clip.asset as AudioPlayableAsset;
        audioPlayableAsset.clip = audioClip;
        clip.start = startTime;
        clip.duration = endTime - startTime;

    }
    public void AssignEnglishAudioClip(AudioClip audioClip, double startTime, double endTime)
    {
        var audioTrack = FindAudioTrack(timelineAsset);
        TimelineClip clip = audioTrack.CreateDefaultClip();
        var audioPlayableAsset = clip.asset as AudioPlayableAsset;
        audioPlayableAsset.clip = audioClip;
        clip.start = startTime;
        clip.duration = endTime - startTime;

    }
    public void PlayTimeline()
    {
        EditorUtility.SetDirty(timelineAsset);
        AssetDatabase.SaveAssets();
        director.Play();
    }


    public void AddSignalEvent(double time, string signalName)
    {
        if (director != null)
        {
            TrackAsset track = timelineAsset.GetOutputTrack(0);
            SignalEmitter marker = track.CreateMarker<SignalEmitter>(time);
            marker.name = signalName;
        }
        PlayTimeline();

    }
    //Check if signal is present or not
    public bool CheckSignalsIspresent(double time)
    {
        double Time = time;
        TrackAsset track = timelineAsset.GetOutputTrack(0);
        var markers = track.GetMarkers().ToArray();
        foreach (IMarker marker in markers)
        {
            // IMarker marker = track.GetMarker();
            if (marker is SignalEmitter signalEmitter)
            {
                if (marker.time == Time)
                {
                    return true;
                    Debug.Log("Signal Emitter " + signalEmitter.name + " is present at time ");

                }
            }
        }
        return false;
    }

    public void PlacedEnglishSignals()
    {
        foreach (var signal in signalTimes)
        {
            //AddSignalEvent(signal.time, signal.signalName);
            AddSignalEvent(signal.time, signal.signalName);
        }
    }
    public void PlacedSpanishSignals()
    {
        foreach (var signal in spanishSignalTimes)
        {
            AddSignalEvent(signal.time, signal.signalName);
        }
    }
    //Remove signals of timeline
    public void RemoveSignals()
    {
        TrackAsset track = timelineAsset.GetOutputTrack(0);
        var markers = track.GetMarkers().ToArray();
        foreach (IMarker marker in markers)
        {
            if (marker is SignalEmitter signalEmitter)
            {
                track.DeleteMarker(marker);
            }
        }
        //var clipsToRemove = new List<TimelineClip>();
        //foreach (var track in timelineAsset.GetOutputTracks())
        //{
        //    if (track is MarkerTrack markerTrack)
        //    {
        //        //var markers = markerTrack.GetMarkers().ToArray();
        //        foreach (var clip in markerTrack.GetMarkers())
        //        {
        //            if(clip is SignalEmitter signalEmitter)
        //            {
        //                markerTrack.DeleteMarker(clip);
        //            }


        //        }

        //    }
        //}
    }

    public bool CheckAudioClipInRange()
    {
        var audioTrack = FindAudioTrack(timelineAsset);
        if (audioTrack != null)
        {

            foreach (var clip in audioTrack.GetClips())
            {
                double clipStart = clip.start;
                double clipEnd = clipStart + clip.duration;
                double startTime = 3.12;
                double endTime = 6.24;

                if (clipStart >= startTime && clipEnd <= endTime)
                {
                    AudioClip audioClip = (clip.asset as AudioPlayableAsset).clip;

                    return true;
                }
            }
        }
        return false;
    }

    public void RemoveAudioClipsByTheirTime(double startTime, double endTime)
    {
        var audioTrack = FindAudioTrack(timelineAsset);
        double duration = endTime - startTime;
        foreach (TimelineClip clip in audioTrack.GetClips())
        {
            if (clip.start == startTime && clip.duration == duration)
            {
                audioTrack.DeleteClip(clip);
            }
        }
    }
}
