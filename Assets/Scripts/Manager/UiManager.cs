using UI;
using UnityEngine;

namespace Manager
{
    public class UiManager : MonoBehaviour
    {
        public static UiManager Instance { get; private set; }
        
        public BaseMenu ActiveMenu { get; private set; }
        public BaseMenu ActivePopup { get; private set; }

        private BaseMenu activeMenuPrefab;

        private void Awake()
        {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public BaseMenu Open(BaseMenu pPrefab)
        {
            if (pPrefab.MenuType == MenuType.POPUP) return OpenPopup(pPrefab);

            return OpenMenu(pPrefab);
        }

        public void ClosePopup()
        {
            if (ActivePopup == null) return;
            
            ActivePopup.Desactivate();
            ActivePopup = null;
            
            if (ActiveMenu != null) ActiveMenu.SetClickable(true);
        }

        private BaseMenu OpenMenu(BaseMenu pPrefab)
        {
            ClosePopup();

            if (ActiveMenu != null
                && activeMenuPrefab == pPrefab) return ActiveMenu;
            
            if (ActiveMenu != null) ActiveMenu.Desactivate();

            ActiveMenu = Instantiate(pPrefab, transform, false);
            activeMenuPrefab = pPrefab;
            ActiveMenu.Activate();
            return ActiveMenu;
        }

        private BaseMenu OpenPopup(BaseMenu pPrefab)
        {
            ClosePopup();
            
            if(ActiveMenu != null) ActiveMenu.SetClickable(false);
            
            ActivePopup = Instantiate(pPrefab, transform, false);
            ActivePopup.Activate();
            return ActivePopup;
        }
        
        private void OnDestroy()
        {
            if (Instance == this) {
                Instance = null;
            }
        }
    }
}
