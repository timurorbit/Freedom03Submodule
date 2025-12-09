using System;
using UnityEngine;

namespace _Game.Scripts.Behaviours
{
    public class CharacterBehaviour : MonoBehaviour
    {
        [Header("Components")] [SerializeField]
        private Animator animator;

        private void Awake()
        {
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
        }

        public GameObject FindClosestEntrance()
        {
           var entrance = FindAnyObjectByType<Entrance>().gameObject;
           return entrance;
        }
        
        
    }
}