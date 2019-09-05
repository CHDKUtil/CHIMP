﻿using Net.Chdk.Meta.Model.Camera.Ps;

namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    sealed class PsBuildProvider : CategoryBuildProvider<PsCameraData, PsCameraModelData, PsCardData>
    {
        public PsBuildProvider(IPsCameraProvider cameraProvider, IPsCameraModelProvider modelProvider, ICameraPlatformProvider platformProvider, ICameraValidator cameraValidator)
            : base(cameraProvider, modelProvider, platformProvider, cameraValidator)
        {
        }

        public override string CategoryName => "PS";
    }
}
