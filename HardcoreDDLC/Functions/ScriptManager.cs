using System;
using System.Windows.Threading;

namespace HardcoreDDLC
{
    public partial class MainWindow
    {
        private readonly DispatcherTimer _animator = new DispatcherTimer();

        private bool _isScriptRunning;
        private string _currentScript;
        private int _scriptCharIndex = 0;

        private void InitializeScript()
        {
            AnimSpeed = 20;
            _animator.Tick += AnimateScript;
        }
        
        private double AnimSpeed
        {
            get => _animator.Interval.TotalMilliseconds;
            set => _animator.Interval = TimeSpan.FromMilliseconds(value);
        }

        private void AnimateScript(object e, EventArgs arg)
        {
            if (_currentScript.Length > ++_scriptCharIndex)
                ScriptTextBlock.Text = _currentScript.Remove(_scriptCharIndex);
            else
                FinishScript();
        }

        private void FinishScript()
        {
            ScriptTextBlock.Text = _currentScript;
            _isScriptRunning = false;
            _animator.Stop();
            _scriptCharIndex = 0;
            _actionIndex++;
        }

        private void RegistScript(string script)
        {
            _isScriptRunning = true;
            _currentScript = script;
            
            _animator.Start();
        }
    }
}
