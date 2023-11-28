using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CardGenerator : MonoBehaviour
{
    string fileName = "niveles-cartas.csv";
    //string filePath = "niveles-cartas.csv";
    //Prefab de la carta que se va a generar
    [SerializeField]
    private GameObject cardPrefab_;
    //Lista de sprites aleatorios que se van a cambiar
    [SerializeField]
    private Sprite[] sprites_;

    List<Carta> cartas = new List<Carta>();
    List<Carta> cartasFijas = new List<Carta>();

    private void Start()
    {
        string filePath = Path.Combine("Assets", "CSV", fileName);
        string[] lines = File.ReadAllLines(filePath);

        foreach (string line in lines)
        {
            string[] values = line.Split(',');

            Carta carta = new Carta
            {
                Tema = values[0],
                Nombre = values[1],
                Personaje = values[2],
                Pregunta = values[3],
                Condicion = values[4],
                // Utilizando int.TryParse para manejar la conversi�n a entero de Probabilidad
                Probabilidad = int.TryParse(values[5], out int probabilidad) ? probabilidad : 0,

                SobrescribirSi = values[6],

                // Utilizando int.TryParse para manejar la conversi�n a entero de SiDinero
                SiDinero = int.TryParse(values[7], out int siDinero) ? siDinero : 0,

                // Utilizando int.TryParse para manejar la conversi�n a entero de SiGente
                SiGente = int.TryParse(values[8], out int siGente) ? siGente : 0,

                // Utilizando int.TryParse para manejar la conversi�n a entero de SiFlora
                SiFlora = int.TryParse(values[9], out int siFlora) ? siFlora : 0,

                // Utilizando int.TryParse para manejar la conversi�n a entero de SiFauna
                SiFauna = int.TryParse(values[10], out int siFauna) ? siFauna : 0,

                // Utilizando int.TryParse para manejar la conversi�n a entero de SiAire
                SiAire = int.TryParse(values[11], out int siAire) ? siAire : 0,

                ExtrasSi = values[12],
                SobrescribeNo = values[13],

                // Utilizando int.TryParse para manejar la conversi�n a entero de NoDinero
                NoDinero = int.TryParse(values[14], out int noDinero) ? noDinero : 0,

                // Utilizando int.TryParse para manejar la conversi�n a entero de NoGente
                NoGente = int.TryParse(values[15], out int noGente) ? noGente : 0,

                // Utilizando int.TryParse para manejar la conversi�n a entero de NoFlora
                NoFlora = int.TryParse(values[16], out int noFlora) ? noFlora : 0,

                // Utilizando int.TryParse para manejar la conversi�n a entero de NoFauna
                NoFauna = int.TryParse(values[17], out int noFauna) ? noFauna : 0,

                // Utilizando int.TryParse para manejar la conversi�n a entero de NoAire
                NoAire = int.TryParse(values[18], out int noAire) ? noAire : 0,

                ExtrasNo = values[19],
                TextoExplicativo = values[20]
            };
            Debug.Log("Hola");
            cartas_.Add(carta);
            numCartas++;
        }
        cartasFijas = cartas.FindAll(carta => carta.Condicion == "Evento fijo");
    }

    //Instancia la nueva carta que estar� detr�s del mazo
    //Le pone como primer hijo y cambia su sprite por un sprite aleatorio
    public void InstantiateCard()
    {
        GameObject newCard = Instantiate(cardPrefab_, transform, false);
        newCard.transform.SetAsFirstSibling();
        int num = Random.Range(0, cartas.Count());

        Carta playerCard = newCard.GetComponent<Carta>();

        playerCard.Tema = cartas_[num].Tema;
        playerCard.Nombre = cartas_[num].Nombre;
        playerCard.Personaje = cartas_[num].Personaje;
        playerCard.Pregunta = cartas_[num].Pregunta;
        playerCard.Condicion = cartas_[num].Condicion;
        playerCard.Probabilidad = cartas_[num].Probabilidad;
        playerCard.SobrescribirSi = cartas_[num].SobrescribirSi;
        playerCard.SiDinero = cartas_[num].SiDinero;
        playerCard.SiGente = cartas_[num].SiGente;
        playerCard.SiFlora = cartas_[num].SiFlora;
        playerCard.SiFauna = cartas_[num].SiFauna;
        playerCard.SiAire = cartas_[num].SiAire;
        playerCard.ExtrasSi = cartas_[num].ExtrasSi;
        playerCard.SobrescribeNo = cartas_[num].SobrescribeNo;
        playerCard.NoDinero = cartas_[num].NoDinero;
        playerCard.NoGente = cartas_[num].NoGente;
        playerCard.NoFlora = cartas_[num].NoFlora;
        playerCard.NoFauna = cartas_[num].NoFauna;
        playerCard.NoAire = cartas_[num].NoAire;
        playerCard.ExtrasNo = cartas_[num].ExtrasNo;
        playerCard.TextoExplicativo = cartas_[num].TextoExplicativo;

        if (playerCard.SobrescribeNo == "")
            playerCard.SobrescribeNo = "No";

        if (playerCard.SobrescribirSi == "")
            playerCard.SobrescribeNo = "Si";

        if (cartas_[num].Personaje == "Faustino el agricultor")
        {
            newCard.GetComponent<Image>().sprite = sprites_[1];
        }
        else if(cartas_[num].Personaje == "Toni el activista")
        {
            newCard.GetComponent<Image>().sprite = sprites_[0];
        }
        else
        {
            newCard.GetComponent<Image>().sprite = sprites_[2];
        }
    }

    public int[] GenerateFixedCardsSelection()
    {
        int[] fixedCardsSelection = new int[7];
        fixedCardsSelection[0] = SelectRandomFixedCard("Agricultura");
        fixedCardsSelection[1] = SelectRandomFixedCard("Deforestaci�n"); ;
        fixedCardsSelection[2] = SelectRandomFixedCard("Energ�a e�lica"); ;
        fixedCardsSelection[3] = SelectRandomFixedCard("Fabrica/Economia");
        fixedCardsSelection[4] = SelectRandomFixedCard("Ganaderia");
        fixedCardsSelection[5] = SelectRandomFixedCard("Energ�a Solar");
        fixedCardsSelection[6] = SelectRandomFixedCard("Prevenci�n de incendios");

        System.Random rng = new System.Random();
        int n = fixedCardsSelection.Length;

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int value = fixedCardsSelection[k];
            fixedCardsSelection[k] = fixedCardsSelection[n];
            fixedCardsSelection[n] = value;
        }

        return fixedCardsSelection;
    }

    int SelectRandomFixedCard(string condicion)
    {
        List<Carta> cartasFiltradas = cartasFijas.FindAll(carta => carta.Tema == condicion);

        int indiceAleatorio = UnityEngine.Random.Range(0, cartasFiltradas.Count);
        return cartasFiltradas[indiceAleatorio].CardId;
    }

    int SelectRandomCard()
    {
        List<Carta> cartasNoFijas = cartas.FindAll(carta => carta.Condicion != "Evento fijo");

        int indiceAleatorio = -1;
        do
        {
            indiceAleatorio = Random.Range(0, cartasNoFijas.Count);
        } while (!cartas[indiceAleatorio].Usada);

        return cartasNoFijas[indiceAleatorio].CardId;
    }

    void DiscardUsedCard(int id)
    {
        cartas[id].Usada = true;
    }
}