using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpanishInfo
{
    public SpanishGame spanishgame = new SpanishGame();

    [System.Serializable]
    public class SpanishGame
    {
        public bool isSpanish;
        public string Title;
        public List<SpanishAudioClipInfo> spanishAudioClips = new List<SpanishAudioClipInfo>();
        public List<SpanishTimeOfSignals> spanishTimeOfSignals = new List<SpanishTimeOfSignals>();
    }
    [System.Serializable]
    public class SpanishAudioClipInfo
    {
        public AudioClip audioClip;
        public string audioName;
        public double startTime;
        public double endTime;
    }
    [System.Serializable]
    public class SpanishTimeOfSignals
    {
        public double time;
        public string signalName;
    }
}
