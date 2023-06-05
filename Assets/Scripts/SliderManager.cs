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
                GameObject modelObj = model_;
                modelObj.transform.localScale = initialModelScale + new Vector3(v, v, v);
                if(modelObj.transform.localScale.x < 0){
                    modelObj.transform.localScale = Vector3.zero;
                }
            });
        }

        if(this.gameObject.name == "rotate"){
            slider_.onValueChanged.AddListener((v) => {
                GameObject modelObj = model_;
                modelObj.transform.rotation = initialModelRotation*Quaternion.Euler(v, 0, 0);
            });
        }
    }

    void Update()
    {
        // if(this.gameObject.name == "rotate"){
        //     this.model_.transform.rotation = this.model_.transform.rotation*Quaternion.Euler(1, 0, 0);
        // }
        // Debug.Log(this.model_.transform.rotation);

        // modelScale.text = model_.transform.localScale.ToString("0.00") + 
        //                   model_.transform.rotation.ToString("") + model_.name + 
        //                   model_.transform.position.ToString("0.00");
    }

    public void initModel(GameObject model){
        model_ = model;
        initialModelScale = model_.transform.localScale;
        initialModelRotation = model_.transform.rotation;
    }
}
