using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PrefabCreator : MonoBehaviour
{
    [SerializeField] private GameObject dragonPrefab;
    [SerializeField] private Vector3 prefabOffset; // Pastikan ini (0,0,0) dulu di Inspector

    private GameObject dragon;
    private ARTrackedImageManager m_TrackedImageManager;

    private void Awake() {
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable() {
        if (m_TrackedImageManager != null) {
            m_TrackedImageManager.trackedImagesChanged += OnImageChanged;
        }
    }

    private void OnDisable() {
        if (m_TrackedImageManager != null) {
            m_TrackedImageManager.trackedImagesChanged -= OnImageChanged;
        }
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs obj) {
        // 1. Cek saat gambar BARU terdeteksi
        foreach (ARTrackedImage image in obj.added) {
            Debug.Log("GAMBAR DITEMUKAN: " + image.referenceImage.name); // Cek Logcat!
            
            // Spawn naga
            dragon = Instantiate(dragonPrefab, image.transform);
            
            // PERBAIKAN PENTING: Gunakan localPosition agar nempel pas di atas gambar
            dragon.transform.localPosition = prefabOffset; 
            
            // Opsional: Paksa rotasi agar menghadap benar
            dragon.transform.localRotation = Quaternion.identity;
        }

        // 2. Cek saat gambar DI-UPDATE (misal tracking lost lalu found lagi)
        foreach (ARTrackedImage image in obj.updated) {
            if (image.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking) {
                // Pastikan naga terlihat (kadang AR menyembunyikan object saat tracking lost)
                if(dragon != null) dragon.SetActive(true);
            } else {
                // Sembunyikan jika tracking hilang (opsional)
                 if(dragon != null) dragon.SetActive(false);
            }
        }
    }
}