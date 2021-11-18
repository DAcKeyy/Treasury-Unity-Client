using System.Collections.Generic;
using Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

namespace LevelConstructions
{
    public class LevelGenerator
    {
        private BlockBehaviour Block;
        private List<BlockBehaviour> Blocks = new List<BlockBehaviour>();
        private GameObject roomParrent;
        private RoomModel room;

        public LevelGenerator(BlockBehaviour blockObject)
        {
            Block = blockObject;
        }
        
        public (Transform playerSpawnPoint, List<Transform> EnemysSpawnPoints) Generate(RoomSettings settings)
        {

            if (Blocks.Count > 0) { 
                Debug.Log("Комната уже создана в памяти скрипта");
                return (null, null);
            }
            room = new RoomModel(settings);
            

            roomParrent = new GameObject("Room") {
                transform = { position = new Vector3(0,0,0)}
            };
            
            var EnemysSpawnPoints = new List<Transform>();
            var PlayerSpawnPoint = roomParrent.transform;
            
            var WallsParrent = new GameObject("Wall") {
                transform = { parent = roomParrent.transform }
            };
            
            var GroundsParrent = new GameObject("Ground") {
                transform = { parent = roomParrent.transform }
            };
            
            var InteractableParrent = new GameObject("Interactable") {
                transform = { parent = roomParrent.transform }
            };
            
            var SpawnPointsParrent = new GameObject("SpawnPoints") {
                transform = { parent = roomParrent.transform }
            };

            int iterator = 0;
            foreach (var blockModel in room.BlockSpace)
            {
                iterator++;
                
                if (blockModel.blockType == BlockModel.BlockType.Border)
                {
                    var cube = InstantiateBlock(blockModel, WallsParrent.transform);
                    cube.name += $" ID:{iterator}";
                    cube.SwitchMaterial(getUrpMaterial("#AAB7B8"));
                    
                    Blocks.Add(cube);
                }
                if (blockModel.blockType == BlockModel.BlockType.Barricade)
                {
                    var cube = InstantiateBlock(blockModel, InteractableParrent.transform);
                    cube.name += $" ID:{iterator}";
                    cube.SwitchMaterial(getUrpMaterial("#5D6D7E"));
                    
                    Blocks.Add(cube);
                }
                if (blockModel.blockType == BlockModel.BlockType.Ground)
                {
                    var cube = InstantiateBlock(blockModel, GroundsParrent.transform);
                    cube.name += $" ID:{iterator}";
                    cube.SwitchMaterial(getUrpMaterial("#27AE60"));
                    
                    Blocks.Add(cube);
                }
                
                if (blockModel.blockType == BlockModel.BlockType.SpawnPoint_Player)
                {
                    var spawnPointPlayer = new GameObject("SpawnPoint_Player") {
                        tag = "Player Spawn Point",
                        transform = {
                            parent = SpawnPointsParrent.transform,
                            position = blockModel.Position
                        }};
                    PlayerSpawnPoint = spawnPointPlayer.transform;
                    spawnPointPlayer.name += $" ID:{iterator}";
                }
                if (blockModel.blockType == BlockModel.BlockType.SpawnPoint_Enemy)
                {
                    var spawnPointEnemy = new GameObject("SpawnPoint_Enemy") {
                        tag = "Enemy Spawn Point",
                        transform = {
                            parent = SpawnPointsParrent.transform,
                            position = blockModel.Position
                        }};
                    
                    EnemysSpawnPoints.Add(spawnPointEnemy.transform);
                    
                    spawnPointEnemy.name += $" ID:{iterator}";
                }
                if (blockModel.blockType == BlockModel.BlockType.Spikes)
                {
                    var cube = InstantiateBlock(blockModel, InteractableParrent.transform);
                    cube.name += $" ID:{iterator}";
                    cube.SwitchMaterial(getUrpMaterial("#7B241C"));
                    
                    Blocks.Add(cube);
                }
                if (blockModel.blockType == BlockModel.BlockType.Exit)
                {
                    var cube = InstantiateBlock(blockModel, InteractableParrent.transform);
                    cube.name += $" ID:{iterator}";
                    cube.SwitchMaterial(getUrpMaterial("#F7DC6F"));
                    
                    Blocks.Add(cube);
                }
            }

            var meshSurface = roomParrent.AddComponent<NavMeshSurface>();
            meshSurface.BuildNavMesh();

            return (PlayerSpawnPoint, EnemysSpawnPoints);
        }
        
        public BlockBehaviour InstantiateBlock(BlockModel blockModel, Transform parrent)
        {
            var cube = GameObject.Instantiate(Block, blockModel.Position, Quaternion.identity, parrent);
            
            cube.name = blockModel.blockType + " block";
            
            cube.SetActive(true);
            
            foreach (var face in GetNeighbours(blockModel, room.BlockSpace))
            {
                cube.SetActive(face, false);
            }

            return cube;
        }
        
        public Material getUrpMaterial(string HEXcolor)
        {
            var mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            var newCol = Color.gray;
            if (ColorUtility.TryParseHtmlString(HEXcolor, out newCol))
                mat.SetColor("_BaseColor", newCol);
            return mat;
        }

        public List<BlockBehaviour.BlockFaces> GetNeighbours(BlockModel block, BlockModel[,,] blockSpace)
        {
            var neighboursFaces = new List<BlockBehaviour.BlockFaces>();
            
            if(block.Position.x > 0)
                if (blockSpace[(int) block.Position.x - 1, (int) block.Position.y, (int) block.Position.z].blockType == block.blockType)
                    neighboursFaces.Add(BlockBehaviour.BlockFaces.FaceMinusX);

            if(block.Position.x < blockSpace.GetLength(0) - 1)
                if (blockSpace[(int) block.Position.x + 1, (int) block.Position.y, (int) block.Position.z].blockType == block.blockType)
                    neighboursFaces.Add(BlockBehaviour.BlockFaces.FacePlusX);
            
            if(block.Position.y > 0)
                if (blockSpace[(int) block.Position.x, (int) block.Position.y - 1, (int) block.Position.z].blockType == block.blockType)
                    neighboursFaces.Add(BlockBehaviour.BlockFaces.FaceBottom);
            
            if(block.Position.y < blockSpace.GetLength(1) - 1)
                if (blockSpace[(int) block.Position.x, (int) block.Position.y + 1, (int) block.Position.z].blockType == block.blockType)
                    neighboursFaces.Add(BlockBehaviour.BlockFaces.FaceTop);

            if(block.Position.z > 0)
                if (blockSpace[(int) block.Position.x, (int) block.Position.y, (int) block.Position.z - 1].blockType == block.blockType)
                    neighboursFaces.Add(BlockBehaviour.BlockFaces.FaceMinusZ);
            
            if(block.Position.z < blockSpace.GetLength(2) - 1)
                if (blockSpace[(int) block.Position.x, (int) block.Position.y, (int) block.Position.z  + 1].blockType == block.blockType)
                    neighboursFaces.Add(BlockBehaviour.BlockFaces.FacePlusZ);
            
            return neighboursFaces;
        }

        public void Destroy()
        {
            if(Blocks.Count == 0) return;
            
            foreach (var block in Blocks)
            {
#if UNITY_EDITOR
                if(block == null) continue;
                GameObject.DestroyImmediate(block.gameObject);
                GameObject.DestroyImmediate(roomParrent);
#else
                GameObject.Destroy(block.gameObject);
                GameObject.Destroy(roomParrent);
#endif
            }
            
            Blocks.Clear();
        }
    }
}