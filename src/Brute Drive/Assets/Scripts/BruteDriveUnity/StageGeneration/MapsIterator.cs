﻿using Google.Maps;
using Google.Maps.Coord;
using Google.Maps.Event;
using Google.Maps.Feature.Style;
using UnityEngine;
using Google.Maps.Unity.Intersections;

namespace BruteDriveUnity.StageGeneration
{
    /// <summary>
    /// Iteratively generates a map using the maps API.
    /// </summary>
    public sealed class MapsIterator : MonoBehaviour
    {
        [Tooltip("The service provider for the maps API.")]
        [SerializeField] private MapsService mapsService = default;


        [Tooltip("The material to use for roads.")]
        [SerializeField] private Material roadMaterial = default;
        [Tooltip("The material to use for buildings.")]
        [SerializeField] private Material buildingMaterial = default;

        [Tooltip("The coordinates of the location to load.")]
        [SerializeField] private LatLng coordinates = default;
        [SerializeField] private int desiredNodes = 300;
        [SerializeField] private float scanStep = 50f;
        [SerializeField] private float maxScan = 1000f;

        public LatLng Coordinates
        {
            get => coordinates;
            set => coordinates = value;
        }


        private float searchScale;

        private IGeneratorListener listener;

        private MapLoadRegion region;

        private GameObjectOptions options;

        public void TryGenerate(IGeneratorListener callbackContext)
        {
            listener = callbackContext;

            // This initializes the maps service
            // with the given coordinates.
            mapsService.InitFloatingOrigin(coordinates);

            // Listen for the state changes in the map loader.
            mapsService.Events.MapEvents.Loaded.AddListener(OnLoaded);
            mapsService.Events.MapEvents.LoadError.AddListener(OnFailed);

            mapsService.Events.RegionEvents.WillCreate.AddListener(
                (WillCreateRegionArgs args) => { args.Cancel = true; });
            /*
            mapsService.Events.ModeledStructureEvents.WillCreate.AddListener(
                (WillCreateModeledStructureArgs args) => { args.Cancel = true; });
            */

            searchScale = scanStep;

            options = new GameObjectOptions()
            {
                ExtrudedStructureStyle = new ExtrudedStructureStyle.Builder
                {
                    WallMaterial = buildingMaterial,
                    RoofMaterial = roadMaterial
                }.Build(),
                ModeledStructureStyle = new ModeledStructureStyle.Builder
                {
                    Material = roadMaterial
                }.Build(),
                RegionStyle = new RegionStyle.Builder
                {
                    FillMaterial = buildingMaterial
                }.Build(),
                AreaWaterStyle = new AreaWaterStyle.Builder
                {
                    FillMaterial = roadMaterial
                }.Build(),
                LineWaterStyle = new LineWaterStyle.Builder
                {
                    Material = roadMaterial
                }.Build(),
                SegmentStyle = new SegmentStyle.Builder
                {
                    Material = roadMaterial,
                    IntersectionMaterial = roadMaterial,
                    Width = 7.0f
                }.Build(),
            };


            region = mapsService.MakeMapLoadRegion()
              .AddCircle(Vector3.zero, searchScale).Load(options);
        }

        private void OnLoaded(MapLoadedArgs args)
        {
            int count = 0;
            foreach (RoadLatticeNode node in mapsService.RoadLattice.Nodes)
                count++;


            if (count > desiredNodes || searchScale > maxScan)
                listener.OnLoaded();
            else
            {
                searchScale += scanStep;
                region.AddCircle(Vector3.zero, searchScale).Load(options);
            }
        }
        private void OnFailed(MapLoadErrorArgs args)
        {
            listener.OnFailed();
        }
    }
}
