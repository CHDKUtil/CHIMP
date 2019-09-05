﻿using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;
using Net.Chdk.Meta.Model.Platform;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.Camera
{
    public interface IBuildProvider
    {
        IDictionary<string, ICameraData> GetCameras(IDictionary<string, PlatformData> platforms, IDictionary<string, ListPlatformData> list, IDictionary<string, TreePlatformData> tree,
            string productName);
    }
}
