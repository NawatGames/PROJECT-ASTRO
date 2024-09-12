using JetBrains.Annotations;
using Unity.VisualScripting;

namespace TestingQuarentine
{
    using System.Collections.Generic;
    using UnityEngine;

    public class TestQuarentineManager
    {
        [SerializeField] [CanBeNull] public List<GameObject> roomAcessibleList;
        [SerializeField] [CanBeNull] public List<GameObject> roomInacessableList;
        [SerializeField] [CanBeNull] public TestRoomQuarentine quarantinedRoom;
        [SerializeField] private float clickDelay; // Delay configurável
        
        private void Start()
        {
            foreach (GameObject room in roomAcessibleList)
            {
                room.SetActive(false);
            }
        }
        
        
        
    }
}