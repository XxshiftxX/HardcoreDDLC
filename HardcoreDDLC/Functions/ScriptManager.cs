using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

namespace HardcoreDDLC
{
    public partial class MainWindow
    {
        private readonly DispatcherTimer Animator = new DispatcherTimer();

        private bool IsScriptRunning;
        private string CurrentScript;
        private int ScriptCharIndex = 0;

        private void InitializeScript()
        {
            AnimSpeed = 20;
            Animator.Tick += AnimateScript;
        }
        
        private double AnimSpeed
        {
            get => Animator.Interval.TotalMilliseconds;
            set => Animator.Interval = TimeSpan.FromMilliseconds(value);
        }

        private void AnimateScript(object e, EventArgs arg)
        {
            if (CurrentScript.Length > ++ScriptCharIndex)
                ScriptTextBlock.Text = CurrentScript.Remove(ScriptCharIndex);
            else
                FinishScript();
        }

        private void FinishScript()
        {
            ScriptTextBlock.Text = CurrentScript;
            IsScriptRunning = false;
            Animator.Stop();
            ScriptCharIndex = 0;
            actionIndex++;
        }

        private void RegistScript(string script)
        {
            IsScriptRunning = true;
            CurrentScript = script;
            
            Animator.Start();
        }
    }
}
