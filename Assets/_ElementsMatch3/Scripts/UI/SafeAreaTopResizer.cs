using UnityEngine;

namespace _ElementsMatch3.Scripts.UI
{
    public class SafeAreaTopResizer : MonoBehaviour
    {
        private void Start()
        {
#if UNITY_IOS
        GetComponent<RectTransform>().anchoredPosition =
 new Vector2(0 , GetComponent<RectTransform>().anchoredPosition.y - ((Screen.height - Screen.safeArea.yMax)/2));
#elif UNITY_ANDROID
            if (SystemInfo.deviceModel.ToLower().Contains("ANE-".ToLower()))
                GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(0, GetComponent<RectTransform>().anchoredPosition.y - 85);
            else
                GetComponent<RectTransform>().anchoredPosition = new Vector2(0,
                    GetComponent<RectTransform>().anchoredPosition.y -  ((Screen.height - Screen.safeArea.yMax)/2));
#endif
        }
    }
}

