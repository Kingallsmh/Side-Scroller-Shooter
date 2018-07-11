using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ColorControl : MonoBehaviour {

    public List<ColorControl> linkedObjects;

    //Test variable
    public int EnvironColorNum = 0;
    public Color currentColor = new Color(1, 1, 1);
    public Color secondColor = new Color(0, 0, 0);
    SpriteRenderer myRend;
    public float timeToLerp = 2, maxLerpTime = 1;

    // Use this for initialization
    void Start() {
        timeToLerp = maxLerpTime;
        myRend = GetComponent<SpriteRenderer>();
        StartCoroutine(UpdateLoop());
    }

    private IEnumerator UpdateLoop() {
        while (this) {
            yield return StartCoroutine(LerpColor(currentColor, secondColor, myRend));
            yield return StartCoroutine(LerpColor(secondColor, currentColor, myRend));
        }
    }

    public void LerpColorLinked(Color color) {
        for (int i = 0; i < linkedObjects.Count; i++) {
            int num = linkedObjects[i].transform.childCount;
            if (num > 0) {
                linkedObjects[i].LerpUpdateChildrenColor(color);
            }
            else {
                linkedObjects[i].LerpUpdateColor(color, linkedObjects[i].gameObject);
            }
        }
    }

    public void ColorLinked(Color color) {
        for (int i = 0; i < linkedObjects.Count; i++) {
            int num = linkedObjects[i].transform.childCount;
            if (num > 0) {
                linkedObjects[i].UpdateChildrenColor(color);
            }
            else {
                linkedObjects[i].UpdateColor(color, linkedObjects[i].gameObject);
            }
        }
    }

    public void UpdateColor(Color color, GameObject toColor) {
        SpriteRenderer rend = toColor.GetComponent<SpriteRenderer>();
        Tilemap tileRend = toColor.GetComponent<Tilemap>();
        if (rend) {
            currentColor = color;
            rend.color = color;
        }
        else if (tileRend) {
            currentColor = color;
            tileRend.color = color;
        }
    }

    public void LerpUpdateColor(Color color, GameObject toColor) {
        SpriteRenderer rend = toColor.GetComponent<SpriteRenderer>();
        Tilemap tileRend = toColor.GetComponent<Tilemap>();
        if (rend) {
            StartCoroutine(LerpColor(rend.color, color, rend));
            currentColor = color;
        }
        else if (tileRend) {
            StartCoroutine(LerpColor(tileRend.color, color, tileRend));
            currentColor = color;
        }
    }

    IEnumerator LerpColor(Color currentC, Color newC, SpriteRenderer rend) {
        float progress = 0;
        while (progress <= 1) {
            rend.color = Color.Lerp(currentC, newC, progress);
            progress += Time.deltaTime / timeToLerp;
            yield return null;
        }
    }

    IEnumerator LerpColor(Color currentC, Color newC, Tilemap tileRend) {
        float progress = 0;
        while (progress <= 1) {
            tileRend.color = Color.Lerp(currentC, newC, progress);
            progress += Time.deltaTime / timeToLerp;
            yield return null;
        }
    }

    public void UpdateChildrenColor(Color color) {
        int num = transform.childCount;
        for (int i = 0; i < num; i++) {
            UpdateColor(color, transform.GetChild(i).gameObject);
        }
    }

    public void LerpUpdateChildrenColor(Color color) {
        int num = transform.childCount;
        for (int i = 0; i < num; i++) {
            LerpUpdateColor(color, transform.GetChild(i).gameObject);
        }
    }

    public void UpdateColorFromDamage(float percent) {
        float red = 0;
        float green = 1;
        if(percent > 0.5f) {
            red = Mathf.Clamp(0.1f + (1-percent)*2, 0, 1);
        }
        else {
            red = 1;
            green = Mathf.Clamp((percent * 2) - 0.1f, 0, 1);
        }
        Color newColor = new Color(red, green, 0);
        currentColor = newColor;
        timeToLerp = maxLerpTime * percent;
    }
}