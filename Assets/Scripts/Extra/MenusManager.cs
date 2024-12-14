using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MenusManager : MonoBehaviour
{
   [SerializeField] private GameObject statsMenuBackground_Right;
   [SerializeField] private GameObject statsMenuBackground_Left;

   [SerializeField] private Button openStatsMenuButton;
   [SerializeField] private Button closeStatsMenuButton;
   
   [SerializeField] private GameObject statsMenu;
   [SerializeField] private GameObject upgradeAmountGameObject;
   [SerializeField] private GameObject currentLevelGameObject;

   [SerializeField] private GameObject[] upgradeIcons;
   [SerializeField] private UpgradesManager upgradesManager;
   private void Start()
   {
       InitializeStatsMenu();
   }

   private void InitializeStatsMenu()
   {
       statsMenuBackground_Right.SetActive(false);
       statsMenuBackground_Left.SetActive(false);

       statsMenuBackground_Right.transform.localScale = new Vector3(0f, 1f, 1f);
       statsMenuBackground_Left.transform.localScale = new Vector3(0f, 1f, 1f);
       
       closeStatsMenuButton.gameObject.SetActive(false);
       closeStatsMenuButton.gameObject.transform.localScale = Vector3.zero;
       
       statsMenu.SetActive(false);
       upgradeAmountGameObject.SetActive(false);

       foreach (var upgradeIcon in upgradeIcons)
       {
           upgradeIcon.SetActive(false);
       }
   }

   public void OpenStatsMenu()
   {
       openStatsMenuButton.interactable = false;
       closeStatsMenuButton.interactable = false;
       
       closeStatsMenuButton.gameObject.SetActive(true);
       
      statsMenuBackground_Right.SetActive(true);
      statsMenuBackground_Left.SetActive(true);

      currentLevelGameObject.transform.DOScale(0f, 0.5f).OnComplete(() =>
      {
          closeStatsMenuButton.gameObject.transform.DOScale(1f, 0.5f).OnComplete(() =>
          {
               closeStatsMenuButton.interactable = true;
          });
      });

      statsMenuBackground_Right.transform.DOScaleX(1f, 0.5f);
      statsMenuBackground_Left.transform.DOScaleX(1f, 0.5f).OnComplete(() =>
      {
          statsMenu.SetActive(true);
          upgradeAmountGameObject.SetActive(true);
          
          foreach (var upgradeIcon in upgradeIcons)
          {
              upgradeIcon.SetActive(true);
          }
          
          upgradesManager.RemoteUpdateUI();
      });
   }

   public void CloseStatsMenu()
   {
       closeStatsMenuButton.interactable = false;
       
       foreach (var upgradeIcon in upgradeIcons)
       {
           upgradeIcon.SetActive(false);
       }
       
       statsMenu.SetActive(false);
       upgradeAmountGameObject.SetActive(false);

       closeStatsMenuButton.gameObject.transform.DOScale(0f, 0.5f).OnComplete(() =>
       {
           currentLevelGameObject.transform.DOScale(1f, 0.5f).OnComplete(() =>
           {
               openStatsMenuButton.interactable = true;
           });
       });
       
       statsMenuBackground_Right.transform.DOScaleX(0f, 0.5f);
       statsMenuBackground_Left.transform.DOScaleX(0f, 0.5f).OnComplete(() =>
       {
           statsMenuBackground_Right.SetActive(false);
           statsMenuBackground_Left.SetActive(false);
       });
   }
}
