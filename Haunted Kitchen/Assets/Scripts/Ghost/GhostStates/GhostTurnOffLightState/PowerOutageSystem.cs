using UnityEngine;

public class PowerOutageSystem : MonoBehaviour
{
    [Header("ไฟหลักของฉาก (เช่น Directional Light)")]
    public Light mainSceneLight;

    [Header("ไฟรอบตัว Player")]
    public Light playerLight;

    void Start()
    {
        // ตั้งค่าเริ่มต้นตอนเริ่มเกม: ให้ไฟฉากติด และไฟ Player ดับ
        if (mainSceneLight != null) mainSceneLight.enabled = true;
        if (playerLight != null) playerLight.enabled = false;
    }

    void Update()
    {
        // ตัวอย่างทดสอบ: กดปุ่ม 'E' เพื่อจำลองเหตุการณ์ไฟตก/ไฟดับ
        if (Input.GetKeyDown(KeyCode.L))
        {
            TogglePower();
        }
    }

    // ฟังก์ชันสำหรับสับสวิตช์ไฟ
    public void TogglePower()
    {
        if (mainSceneLight != null && playerLight != null)
        {
            // ดับไฟ/เปิดไฟฉากหลัก
            mainSceneLight.enabled = !mainSceneLight.enabled;

            // ตั้งให้ไฟ Player มีสถานะ "ตรงข้าม" กับไฟฉากเสมอ
            // (ถ้าไฟฉากดับ = ไฟ Player ติด / ถ้าไฟฉากติด = ไฟ Player ดับ)
            playerLight.enabled = !mainSceneLight.enabled;
        }
    }
}