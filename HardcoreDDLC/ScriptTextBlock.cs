using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace HardcoreDDLC
{
    class ScriptTextBlock : OutlinedTextBlock
    {
        public List<string> Scripts = new List<string>();

        private int ScriptIndex = 0;
        private int ScriptCharIndex = 0;
        private bool isAnimating = false;
        private DispatcherTimer timer = new DispatcherTimer();

        public double AnimSpeed
        {
            get => timer.Interval.TotalMilliseconds;
            set => timer.Interval = TimeSpan.FromMilliseconds(value);
        }

        public ScriptTextBlock()
        {
            // Timer Setup
            AnimSpeed = 20;
            timer.Tick += Animation;
        }

        public ScriptTextBlock(IEnumerable<string> scripts) : this()
        {
            Scripts = new List<string>(scripts);
        }

        public void Input()
        {
            if(isAnimating)
            {
                isAnimating = false;
                Text = Scripts[ScriptIndex++];
            }
            else if (ScriptIndex < Scripts.Count)
            {
                isAnimating = true;
                ScriptCharIndex = 0;
                timer.Start();
            }
        }

        private void Animation(object sender, EventArgs e)
        {
            if (isAnimating)
            {
                var currentScript = Scripts[ScriptIndex];

                if (currentScript.Length > ++ScriptCharIndex)
                {
                    Text = currentScript.Remove(ScriptCharIndex);
                }
                else
                {
                    Text = currentScript;
                    isAnimating = false;
                    ScriptIndex++;
                    timer.Stop();
                }
            }
        }
    }
}
