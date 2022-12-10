using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class GameScores
{
    [System.Serializable]
    public struct PlayerScore
    {
        public string name;
        public int highscore;
    }
    public static PlayerScore[] GetScores()
    {
        if (!File.Exists(Path.Combine(Application.persistentDataPath, "scores.json"))) return null;
        return JsonHelper.getJsonArray<PlayerScore>(File.ReadAllText("scores.json"));
    }

    public static void SaveScoreToFile(PlayerScore score)
    {
        PlayerScore[] scores = null;
        if (!File.Exists("scores.json"))
            scores = new PlayerScore[1];
        else
        {
            scores = JsonHelper.getJsonArray<PlayerScore>(File.ReadAllText("scores.json"));
            PlayerScore[] newScores = new PlayerScore[scores.Length + 1];
            for(int i = 0; i < scores.Length; i++)
            {
                newScores[i] = scores[i];
            }
            scores = newScores;
        }
        scores[scores.Length - 1] = score;
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "scores.json"), JsonHelper.arrayToJson<PlayerScore>(scores));
    }

    class JsonHelper
    {
        public JsonHelper()
        {
        }
        public static T[] getJsonArray<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }
        public static string arrayToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.array = array;
            return JsonUtility.ToJson(wrapper);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }
}