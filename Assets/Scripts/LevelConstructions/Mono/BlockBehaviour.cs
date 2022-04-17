using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Treasury.LevelConstructions.Mono
{
    public class BlockBehaviour : MonoBehaviour
    {
        public Face[] Faces = new Face[6];
        
        public void SwitchMaterial(BlockFaces face, Material newMaterial)
        {
            foreach (var keys in Faces)
                if (keys.FaceKey == face)
                    if(keys.FaceObject.gameObject.TryGetComponent(out Renderer render))
                        render.material = newMaterial;
        }
        
        public void SwitchMaterial(Material newMaterial)
        {
            SwitchMaterial(BlockFaces.FaceTop, newMaterial);
            SwitchMaterial(BlockFaces.FaceMinusX, newMaterial);
            SwitchMaterial(BlockFaces.FacePlusX, newMaterial);
            SwitchMaterial(BlockFaces.FaceMinusZ, newMaterial);
            SwitchMaterial(BlockFaces.FacePlusZ, newMaterial);
            SwitchMaterial(BlockFaces.FaceBottom, newMaterial);
        }
    
        public void SetActive(BlockFaces face, bool isActive)
        {
            foreach (var keys in Faces)
                if (keys.FaceKey == face)
                    keys.FaceObject.gameObject.SetActive(isActive);
        }
        
        public void SetActive(bool isActive)
        {
            SetActive(BlockFaces.FaceTop, isActive);
            SetActive(BlockFaces.FaceMinusX, isActive);
            SetActive(BlockFaces.FacePlusX, isActive);
            SetActive(BlockFaces.FaceMinusZ, isActive);
            SetActive(BlockFaces.FacePlusZ, isActive);
            SetActive(BlockFaces.FaceBottom, isActive);
        }
        
        public enum BlockFaces
        {
            FaceTop,
            FaceMinusX,
            FacePlusX,
            FaceMinusZ,
            FacePlusZ,
            FaceBottom
        }
        
        [Serializable]
        public struct Face
        {
            public BlockFaces FaceKey;
            public GameObject FaceObject;
        }
    }
    
    #region Editor
    [CustomEditor(typeof(BlockBehaviour))]
    public class BlockBehaviourEditor : Editor
    {
        private BlockBehaviour _blockBehaviour;
        private List<(bool enabled, BlockBehaviour.Face face)> isShouldBeEnabled 
            = new List<(bool enabled, BlockBehaviour.Face face)>();
        
        public void OnEnable()
        {
            _blockBehaviour = (BlockBehaviour)target;

            foreach (var faceObject in _blockBehaviour.Faces)
            {
                isShouldBeEnabled.Add((faceObject.FaceObject.activeSelf, faceObject));
                faceObject.FaceObject.SetActive(true);
            }
        }
        
        public void OnDisable()
        {
            foreach (var element in isShouldBeEnabled)
            {
                if(element.face.FaceObject == null) continue;
                element.face.FaceObject.SetActive(element.enabled);
            }
        }
    }
    #endregion
}
