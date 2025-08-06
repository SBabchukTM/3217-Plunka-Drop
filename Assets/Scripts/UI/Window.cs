using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class Window : MonoBehaviour
    {
        protected void Hide()
        {
            Destroy(gameObject);
        }
    }
}