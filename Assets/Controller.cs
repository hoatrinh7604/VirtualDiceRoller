using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class Controller : MonoBehaviour
{
    [SerializeField] GameObject inputField;
    [SerializeField] Button addButton;
    [SerializeField] Button continueButton;

    [SerializeField] TMP_Dropdown typeVolume;

    [SerializeField] GameObject[] listShape;

    [SerializeField] TMP_InputField amount;
    [SerializeField] TextMeshProUGUI sum;

    [SerializeField] GameObject dicePrefab;
    [SerializeField] Transform dicePrefabParent;
    [SerializeField] Sprite[] listSprites;


    [SerializeField] GameObject listFieldParent;

    [SerializeField] Button calButton;

    private List<GameObject> list = new List<GameObject>();
    private List<double> listInt = new List<double>();
    private double result = 0;

    public string CURRENCY_FORMAT = "#,##0.00";
    public NumberFormatInfo NFI = new NumberFormatInfo { NumberDecimalSeparator = ",", NumberGroupSeparator = "." };

    private int type = 0;

    [SerializeField] Color[] listColor;

    //Singleton
    public static Controller Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start()
    {
        Clear();
        //typeVolume.options.Clear();
        //List<string> items = new List<string>();

        //items.Add("2D");
        //items.Add("3D");

        //foreach(var item in items)
        //{
        //    typeVolume.options.Add(new TMP_Dropdown.OptionData() { text = item });
        //}

        //typeVolume.onValueChanged.AddListener(delegate { DropdownitemSelected(); });
        //typeVolume.value = 0;
        //type = 0;
        //listShape[0].SetActive(true);
    }

    private void DropdownitemSelected()
    {
        switch(typeVolume.options[typeVolume.value].text)
        {
            case "2D": type = 0;
                break; 
            case "3D": type = 1;
                break;
        }

        SwitchToVolume();
    }



    public void OnValueChanged()
    {
        if(CheckValidate())
        {
            calButton.interactable = true;
        }
        else
        {
            calButton.interactable= false;
        }
    }

    private bool CheckValidate()
    {
        if (amount.text == "")
            return false;

        int am = int.Parse(amount.text);

        if(am < 1 || am > 49)
        {
            return false;
        }
        //return text.All(char.IsDigit);
        return true;
    }


    public void Sum()
    {
        for (int i = 0; i < dicePrefabParent.childCount; i++)
        {
            Destroy(dicePrefabParent.GetChild(i).gameObject);
        }

        CalWithAdult();
        //listFieldParent.SetActive(true);
    }

    private void CalWithAdult()
    {
        sum.gameObject.SetActive(true);
        int am = int.Parse(amount.text);
        long sumation = 0;
        for(int i = 0; i < am; i++)
        {
            GameObject dice = Instantiate(dicePrefab, Vector2.zero, Quaternion.identity, dicePrefabParent);
            int random = UnityEngine.Random.Range(0, listSprites.Length);
            sumation += (random + 1);
            dice.GetComponent<Image>().sprite = listSprites[random];
        }

        sum.text = "Sum = " + sumation;
    }

    void SwitchToVolume()
    {
        for(int i = 0; i < listShape.Length; i++)
        {
            listShape[i].SetActive(i==type);
        }
    }

    double m2toft2 = 10.7639104;
    double m2toin2 = 1550.0031;


    double M2ToFt2(double m2)
    {
        return m2 * m2toft2;
    }
    
    double M2ToIn2(double m2)
    {
        return m2 * m2toin2;
    }

    public void Continue()
    {
        //if(amount==0) return;
        double currentResult = result;
        Clear();
        list[0].GetComponent<TMP_InputField>().text = currentResult.ToString();
        listInt[0] = currentResult;
    }

    public void Clear()
    {
        listFieldParent.SetActive(false);
        sum.gameObject.SetActive(false);
        typeVolume.value = 0;
        amount.text = "";

        calButton.interactable = false;

        for(int i = 0; i< dicePrefabParent.childCount; i++)
        {
            Destroy(dicePrefabParent.GetChild(i).gameObject);
        }
    }

    public void Quit()
    {
        Clear();
        Application.Quit();
    }
}
