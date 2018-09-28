using System.Collections;
using UnityEngine;

public class ColorChanger : MonoBehaviour {

    SpriteRenderer ChessBoardSR;
    public Color nextColor;
    public Color lastColor;

    void Awake() {
        ChessBoardSR = gameObject.GetComponent<SpriteRenderer>();
        nextColor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0.5f, 1f));
        lastColor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0.5f, 1f));
        changeInteval = Random.Range(0.1f, 5f);
    }

    void Start() {
        StartCoroutine(produceNextColor());
        StartCoroutine(changeColor());
    }

    void Update() {

    }

    float changeInteval = 3f;
    IEnumerator produceNextColor() {
        while (true) {
            lastColor = nextColor;
            nextColor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0.5f, 1f));
            changeTicker = 0;
            yield return new WaitForSeconds(changeInteval);
        }
    }

    float showInteval = 0.05f;
    float changeTicker = 0;
    IEnumerator changeColor() {
        while (true) {
            changeTicker += showInteval;
            float ratio = changeTicker / changeInteval;
            ChessBoardSR.color = new Color(ratio * (nextColor.r - lastColor.r) + lastColor.r, ratio * (nextColor.g - lastColor.g) + lastColor.g, ratio * (nextColor.b - lastColor.b) + lastColor.b, ratio * (nextColor.a - lastColor.a) + lastColor.a);
            yield return new WaitForSeconds(showInteval);
        }
    }
}