using System;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;

    private void Awake()
    {
        entryContainer = transform.Find("hightScoreContainer");
        entryTemplate = entryContainer.Find("hightScoreTemplate");

        entryTemplate.gameObject.SetActive(false);
    }

    public void ChooseLevel(int level)
    {
        DeleteClones();
        float templateHeight = 20f;
        GameScores.PlayerScore[] scores = GameScores.GetScores(level);
        if (scores == null) return;
        else
        {
            for (int i = 0; i < scores.Length; i++)
            {
                
                Transform entryTransform = Instantiate(entryTemplate, entryContainer);
                RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
                entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
                entryTransform.gameObject.SetActive(true);

                entryTransform.Find("name").GetComponent<TMPro.TextMeshProUGUI>().text = scores[i].name;
                entryTransform.Find("score").GetComponent<TMPro.TextMeshProUGUI>().text = scores[i].highscore.ToString().ToUpper();
            }
        }
    }

    private void DeleteClones()
    {
        for (int i = 1; i< entryContainer.childCount; i++)
        {
            Transform templateClone = entryContainer.GetChild(i);
            Destroy(templateClone.gameObject);   
        }
    }


}

