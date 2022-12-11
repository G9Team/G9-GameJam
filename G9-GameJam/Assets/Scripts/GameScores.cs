using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;
using UnityEditor.Experimental.GraphView;

public class GameScores
{
    [System.Serializable]
    public struct PlayerScore
    {
        public string name;
        public int highscore;
    }
    public static PlayerScore[] GetScores(int level)
    {
        if (!File.Exists(Path.Combine(Application.persistentDataPath, "scores.json"))) return null;
        try
        {
            Dictionary<int, PlayerScore[]> all_scores = JsonConvert.DeserializeObject<Dictionary<int, PlayerScore[]>>(File.ReadAllText(Path.Combine(Application.persistentDataPath, "scores.json")));
            return all_scores.ContainsKey(level) ? all_scores[level] : null;
        }
        catch(System.Exception e) //json component mismatch
        {
            File.Delete(Path.Combine(Application.persistentDataPath, "scores.json"));
            return null;
        }
    }

    public static void SaveScoreToFile(int level, PlayerScore score)
    {
        Dictionary<int, PlayerScore[]> scores = new Dictionary<int, PlayerScore[]>();
        if (File.Exists(Path.Combine(Application.persistentDataPath, "scores.json")))
        {
            try
            {
                scores = JsonConvert.DeserializeObject<Dictionary<int, PlayerScore[]>>(File.ReadAllText(Path.Combine(Application.persistentDataPath, "scores.json")));
            }
            catch (System.Exception e) //json component mismatch
            {
                File.Delete(Path.Combine(Application.persistentDataPath, "scores.json"));
            }
            if (!scores.ContainsKey(level))
                scores.Add(level, new PlayerScore[1]);
            PlayerScore[] newScores = new PlayerScore[scores[level].Length + 1];
            for(int i = 0; i < scores[level].Length; i++)
            {
                newScores[i] = scores[level][i];
            }
            newScores[newScores.Length - 1] = score;
            Sort(newScores, 0, newScores.Length - 1);
            if (newScores.Length > 10)
            {
                scores[level] = new PlayerScore[10];
                for (int i = 0; i < scores[level].Length; i++)
                    scores[level][i] = newScores[i];
            }
            else
                scores[level] = newScores;
        }
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "scores.json"), JsonConvert.SerializeObject(scores));
    }

    static void Sort(PlayerScore[] arr, int start, int end)
    {
        int i = start;
        int j = end;
        PlayerScore tmp;
        int sort_tmp;
        PlayerScore x = arr[(i + j) / 2];

        do
        {
            while (arr[i].highscore > x.highscore)
                i++;
            while (arr[j].highscore < x.highscore)
                j--;
            if (i <= j)
            {
                if (i < j)
                {

                    tmp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = tmp;
                }
                i++;
                j--;
            }
        } while (i <= j);

        if (i < end)
            Sort(arr, i, end);
        if (start < j)
            Sort(arr, start, j);
    }
}