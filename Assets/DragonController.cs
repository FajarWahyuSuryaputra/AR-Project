using UnityEngine;
using System.Collections;

public class DragonController : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 0.5f; // Kecepatan gerak (kecil karena skala AR)
    public float rotateSpeed = 10f;

    [Header("References")]
    private Joystick joystick; // Referensi ke script Joystick Pack
    private Animator animator; // Untuk animasi
    
    void Start()
    {
        // 1. Cari Joystick secara otomatis saat Naga muncul
        joystick = FindObjectOfType<Joystick>(); 
        
        // 2. Ambil komponen Animator
        animator = GetComponent<Animator>();

        if (joystick == null)
        {
            Debug.LogError("Joystick tidak ditemukan di Scene!");
        }

        transform.SetParent(null);
    }

    void Update()
    {
        if (joystick == null) return;

        // Ambil input dari Joystick
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        // Buat vector gerakan (bergerak di sumbu X dan Z lantai)
        Vector3 direction = new Vector3(horizontal, 0, vertical);

        // Cek jika ada input gerakan
        if (direction.magnitude >= 0.1f)
        {
            // 1. Gerakkan Naga
            // Menggunakan Translate (World Space)
            transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

            // 2. Putar badan Naga ke arah jalan
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);

            // 3. Jalankan Animasi Jalan (Pastikan parameter 'IsWalking' ada di Animator)
            if(animator) animator.SetBool("IsWalking", true);
        }
        else
        {
            // Stop Animasi
            if(animator) animator.SetBool("IsWalking", false);
        }
    }
}