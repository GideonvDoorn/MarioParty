using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Resource))]

public class ResourceUI : MonoBehaviour {

    public Text Value;
    public Text Label;

    private Resource resource;

    private void Awake()
    {
        resource = GetComponent<Resource>();
    }

    void Start()
    {

        UpdateUI();
    }

    public void UpdateUI()
    {
        Label.text = resource.name;
        Value.text = resource.Amount.ToString();
    }

}
