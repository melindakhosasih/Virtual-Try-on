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

    [SerializeField] SliderManager sliderManager_;

    private int currentModelNum = 1;

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
            sliderManager_.initModel(images2prefabs["watch"+currentModelNum.ToString()]);
            btnText.text = currentModelNum.ToString();
        });

        aRTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        sliderManager_.initModel(images2prefabs["watch"+currentModelNum.ToString()]);

    }

    private void OnEnable() {
        aRTrackedImageManager.trackedImagesChanged += ImagesChanged;
    }

    private void OnDisable() {
        aRTrackedImageManager.trackedImagesChanged -= ImagesChanged;
    }

    private void ImagesChanged(ARTrackedImagesChangedEventArgs args) {
        foreach(ARTrackedImage trackedImage in args.added){
            TrackedImageUpdate(trackedImage);
        }
        foreach(ARTrackedImage trackedImage in args.updated){
            TrackedImageUpdate(trackedImage);
        }
        foreach(ARTrackedImage trackedImage in args.removed){
            TrackedImageRemove(trackedImage);
        }
    }

    private void TrackedImageUpdate(ARTrackedImage image) {
        GameObject _prefab = images2prefabs[image.referenceImage.name+currentModelNum.ToString()];
        _prefab.transform.position = image.transform.position;
        _prefab.transform.localScale = _prefab.transform.localScale;
        // _prefab.transform.position = image.transform.position + new Vector3(0, _prefab.transform.position.y, _prefab.transform.position.z);
        // _prefab.transform.rotation = Quaternion.identity*_prefab.transform.rotation;
        _prefab.transform.rotation = _prefab.transform.rotation;
        _prefab.SetActive(true);
        // btnText.text = btnText.text + _prefab.transform.position;
    }

    private void TrackedImageRemove(ARTrackedImage image) {
        images2prefabs[image.name+currentModelNum].SetActive(false);
    }
}
