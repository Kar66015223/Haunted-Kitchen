using UnityEngine;

public class PlayerDustEffect : MonoBehaviour
{
    [Header("References")]
    [Tooltip("ลาก PlayerController มาใส่ หรือปล่อยว่างเดี๋ยวมันหาเอง")]
    public PlayerController playerController;
    public ParticleSystem dustParticle;

    [Header("Settings")]
    public float walkRate = 5f;   // จำนวนควันตอนเดิน
    public float runRate = 30f;   // จำนวนควันตอนวิ่ง (ให้เยอะๆ เลยจะได้เห็นชัด)

    private ParticleSystem.EmissionModule emissionModule;
    private CharacterController charController;

    void Start()
    {
        // 1. หา PlayerController อัตโนมัติถ้าลืมลากใส่
        if (playerController == null)
            playerController = GetComponent<PlayerController>();

        // 2. หา CharacterController เพื่อเช็คความเร็วขยับจริง
        charController = GetComponent<CharacterController>();

        // 3. เก็บ module emission ไว้แก้ไขค่า
        if (dustParticle != null)
        {
            emissionModule = dustParticle.emission;
        }
    }

    void Update()
    {
        if (dustParticle == null || playerController == null) return;

        // เช็คว่าตัวละครขยับอยู่ไหม (ดูจาก Velocity ของ CharacterController แม่นยำสุด)
        bool isMoving = charController.velocity.sqrMagnitude > 0.1f;

        if (isMoving)
        {
            // เช็คสถานะวิ่ง จากตัวแปร IsRunning ใน Script เก่าของคุณ
            if (playerController.IsRunning)
            {
                // วิ่ง: ใส่ควันเยอะๆ
                emissionModule.rateOverTime = runRate;
            }
            else
            {
                // เดิน: ใส่ควันน้อยๆ
                emissionModule.rateOverTime = walkRate;
            }
        }
        else
        {
            // ยืนเฉยๆ: ปิดควัน
            emissionModule.rateOverTime = 0f;
        }
    }
}