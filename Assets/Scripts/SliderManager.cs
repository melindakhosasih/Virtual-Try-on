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
    private Quaternion initialModelRotation;

    void Start(){
        initialModelScale = model_.transform.localScale;
        initialModelRotation = model_.transform.rotation;

        if(this.gameObject.name == "scale"){
            slider_.onValueChanged.AddListener((v) => {
                GameObject modelObj = this.model_;
                modelObj.transform.localScale = initialModelScale + new Vector3(v, v, v);
                modelScale.text = modelObj.transform.localScale.ToString("0.00") + 
                                    modelObj.transform.rotation.ToString("000.00");
                if(modelObj.transform.localScale.x < 0){
                    modelObj.transform.localScale = Vector3.zero;
                }
            });
        }

        if(this.gameObject.name == "rotate"){
            slider_.onValueChanged.AddListener((v) => {
                GameObject modelObj = this.model_;
                modelScale.text = modelObj.transform.localScale.ToString("0.00") + 
                                    modelObj.transform.rotation.ToString("000.00");
                modelObj.transform.rotation = initialModelRotation*Quaternion.Euler(v, 0, 0);
            });
        }
    }

    void Update()
    {
        modelScale.text = model_.transform.localScale.ToString("0.00") + 
                                    model_.transform.rotation.ToString("000.00") + model_.name;
    }

    public void initModel(GameObject model){
        this.model_ = model;
        initialModelScale = model_.transform.localScale;
        initialModelRotation = model_.transform.rotation;

        // if(this.gameObject.name == "scale"){
        //     slider_.onValueChanged.AddListener((v) => {
        //         model_.transform.localScale = initialModelScale + new Vector3(v, v, v);
        //         modelScale.text = model_.transform.localScale.ToString("0.00") + 
        //                             model_.transform.rotation.ToString("000.00");
        //         if(model_.transform.localScale.x < 0){
        //             model_.transform.localScale = Vector3.zero;
        //         }
        //     });
        // }

        // if(this.gameObject.name == "rotate"){
        //     slider_.onValueChanged.AddListener((v) => {
        //         modelScale.text = model_.transform.localScale.ToString("0.00") + 
        //                             model_.transform.rotation.ToString("000.00");
        //         model_.transform.rotation = initialModelRotation*Quaternion.Euler(v, 0, 0);
        //     });
        // }
    }
}
