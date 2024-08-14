using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    // 버튼에 할당할 메서드
    public void Resetbutton()
    {
        // PlayerPrefs 전체 삭제
        PlayerPrefs.DeleteAll();

        // 변경 사항을 저장
        PlayerPrefs.Save();

        // 확인 메시지 로그 (필요시 UI로도 출력 가능)
        Debug.Log("All PlayerPrefs have been cleared.");
    }
}
