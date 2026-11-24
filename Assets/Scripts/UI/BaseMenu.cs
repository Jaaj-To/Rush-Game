using UnityEngine;

namespace UI
{
    public abstract class BaseMenu : MonoBehaviour
    {
        [SerializeField] private MenuType menuType = MenuType.MENU;

        public MenuType MenuType => menuType;

        public virtual void Activate()
        {
            gameObject.SetActive(true);
        }

        public virtual void Desactivate()
        {
            Destroy(gameObject);
        }

        public void SetClickable(bool pClickable)
        {
        }
    }
}
