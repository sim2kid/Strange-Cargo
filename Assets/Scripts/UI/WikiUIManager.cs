using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WikiUIManager : MonoBehaviour
{

    public GameObject HomePanel;
    public GameObject CarePanel;
    public GameObject AccesoriesPanel;
    public GameObject PaymentPanel;
    public GameObject HelpPanel;

    public void openHome()
    {
        HomePanel.SetActive(true);
        CarePanel.SetActive(false);
        AccesoriesPanel.SetActive(false);
        PaymentPanel.SetActive(false);
        HelpPanel.SetActive(false);
    }

    public void openCare()
    {
        HomePanel.SetActive(false);
        CarePanel.SetActive(true);
        AccesoriesPanel.SetActive(false);
        PaymentPanel.SetActive(false);
        HelpPanel.SetActive(false);

    }

    public void openAccesories()
    {
        HomePanel.SetActive(false);
        CarePanel.SetActive(false);
        AccesoriesPanel.SetActive(true);
        PaymentPanel.SetActive(false);
        HelpPanel.SetActive(false);

    }

    public void openPayment()
    {
        HomePanel.SetActive(false);
        CarePanel.SetActive(false);
        AccesoriesPanel.SetActive(false);
        PaymentPanel.SetActive(true);
        HelpPanel.SetActive(false);

    }

    public void openHelp()
    {
        HomePanel.SetActive(false);
        CarePanel.SetActive(false);
        AccesoriesPanel.SetActive(false);
        PaymentPanel.SetActive(false);
        HelpPanel.SetActive(true);

    }

    void onEnable()
    {
        HomePanel.SetActive(true);
        CarePanel.SetActive(false);
        AccesoriesPanel.SetActive(false);
        PaymentPanel.SetActive(false);
        HelpPanel.SetActive(false);
    }
}
