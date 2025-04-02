using Monsters.PathFinder;
using UnityEngine;

namespace Monsters
{
    public class MovementController : MonoBehaviour
    {
        [Header("Scripts")]
        [SerializeField] private PathFinding pathFinding;
        [SerializeField] private GridGen gridGen;

        [Header("MovementSettings")] 
        [SerializeField] private float speed;
        
    }
}