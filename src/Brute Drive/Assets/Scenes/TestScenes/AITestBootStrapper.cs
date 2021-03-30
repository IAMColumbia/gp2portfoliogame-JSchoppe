using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BruteDrive.Designer.Unity.Vehicles;

namespace BruteDrive.BootStrappers
{
    /// <summary>
    /// The startup logic for the AI test scene.
    /// </summary>
    public sealed class AITestBootStrapper : MonoBehaviour
    {
        [SerializeField] private VehicleInstance player = default;


        private void Start()
        {
            // Initialize the player instance.
            player.Instance();
        }
    }
}
