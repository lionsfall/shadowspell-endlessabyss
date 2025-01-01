using Dogabeey.SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dogabeey
{ 
    
    public class HubController : SingletonComponent<HubController>, ISaveable
    {
        public List<UnlockableZones> unlockableZones;
        [Space]
        public Transform hubStartPoint;

        public bool[] unlockedZones;

        public string SaveId => "HubController";

        private void OnEnable()
        {
            foreach (UnlockableZones zone in unlockableZones)
            {
                EventManager.StartListening(zone.unlockEventName, (EventParam e) => UnlockZone(zone));
            }
        }
        private void OnDisable()
        {
            foreach (UnlockableZones zone in unlockableZones)
            {
                EventManager.StopListening(zone.unlockEventName, (EventParam e) => UnlockZone(zone));
            }
        }
        private void Start()
        {
            SaveManager.Instance.Register(this);
            if (!LoadSave())
            {
                unlockedZones = new bool[unlockableZones.Count];
            }
            else
            {
                // If loaded unlockable zones are not equal to the current unlockable zones, add the new ones to the list.
                if (unlockedZones.Length != unlockableZones.Count)
                {
                    bool[] newUnlockedZones = new bool[unlockableZones.Count];
                    for (int i = 0; i < newUnlockedZones.Length; i++)
                    {
                        if(i < unlockedZones.Length)
                        {
                            newUnlockedZones[i] = unlockedZones[i];
                        }
                        else
                        {
                            newUnlockedZones[i] = false;
                        }
                    }
                    unlockedZones = newUnlockedZones;
                }
            }
        }

        private void UnlockZone(UnlockableZones zone)
        {
            zone.enabledObject.SetActive(true);
            zone.disabledObject.SetActive(false);
            if (zone.startPoint)
            {
                hubStartPoint = zone.startPoint;
            }
            zone.onUnlock.Invoke();

            unlockedZones[unlockableZones.IndexOf(zone)] = true;
        }

        public Dictionary<string, object> Save()
        {
            Dictionary<string, object> json = new Dictionary<string, object>();

            for (int i = 0; i < unlockableZones.Count; i++)
            {
                json.Add("zone_" + i, unlockedZones[i]);
            }
            return json;
        }

        public bool LoadSave()
        {
            JSONNode json = SaveManager.Instance.LoadSave(this);

            if (json == null)
            {
                return false;
            }

            for (int i = 0; i < unlockedZones.Length; i++)
            {
                unlockedZones[i] = json["zone_" + i].AsBool;
            }
            return true;
        }

    }

    [System.Serializable]
    public class UnlockableZones
    {
        [Tooltip("The object which is enabled upon unlocking the zone.")]
        public GameObject enabledObject;
        [Tooltip("The object which is disabled upon unlocking the zone.")]
        public GameObject disabledObject;
        [Tooltip("The name of the event which unlocks the zone.")]
        public string unlockEventName;
        [Tooltip("(Optional) Changes the start point of the hub to that location.")]
        public Transform startPoint;
        [Tooltip("The event which is triggered upon unlocking the zone.")]
        public UnityEvent onUnlock;
    }
}