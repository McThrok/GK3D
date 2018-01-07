using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3D.Graphics.SceneComponents
{
    public abstract class SceneScenario
    {
        public abstract void Process(float deltaTime);
    }
    public class TestSceneScenario : SceneScenario
    {
        public override void Process(float deltaTime)
        {
        }
    }
}
