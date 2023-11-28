using System.Collections;
using TMPro;
using UnityEngine;

public class TextAnimator : MonoBehaviour
{
    public float delayBetweenLetters = 0.05f;  // Tiempo de espera entre letras
    private TextMeshProUGUI textMeshPro;
    [SerializeField]
    string text;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        StartCoroutine(AnimateText(text));
    }

    IEnumerator AnimateText(string text)
    {
        textMeshPro.text = "";
        foreach (char c in text)
        {
            textMeshPro.text += c;
            yield return new WaitForSeconds(delayBetweenLetters);
        }
    }
}