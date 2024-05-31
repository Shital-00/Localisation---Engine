
using UnityEngine;
using System;
using System.IO;



    public class SaveDataManager: MonoBehaviour
    {
        // Start is called before the first frame update
        public EnglishInfo.EnglishGame englishGame = new EnglishInfo.EnglishGame();
        public SpanishInfo.SpanishGame spanishGame = new SpanishInfo.SpanishGame();



        public const string directoryName = "/SaveData";
        public const string EnglishfileName = "/EnglishGameData.json";
        public const string SpanishfileName = "/SpanishGameData.json";
        

        public void SaveEnglishData()
        {
            var dir = Application.streamingAssetsPath + directoryName;
            if(!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string json = JsonUtility.ToJson(englishGame, true);
            File.WriteAllText(dir + EnglishfileName, json);
            Debug.Log(dir + EnglishfileName);
           // GUIUtility.systemCopyBuffer = dir;
            //return true;
        }
        public void SaveSpanishData()
        {
            var dir = Application.streamingAssetsPath + directoryName;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string json = JsonUtility.ToJson(spanishGame, true);
            File.WriteAllText(dir + SpanishfileName, json);
            Debug.Log(dir + SpanishfileName);
            //GUIUtility.systemCopyBuffer = dir;
            //return true;
        }
    }


