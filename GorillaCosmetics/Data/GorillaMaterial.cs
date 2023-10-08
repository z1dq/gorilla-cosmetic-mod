﻿using System;
using UnityEngine;
using GorillaCosmetics.Utils;

namespace GorillaCosmetics.Data
{
    public class GorillaMaterial : IAsset
    {
        public string FileName { get; }
        public AssetBundle AssetBundle { get; }
        public CosmeticDescriptor Descriptor { get; }

        Material material;

        public GorillaMaterial(string path)
        {
            if (path != "Default")
            {
                try
                {
                    // load
                    FileName = path;
                    var bundleAndJson = PackageUtils.AssetBundleAndJSONFromPackage(FileName);
                    AssetBundle = bundleAndJson.bundle;
                    PackageJSON json = bundleAndJson.json;

                    // get material object and stuff
                    GameObject materialObject = AssetBundle.LoadAsset<GameObject>("_Material");
                    material = materialObject.GetComponent<Renderer>().material;

                    // Make Descriptor
                    Descriptor = PackageUtils.ConvertJsonToDescriptor(json);
                }
                catch (Exception err)
                {
                    // loading failed. that's not good.
                    Debug.Log(err);
                    throw new Exception($"Loading material at {path} failed.");
                }
            }
            else
            {
                // try to load the default material
                Descriptor = new CosmeticDescriptor();
                Descriptor.Name = "Default";
                Descriptor.CustomColors = true;
                material = Resources.Load<Material>("objects/treeroom/materials/lightfur");
            }
        }

        public Material GetMaterial()
		{
            return UnityEngine.Object.Instantiate(material);
		}

        public GameObject GetPreviewOrb(Transform parent)
		{
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            gameObject.layer = 18;
            gameObject.transform.SetParent(parent);
            gameObject.transform.localPosition = Constants.PreviewOrbLocalPosition;
            gameObject.transform.rotation = Constants.PreviewOrbRotation;
            gameObject.transform.localScale = Constants.PreviewOrbLocalScale;
			gameObject.GetComponent<Renderer>().material = material;
            return gameObject;
		}
    }
}
