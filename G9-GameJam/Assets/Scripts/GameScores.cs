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
        return JsonHelper.FromJson<PlayerScore>(File.ReadAllText(Path.Combine(Application.persistentDataPath, "scores.json")));
    }

    public static void SaveScoreToFile(PlayerScore score)
    {
        PlayerScore[] scores = null;
        if (!File.Exists(Path.Combine(Application.persistentDataPath, "scores.json")))
            scores = new PlayerScore[1];
        else
        {
            scores = JsonHelper.FromJson<PlayerScore>(File.ReadAllText(Path.Combine(Application.persistentDataPath, "scores.json")));
            PlayerScore[] newScores = new PlayerScore[scores.Length + 1];
            for(int i = 0; i < scores.Length; i++)
            {
                newScores[i] = scores[i];
            }
            scores = newScores;
        }
        scores[scores.Length - 1] = score;
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "scores.json"), JsonHelper.ToJson<PlayerScore>(scores));
    }

    class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}