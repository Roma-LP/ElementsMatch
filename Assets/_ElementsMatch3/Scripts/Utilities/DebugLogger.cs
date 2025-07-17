using UnityEngine;

namespace _ElementsMatch3.Scripts.Utilities
{
    public class DebugLogger
    {
        private string GetFormatedString(string location, string text)
        {
            return $"<color=pink>[{location}] {text}</color>";
        }
        
        public void PrintException(string location, string text)
        {
            Debug.LogError(GetFormatedString(location,text));
        }

        public void PrintLog(string location, string text)
        {
            Debug.Log(GetFormatedString(location,text));
        }
    }
}