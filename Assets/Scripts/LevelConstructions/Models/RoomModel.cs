using System.Collections.Generic;
using Data;
using UnityEngine;

namespace LevelConstructions
{
    public class RoomModel
    {
        public readonly BlockModel[,,] BlockSpace;

        public RoomModel(RoomSettings settings)
        {
            var walls = 2;
            var height = 5;
            var width = 11;

            BlockSpace = new BlockModel[width + walls, height, settings.roomLenght + walls];
            
            InitBlockSpace(ref BlockSpace);
            CreateBorders(ref BlockSpace);
            CreateExit(ref BlockSpace);
            CreateBasicGround(ref BlockSpace);
            CreateBarricade(ref BlockSpace, settings);
            CreatePlayerSpawnpoint(ref BlockSpace);
            CreateEnemiesSpawnpoint(ref BlockSpace, settings);
        }

        public void InitBlockSpace(ref BlockModel[,,] roomSpace)
        {
            for (int height = 0; height < roomSpace.GetLength(1); height++)
                for (int width = 0; width < roomSpace.GetLength(0); width++)
                    for (int lenght = 0; lenght < roomSpace.GetLength(2); lenght++)
                    {
                        roomSpace[width, height, lenght].blockType = BlockModel.BlockType.Empty;
                        roomSpace[width, height, lenght].Position = new Vector3(width, height, lenght);
                    }
        }
        
        public void ClearBlockSpace(ref BlockModel[,,] roomSpace)
        {
            for (int height = 0; height < roomSpace.GetLength(1); height++)
                for (int width = 0; width < roomSpace.GetLength(0); width++)
                    for (int lenght = 0; lenght < roomSpace.GetLength(2); lenght++)
                    {
                        roomSpace[width, height, lenght].blockType = BlockModel.BlockType.Empty;
                    }
        }
        
        public void CreateBorders(ref BlockModel[,,] roomSpace)
        {                    
            for (int height = 0; height < roomSpace.GetLength(1); height++)
                for (int width = 0; width < roomSpace.GetLength(0); width++)
                    for (int lenght = 0; lenght < roomSpace.GetLength(2); lenght++)
                    {
                        if (lenght == 0) 
                            roomSpace[width, height, lenght].blockType = BlockModel.BlockType.Border;
                        
                        if (lenght == roomSpace.GetLength(2) - 1)
                            roomSpace[width, height, lenght].blockType = BlockModel.BlockType.Border;
                        
                        if(width == 0) 
                            roomSpace[width, height, lenght].blockType = BlockModel.BlockType.Border;
                        
                        if(width == roomSpace.GetLength(0) - 1) 
                            roomSpace[width, height, lenght].blockType = BlockModel.BlockType.Border;
                    }
        }
        
        public void CreateExit(ref BlockModel[,,] roomSpace)
        {                    
            for (int height = 0; height < roomSpace.GetLength(1); height++)
                for (int width = 0; width < roomSpace.GetLength(0); width++)
                    for (int lenght = 0; lenght < roomSpace.GetLength(2); lenght++)
                    {
                        if (width == 5 || width == 6 || width == 7)
                            if (lenght == roomSpace.GetLength(2) - 1 && height > 2)
                            {
                                roomSpace[width, height, lenght].blockType = BlockModel.BlockType.Exit;
                            }
                    }
        }
        
        public void CreateBarricade(ref BlockModel[,,] roomSpace, RoomSettings roomData)
        {
            for(int coordinatesLenght = 0; coordinatesLenght < roomData.RoomGrid.GetLength(0); coordinatesLenght++)
                for (int coordinatesWight = 0; coordinatesWight < roomData.RoomGrid.GetLength(1); coordinatesWight++)
                {
                    if (roomData.RoomGrid[coordinatesLenght, coordinatesWight] == BlockModel.BlockType.Barricade)
                    {
                        roomSpace[coordinatesWight + 1, 3, coordinatesLenght + 1].blockType = BlockModel.BlockType.Barricade;
                        roomSpace[coordinatesWight + 1, 4, coordinatesLenght + 1].blockType = BlockModel.BlockType.Barricade;
                    }
                }
        }
        
        public void CreatePlayerSpawnpoint(ref BlockModel[,,] roomSpace)
        {                    
            for (int height = 0; height < roomSpace.GetLength(1); height++)
                for (int width = 0; width < roomSpace.GetLength(0); width++)
                    for (int lenght = 0; lenght < roomSpace.GetLength(2); lenght++)
                    {
                        if (width == Mathf.Round(roomSpace.GetLength(0) / 2))
                            if (lenght == 3)
                                if (height == 3)
                                {
                                    roomSpace[width, height, lenght].blockType = BlockModel.BlockType.SpawnPoint_Player;
                                }
                    }
        }

        public void CreateEnemiesSpawnpoint(ref BlockModel[,,] roomSpace, RoomSettings settings)
        {
            for (int iterator = 0; iterator < settings.enemies.Count; iterator++)
            {
                var possiblePlaces = new List<BlockModel>();

                // 25 / 3 === (8...25 lenght is spawnable for enemies)
                int thirdOfLenght = (int)Mathf.Round((roomSpace.GetLength(2) - 1) / 4) ;

                for (int height = 0; height < roomSpace.GetLength(1); height++)
                    for (int width = 0; width < roomSpace.GetLength(0); width++)
                        for (int lenght = 0; lenght < roomSpace.GetLength(2); lenght++)
                        {
                            if(height == 3)
                                if (lenght >= thirdOfLenght) 
                                    if(roomSpace[width, height - 1, lenght].blockType == BlockModel.BlockType.Ground)
                                        if (roomSpace[width, height, lenght].blockType == BlockModel.BlockType.Empty)
                                        {
                                            possiblePlaces.Add(roomSpace[width, height, lenght]);
                                        }
                        }

                var possiblePlace = possiblePlaces[UnityEngine.Random.Range(0, possiblePlaces.Count - 1)];
                
                roomSpace[(int)possiblePlace.Position.x, (int)possiblePlace.Position.y, (int)possiblePlace.Position.z]
                    .blockType = BlockModel.BlockType.SpawnPoint_Enemy;
            }
        }
        
        public void CreateBasicGround(ref BlockModel[,,] roomSpace)
        {
            for (int height = 0; height < roomSpace.GetLength(1); height++)
                for (int width = 0; width < roomSpace.GetLength(0); width++)
                    for (int lenght = 0; lenght < roomSpace.GetLength(2); lenght++)
                    {
                        if (height < 3)
                            if(roomSpace[width, height, lenght].blockType == BlockModel.BlockType.Empty)
                            {
                                roomSpace[width, height, lenght].blockType = BlockModel.BlockType.Ground;
                            }
                    }
        }
    }
}