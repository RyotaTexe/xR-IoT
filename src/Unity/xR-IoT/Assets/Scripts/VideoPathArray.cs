using System.Collections.Generic;
using UnityEngine;

namespace xRIoT
{
    public class VideoPathArray
    {
        private static readonly string[] videoPaths =
        {
            // 0
            "Videos/IntroducingMicrosoftMesh",

            // 1
            "Videos/InfiniteOffice",

            // 2
            "Videos/iPadProVideo",
        };

        public static IEnumerable<string> VideoPaths => videoPaths;
    }
}