using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wos2Item : MonoBehaviour
{
    public int id = 0;
    public Image parent;
    public Image child;
    public bool solved = false;
    // Start is called before the first frame update
    public void SetParentAndChild(Sprite _parent, Sprite _child)
    {
        parent.sprite = _parent;
        child.sprite = _child;
    }
    public void SetParentOnly(Sprite _parent)
    {
        parent.sprite = _parent;
        child.sprite = null;
        child.gameObject.SetActive(false);


    }
    public void turnMyParentOn(Sprite _parent)
    {
        parent.gameObject.SetActive(true);
        parent.sprite = _parent;
        SetSolved();



    }
    public void SetChildOnly(Sprite _child)
    {
        child.sprite = _child;
        // parent.sprite = null;
        parent.gameObject.SetActive(false);

    }
    public bool isSolved(
    )
    {
        if (solved)
        {
            return true;
        }
        return false;

    }
    public void SetSolved()
    {
        // gameObject.Layer = LayerMask.NameToLayer("Ui");
        gameObject.layer = LayerMask.NameToLayer("UI");
        solved = true;
    }
    // Update is called once per frame

}
