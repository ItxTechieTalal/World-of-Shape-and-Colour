using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class ItemManagerWorldV2 : MonoBehaviour
{
    // public List<WosV2> wosV2s = new List<WosV2>();
    public List<Sprite> parents = new List<Sprite>();
    public List<Sprite> children = new List<Sprite>();
    public static ItemManagerWorldV2 instance;

    private void Awake()
    {
        instance = this;
    }

    // public 
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
[System.Serializable]
public class WosV2
{
    // ==  core + frame
    public Sprite core;
    public Sprite frame;
}
