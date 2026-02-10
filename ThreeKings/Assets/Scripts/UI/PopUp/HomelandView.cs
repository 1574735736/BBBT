using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomelandView : UIAction
{
    private GameObject I_Reset;
    private GameObject I_Books;
    private GameObject I_Practice;
    private GameObject I_Citta;
    private GameObject I_CittaBook;
    private GameObject I_Experience;
    private GameObject I_Gold;
    private GameObject I_BuyHouse;
    private GameObject I_GetAward;
    void Start()
    {
        I_Reset = this.gameObject.GetDeepGameObject("I_Reset");
        I_Books = this.gameObject.GetDeepGameObject("I_Books");
        I_Practice = this.gameObject.GetDeepGameObject("I_Practice");
        I_Citta = this.gameObject.GetDeepGameObject("I_Citta");
        I_CittaBook = this.gameObject.GetDeepGameObject("I_CittaBook");
        I_Experience = this.gameObject.GetDeepGameObject("I_Experience");
        I_Gold = this.gameObject.GetDeepGameObject("I_Gold");
        I_BuyHouse = this.gameObject.GetDeepGameObject("I_BuyHouse");
        I_GetAward = this.gameObject.GetDeepGameObject("I_GetAward");

        this.transform.SetDeepButtonAction("B_Reset", () =>
        {
            I_Reset.SetActive(true);
        });
        this.transform.SetDeepButtonAction("B_Books", () =>
        {
            I_Books.SetActive(true);
        });
        this.transform.SetDeepButtonAction("B_Practice", () =>
        {
            I_Practice.SetActive(true);
        });
        this.transform.SetDeepButtonAction("B_Citta", () =>
        {
            I_Citta.SetActive(true);
        });
        this.transform.SetDeepButtonAction("B_Experience", () =>
        {
            I_Experience.SetActive(true);
        });
        this.transform.SetDeepButtonAction("B_Gold", () =>
        {
            I_Gold.SetActive(true);
        });
        this.transform.SetDeepButtonAction("B_BuyHouse", () =>
        {
            I_BuyHouse.SetActive(true);
        });
        this.transform.SetDeepButtonAction("I_Reset", () =>
        {
            I_Reset.SetActive(false);
        });
        this.transform.SetDeepButtonAction("I_Books", () =>
        {
            I_Books.SetActive(false);
        });
        this.transform.SetDeepButtonAction("I_Practice", () =>
        {
            I_Practice.SetActive(false);
        });
        this.transform.SetDeepButtonAction("I_Citta", () =>
        {
            I_Citta.SetActive(false);
        });
        this.transform.SetDeepButtonAction("I_CittaBook", () =>
        {
            I_CittaBook.SetActive(false);
        });
        this.transform.SetDeepButtonAction("I_Experience", () =>
        {
            I_Experience.SetActive(false);
        });
        this.transform.SetDeepButtonAction("I_Gold", () =>
        {
            I_Gold.SetActive(false);
        });
        this.transform.SetDeepButtonAction("I_BuyHouse", () =>
        {
            I_BuyHouse.SetActive(false);
        });
        this.transform.SetDeepButtonAction("I_GetAward", () =>
        {
            I_GetAward.SetActive(false);
        });

    }

   
}
