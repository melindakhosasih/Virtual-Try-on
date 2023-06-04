using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    [SerializeField] private GameObject model_;
    [SerializeField] private Slider slider_;
    [SerializeField] private Text modelScale;

    private Vector3 initialModelScale;

    void Start(){
        initialModelScale = model_.transform.localScale;

        slider_.onValueChanged.AddListener((v) => {
            model_.transform.localScale = initialModelScale + new Vector3(v, v, v);
            // modelScale.text = model_.transform.localScale.ToString("0.00") + 
            //                     model_.transform.rotation.ToString("0.00");
            if(model_.transform.localScale.x < 0){
                model_.transform.localScale = Vector3.zero;
            }
        });
    }

    void Update()
    {
        
    }

    public void initModel(GameObject model){
        this.model_ = model;
    }
}
