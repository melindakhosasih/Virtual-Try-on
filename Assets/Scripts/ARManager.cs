using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ARManager : MonoBehaviour
{
    public GameObject[] prefabs;

    public GameObject btnObj;
    public GameObject btnTextObj;

    private Button btn;
    private Text btnText;
    private Vector3[] localScale_ = {
        Vector3.zero,
        Vector3.zero,
        Vector3.zero,
        Vector3.zero,
        Vector3.zero,
        Vector3.zero,
        Vector3.zero,
        Vector3.zero,
        Vector3.zero,
        Vector3.zero
    };
    private Vector3[] localPosition_ = {
        Vector3.zero,
        Vector3.zero,
        Vector3.zero,
        Vector3.zero,
        Vector3.zero,
        Vector3.zero,
        Vector3.zero,
        Vector3.zero,
        Vector3.zero,
        Vector3.zero
    };

    [SerializeField] SliderManager sliderScaleManager_;
    [SerializeField] SliderManager sliderRotateManager_;
    [SerializeField] private Text modelScale;

    private int currentModelNum = 1;
    private GameObject model_;

    ARTrackedImageManager aRTrackedImageManager;
    Dictionary<string, GameObject> images2prefabs = new Dictionary<string, GameObject>();

    void Start(){
        
    }

    private void Awake() {
        foreach(GameObject prefab in prefabs) {
            GameObject _prefab = Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);
            _prefab.name = prefab.name;
            _prefab.SetActive(false);
            images2prefabs.Add(_prefab.name, _prefab);
            localScale_[currentModelNum-1] = prefab.transform.localScale;
            localPosition_[currentModelNum-1] = prefab.transform.position;
            currentModelNum += 1;
        }
        currentModelNum = 1;

        btn = btnObj.GetComponent<Button>();
        btnText = btnTextObj.GetComponent<Text>();
        btn.onClick.AddListener(()=>{
            images2prefabs["watch"+currentModelNum.ToString()].SetActive(false);
            currentModelNum += 1;
            if(currentModelNum>2){
                currentModelNum = 1;
            }
            sliderScaleManager_.initModel(images2prefabs["watch"+currentModelNum.ToString()]);
            sliderRotateManager_.initModel(images2prefabs["watch"+currentModelNum.ToString()]);
            btnText.text = currentModelNum.ToString();
        });

        aRTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        sliderScaleManager_.initModel(images2prefabs["watch"+currentModelNum.ToString()]);
        sliderRotateManager_.initModel(images2prefabs["watch"+currentModelNum.ToString()]);

    }

    private void OnEnable() {
        aRTrackedImageManager.trackedImagesChanged += ImagesChanged;
    }

    private void OnDisable() {
        aRTrackedImageManager.trackedImagesChanged -= ImagesChanged;
    }

    private void ImagesChanged(ARTrackedImagesChangedEventArgs args) {
        foreach(ARTrackedImage trackedImage in args.added){
            TrackedImageUpdate(trackedImage, "added");
        }
        foreach(ARTrackedImage trackedImage in args.updated){
            TrackedImageUpdate(trackedImage, "updated");
        }
        foreach(ARTrackedImage trackedImage in args.removed){
            TrackedImageRemove(trackedImage);
        }
    }

    private void TrackedImageUpdate(ARTrackedImage image, string state) {
            // GameObject _prefab = images2prefabs[image.referenceImage.name+currentModelNum.ToString()];
            // _prefab.transform.position = image.transform.position;
            images2prefabs["watch"+currentModelNum.ToString()].transform.localScale = images2prefabs["watch"+currentModelNum.ToString()].transform.localScale;
            images2prefabs["watch"+currentModelNum.ToString()].transform.position = image.transform.position;
            // images2prefabs["watch"+currentModelNum.ToString()].transform.position = image.transform.position + localPosition_[currentModelNum-1];
            if(state == "added"){
                images2prefabs["watch"+currentModelNum.ToString()].transform.rotation = images2prefabs["watch"+currentModelNum.ToString()].transform.rotation;
            }
            // _prefab.transform.rotation = Quaternion.identity*_prefab.transform.rotation;
            images2prefabs["watch"+currentModelNum.ToString()].SetActive(true);
            // btnText.text = btnText.text + _prefab.transform.position;
    }

    private void TrackedImageRemove(ARTrackedImage image) {
        images2prefabs["watch"+currentModelNum.ToString()].SetActive(false);
    }

    void Update(){
        model_ = images2prefabs["watch"+currentModelNum.ToString()];
        modelScale.text = model_.transform.localScale.ToString("0.00") + '\n' +
                          model_.transform.rotation.ToString("") + '\n' + 
                          " watch" + currentModelNum.ToString() + '\n' +
                          model_.transform.position.ToString("0.00") + '\n' +
                          localPosition_[currentModelNum-1].ToString("0.00");
    }
}
