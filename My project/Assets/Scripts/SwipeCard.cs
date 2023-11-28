using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SwipeCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    GameObject text, canvas; 
    //Distancia que se mueve la carta para que desaparezca
    private const float distanceDragged = 0.15f;
    //Guarda la posici�n inicial
    private Vector3 iniPos_;
    //Guarda la distancia movida desde la iniPos_ hasta la distancia movida con el click en el OnDrag
    private float distancedMoved_;
    //Booleano para saber si es hacia un lado u  otro, es decir izquierda o derecha
    private bool swipeLeft_;
    //Sprite p�blico al que se le pasa la referencia desde secondImage
    [HideInInspector]
    public Sprite frontSprite;
    //Evento que suceder� al hacer el volteo de la carta y har� llamar a los m�todos suscritos a �l
    public event Action cardMoved;
    //Guarda la posInicial
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).GetComponent<TMPro.TextMeshProUGUI>())
                text = transform.GetChild(i).gameObject;
            else
                canvas = transform.GetChild(i).gameObject;
        }
        iniPos_ = transform.position;
    }
    //Movemos la posici�n y calculamos la diferencia entre la posX actual y la original
    //Dependiendo de esa diferencia rotar� m�s o menos y dependiendo el lado, a la izquierda o a la derecha
    public void OnDrag(PointerEventData eventData)
    {
        transform.localPosition = new Vector2(transform.localPosition.x + eventData.delta.x, transform.localPosition.y);
    
        if(transform.localPosition.x - iniPos_.x > 0)
        {
            transform.localEulerAngles = new Vector3(0, 0,
                Mathf.LerpAngle(0, -30, (iniPos_.x + transform.localPosition.x) / (Screen.width / 2)));
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, 0,
               Mathf.LerpAngle(0, +30, (iniPos_.x - transform.localPosition.x) / (Screen.width / 2)));
        }

        distancedMoved_ = Mathf.Abs(transform.localPosition.x - iniPos_.x);

        if (distancedMoved_ < distanceDragged * Screen.width)
        {
            text.SetActive(false);
            canvas.SetActive(false);
        }
        else
        {
            text.SetActive(true);
            canvas.SetActive(true);

            if (transform.localPosition.x > iniPos_.x)        
                text.GetComponent<TMPro.TextMeshProUGUI>().text = GetComponent<Carta>().SobrescribirSi;
            else
                text.GetComponent<TMPro.TextMeshProUGUI>().text = GetComponent<Carta>().SobrescribeNo;
        }
    }
    //Cuando empieza el drag, es decir el click sobre la imagen y movimiento guardamos la posOriginal
    public void OnBeginDrag(PointerEventData eventData)
    {
        iniPos_ = transform.localPosition;
    }
    //Al terminar el drag si se ha movido m�s de la mitad de la pantalla * 0.3
    //Llama al invoke de cardMoved, sino vueve a la posici�n inicial y con la rotaci�n actual
    public void OnEndDrag(PointerEventData eventData)
    {
        float time = 0;

        Color textColor = text.GetComponent<TMPro.TextMeshProUGUI>().color;
        Color canvasColor = canvas.GetComponent<Image>().color;

        distancedMoved_ = Mathf.Abs(transform.localPosition.x - iniPos_.x);
        if(distancedMoved_ < distanceDragged * Screen.width)
        {
            transform.localPosition = iniPos_;
            transform.eulerAngles = Vector3.zero;
        }
        else
        {
            if (transform.localPosition.x > iniPos_.x)
                swipeLeft_ = false;
            else
                swipeLeft_ = true;

            cardMoved?.Invoke();
            StartCoroutine(MovedCard());
        }
    }
    //Al haber movido la carta, hace scroll hacia un lateral y un fadeout, al terminar
    private IEnumerator MovedCard()
    {
        float time = 0;
        
        Color textColor = text.GetComponent<TMPro.TextMeshProUGUI>().color;
        Color canvasColor = canvas.GetComponent<Image>().color;

        while (GetComponent<Image>().color != new Color(1, 1, 1, 0))
        {
            time += Time.deltaTime;
            if (swipeLeft_)
            {
                transform.localPosition = new Vector3(Mathf.SmoothStep(transform.localPosition.x, 
                    transform.localPosition.x - 10, time), transform.localPosition.y, 0);
            }
            else
            {
                transform.localPosition = new Vector3(Mathf.SmoothStep(transform.localPosition.x,
                    transform.localPosition.x + 10, time), transform.localPosition.y, 0);
            }
            GetComponent<Image>().color = new Color(1, 1, 1, Mathf.SmoothStep(1, 0, 4 * time));
            text.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(textColor.r, textColor.g, textColor.b, Mathf.SmoothStep(1, 0, 4 * time));
            canvas.GetComponent<Image>().color = new Color(canvasColor.r, canvasColor.g, canvasColor.b, Mathf.SmoothStep(1, 0, 4 * time));
            yield return null;
        }
        Destroy(gameObject);
    }
    //Cambia el sprite por el frontal desde un evento en el animator
    public void changeSprite() {
        GetComponent<Image>().sprite = frontSprite;
    }
    //Desactiva el animator desde un evento en el animator
    public void disableAnimator()
    {
        GetComponent<Animator>().enabled = false;
    }
    //public void FadeIn()
    //{
    //    LeanTween.value(gameObject, UpdateAlpha, 0f, 1f, 0.5f);
    //}

    //public void FadeOut()
    //{
    //    LeanTween.value(gameObject, UpdateAlpha, 1f, 0f, 0.5f);
    //}

    //private void UpdateAlpha(float alpha)
    //{
    //    canvasGroup.alpha = alpha;
    //}
}