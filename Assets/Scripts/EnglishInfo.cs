using System;
using System.Collections.Generic;
using UnityEngine;



    public class EnglishInfo
    {

        public EnglishGame englishgame = new EnglishGame();
       
        [System.Serializable]
        public class EnglishGame
        {
            public bool isEnglish;
            public string Title;
            public List<EnglishAudioClipInfo> englishAudioClips = new List<EnglishAudioClipInfo>();
            public List<EnglishTimeOfSignals> englishSignalsOfTime = new List<EnglishTimeOfSignals>();
        }
       
        [System.Serializable]
        public class EnglishAudioClipInfo
        {
            public AudioClip audioClip;
            public string audioName;
            public double startTime;
            public double endTime;
        }
        [System.Serializable]
        public class EnglishTimeOfSignals
        {
            public double time;
            public string signalName;
        }
       
    }


