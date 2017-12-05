using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FirstProject
{
    internal class LightsManager
    {
        List<DirLight> lights = new List<DirLight>();
        public List<DirLight> Lights => lights;

        public LightsManager(bool prepareLights)
        {
            if (prepareLights)
                PrepareLights();
        }

        private void PrepareLights()
        {
            lights.Add(new DirLight()
            {
                Direction = new Vector3(0.5f, 0, 0),
                DiffuseColor = new Vector3(1, 0, 0),
                SpecularColor = new Vector3(0, 1, 0)
            });
            lights.Add(new DirLight()
            {
                Direction = new Vector3(0, 0.5f, 0),
                DiffuseColor = new Vector3(0, 1, 0),
                SpecularColor = new Vector3(0, 0, 1)
            });
            lights.Add(new DirLight()
            {
                Direction = new Vector3(0, 0, -1),
                DiffuseColor = new Vector3(0, 0, 1),
                SpecularColor = new Vector3(1, 0, 0)
            });
        }
    }
}