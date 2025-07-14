using UnityEngine;

namespace _ElementsMatch3.Scripts.Utilities
{
    public class DebugLogger
    {
        public void PrintException(string location, string text)
        {
            Debug.LogError($"<color=pink>[{location}] {text}</color>");
        }
        
        public enum ExceptionLocation
        {
            
        }
    }
}