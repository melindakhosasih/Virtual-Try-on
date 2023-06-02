using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking : MonoBehaviour
{
    public GameObject[] prefabs;

    ARTrackedImageManager aRTrackedImageManager;
    Dictionary<string, GameObject> images2prefabs = new Dictionary<string, GameObject>();

    private void Awake() {
        aRTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        foreach(GameObject prefab in prefabs) {
            GameObject _prefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            _prefab.name = prefab.name;
            _prefab.SetActive(false);
            images2prefabs.Add(_prefab.name, _prefab);
        }
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
        GameObject _prefab = images2prefabs[image.referenceImage.name];
        _prefab.transform.position = image.transform.position;
        _prefab.SetActive(true);
    }

    private void TrackedImageRemove(ARTrackedImage image) {
        images2prefabs[image.name].SetActive(false);
    }
}
