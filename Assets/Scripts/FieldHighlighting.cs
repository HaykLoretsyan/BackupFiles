using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldHighlighting : MonoBehaviour {

	public static FieldHighlighting Instance;

    public GameObject highlightPrefab;
    private List<GameObject> highlights;

    void Awake()
    {
        Instance = this;
        highlights = new List<GameObject>();
    }

    public GameObject HighlightObject()
    {
        GameObject go = highlights.Find(g => !g.activeSelf);

        if(go == null)
        {
            go = Instantiate(highlightPrefab);
            highlights.Add(go);
        }

        return go;
    }

    public void HighlightAllowedMoves(bool[,] moves)
    {
        for(int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                if(moves[i,j])
                {
                    GameObject go = HighlightObject();
                    go.SetActive(true);
                    go.transform.position = main.Instance.GetTileCenterLight(i, j);
                }
            }
        }
    }

    public void HideHighlights()
    {
        foreach(GameObject go in highlights)
        {
            go.SetActive(false);
        }
    }
}
