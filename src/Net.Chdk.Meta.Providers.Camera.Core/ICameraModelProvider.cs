﻿using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.Camera
{
    public interface ICameraModelProvider
    {
        CameraModelData GetModel(string platform, string[] names, ListPlatformData list, TreePlatformData tree, string productName);
    }
}
